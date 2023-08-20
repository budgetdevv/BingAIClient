using System;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BingAIClient // Note: actual namespace depends on the project name.
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            builder.WebHost.UseElectron(args);

            builder.Services.AddElectron();

            var host = builder.Build();

            await host.StartAsync();

            var windowsManager = Electron.WindowManager;

            const string TITLE = "Bing AI Client";
            
            var window = await windowsManager.CreateWindowAsync(new BrowserWindowOptions()
            {
                TitleBarStyle = TitleBarStyle.defaultStyle,
                Title = TITLE
            });
            
            var os = Environment.OSVersion.Platform;

            string userAgent;

            switch (os) //https://www.whatismybrowser.com/guides/the-latest-user-agent/edge
            {
                default:
                case PlatformID.Win32NT:
                {
                    userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36 Edg/112.0.1722.48";
                    break;
                }
                
                case PlatformID.MacOSX:
                case PlatformID.Unix:    
                {
                    userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 13_3_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36 Edg/112.0.1722.48";
                    break;
                }
                
                case PlatformID.Xbox:
                {
                    userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; Xbox; Xbox One) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36 Edge/44.18363.8131";
                    break;
                }
            }

            var app = Electron.App;
            
            app.UserAgentFallback = userAgent;
            
            window.LoadURL("https://www.bing.com/rewards/authcheck?ru=%2Fmsrewards%2Fapi%2Fv1%2Fenroll%3Fpubl%3DBINGIP%26crea%3DMY00IA%26pn%3Dbingcopilotwaitlist%26partnerId%3DBingRewards%26pred%3Dtrue%26wtc%3DChatPaywall%26sessionId%3D3738576C319666AE1E81459B303A6708%26ru%3D%252fsearch%253fq%253dBing%252bAI%2526showconv%253d1%2526FORM%253dhpcodx%2526wlsso%253d0%2526scdexwlcs%253d1%2526scdexwlispw%253d1", new LoadURLOptions()
            {
                UserAgent = userAgent
            });

            window.OnPageTitleUpdated += _ =>
            {
                window.SetTitle(TITLE);
            };

            app.WindowAllClosed += () =>
            {
                Electron.App.Exit();
            };

            await host.WaitForShutdownAsync();
        }
    }
}