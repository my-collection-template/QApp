﻿using QApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceInstallNS {
    public class ServiceStarter {
        public bool StartTheService() {
            ServiceController service = null;
            try {
                service = ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == App.Config.AppName);
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to Find the {App.Config.AppName} Service: " + ex.Message);
                return false;
            }
            if (service == null) {
                Console.WriteLine($"The {App.Config.AppName} Service is not installed.");
                return false;
            }
            try {
                Console.WriteLine("Service Status: " + service.Status.ToString("g"));
                if (service.Status == ServiceControllerStatus.Running) {
                    Console.WriteLine("Service was already running.");
                    return true;
                }
                try {
                    Console.WriteLine("Starting Service");
                    service.Start();
                    var sleepDuration = 200;
                    var tryForMils = 10000;
                    for (int i = 0; i < tryForMils; i += sleepDuration) {
                        Console.Write(".");
                        service.Refresh();
                        if (service.Status == ServiceControllerStatus.Running) {
                            break;
                        }
                        //We wouldn't do this in a real application, but this is just an installer:
                        Thread.Sleep(sleepDuration);
                    }
                    Console.WriteLine();
                    if (service.Status != ServiceControllerStatus.Running) {
                        Console.WriteLine("Failed to start service after " + (tryForMils / 1000) + " seconds.");
                        Console.WriteLine($"Try manually starting the {App.Config.AppName} service.");
                        Console.WriteLine("If the service is locked in a starting/stopping state, you may need to restart the computer.");
                        return false;
                    }
                    return true;
                }
                catch (Exception ex) {
                    Console.WriteLine("Failed to start service: " + ex.Message);
                    Console.WriteLine($"Try manually starting the {App.Config.AppName} service.");
                    return false;
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to Find {App.Config.AppName} Service Status: " + ex.ToString());
                Console.WriteLine($"Try manually starting the {App.Config.AppName} service.");
                return false;
            }
        }
    }
}
