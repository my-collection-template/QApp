﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using KC.InstallServiceNS;
using QApp;

namespace AppFramework {
    [System.ComponentModel.DesignerCategory("")]
    public class Program : ServiceBase {

        static void Main(string[] args) {
            try {
                if (args != null && args.Any(x => x == "/i")) {
                    InstallService.Install(App.Config.AppName);
                    return;
                }
                if (args != null && args.Any(x => x == "/s")) {
                    InstallService.TryStopService(App.Config.AppName);
                    return;
                }
                if (Environment.UserInteractive) {
                    var app = new App();
                    app.Run().Wait();
                }
                else {
                    ServiceBase.Run(new Program());
                }
            }
            catch (Exception ex) {
                File.AppendAllText(Path.Combine("C:\\", "failedToStart.txt"), ex.ToString());
            }
        }

        App app;
        public Program() {
            this.CanHandlePowerEvent = false;
            this.CanStop = true;
            this.ServiceName = App.Config.AppName;
            app = new App();
        }

        protected override void OnStart(string[] args) {
            base.OnStart(args);

            //Don't log this as the log isn't started yet!

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            app.Run();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        protected override void OnStop() {
            base.OnStop();
            app.Dispose();
            //Give the app up to 5 seconds to shut down.
            //This is coincidentally the default amount of time windows gives us
            //when the PC is rebooting, so this can't go any higher.
            System.Threading.Thread.Sleep(5000);
        }

        protected override void OnShutdown() {
            base.OnShutdown();
            app.Dispose();
            //Give the app up to 5 seconds to shut down.
            //This is coincidentally the default amount of time windows gives us
            //when the PC is rebooting, so this can't go any higher.
            System.Threading.Thread.Sleep(5000);
        }

    }

    [System.ComponentModel.DesignerCategory("")]
    [RunInstaller(true)]
    public class TheServiceInstaller : Installer {
        public TheServiceInstaller() {
            var serviceProcessInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
            serviceInstaller.DisplayName = App.Config.AppName;
            serviceInstaller.Description = App.Config.AppName;
            serviceInstaller.StartType = ServiceStartMode.Automatic;
            serviceInstaller.DelayedAutoStart = true;
            serviceInstaller.ServiceName = App.Config.AppName;
            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
