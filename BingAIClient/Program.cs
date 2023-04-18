using System;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BingAIClient // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private static async Task Main(string[] Args)
        {
            var Builder = WebApplication.CreateBuilder();

            Builder.WebHost.UseElectron(Args);

            Builder.Services.AddElectron();

            var App = Builder.Build();

            await App.StartAsync();

            var WindowManager = Electron.WindowManager;

            // BrowserWindow Window;
            //
            // try
            // {
            //     Window = await WindowManager.CreateWindowAsync();
            // }
            //
            // catch (Exception Ex)
            // {
            //     Console.WriteLine(Ex);
            // }

            var Window = await WindowManager.CreateWindowAsync();
            
            Window.OnReadyToShow += () =>
            {
                var OS = Environment.OSVersion.Platform;

                string UserAgent;

                switch (OS) //https://www.whatismybrowser.com/guides/the-latest-user-agent/edge
                {
                    default:
                    case PlatformID.Win32NT:
                    {
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36 Edg/112.0.1722.48";
                        break;
                    }
                
                    case PlatformID.MacOSX:
                    case PlatformID.Unix:    
                    {
                        UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_3_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36 Edg/112.0.1722.48";
                        break;
                    }
                
                    case PlatformID.Xbox:
                    {
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Xbox; Xbox One) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36 Edge/44.18363.8131";
                        break;
                    }
                }
            
                Window.LoadURL("https://www.bing.com/search?q=Bing+AI&showconv=1", new LoadURLOptions()
                {
                    UserAgent = UserAgent
                });
                
                Window.Show();
            };

            await App.WaitForShutdownAsync();
        }
    }
}