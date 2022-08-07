using System;
// using System.Type;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;
using Common;

namespace Server.Integration.Test {

    public class Service : IDisposable { 

        private bool _running = false;
        private Process process = null;

        public int Monitoring = 1;
        public bool isRunning() {
            _running = checkalive();
            return _running;
        }

        public string getProjectName() {
            return "Server";
        }

        public string getBaseUrl() {
            return "http://localhost:5000/";
        }


        /**
        get echo responce from MitmMockService process
        */
        public bool checkalive() {
            try {
                Http.get( getBaseUrl() );
                Console.WriteLine( "Service.checkalive: alive");
                return true;
            } catch(EHttp404Error){
                return true; // service is up anyway
            } catch( EHttp e ) {
                Console.WriteLine( "Service.checkalive: "+e.Message );
            }
            return false;
        }

        public void start() {

            Console.WriteLine("================ starting ServerAPITelegram  ============");
            if( process == null ) {
                process = new Process();
            }
            // Process process = new Process();
            // string runcmd = "run --no-build --project "
            string runcmd = "run --project "
                +Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetParent(
                    Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).ToString()
                    ).ToString()).ToString()).ToString()).ToString()
                +System.IO.Path.DirectorySeparatorChar+getProjectName()
                +System.IO.Path.DirectorySeparatorChar+getProjectName()+".csproj";
            Console.WriteLine("Calling: dotnet "+runcmd);
            process.StartInfo = new ProcessStartInfo("dotnet", runcmd );
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            int maxreads = 100;
            while( !checkalive() && maxreads > 0) {
                Console.WriteLine("Assuming build not finished, wait and retry ..");
                System.Threading.Thread.Sleep(1000);
                maxreads--;
            }
            Console.WriteLine("================ ServerAPITelegram started ============");
        }

        public void stop() {
            // try {
            try {
                Console.WriteLine("================ Stopping Service ====================");
                // process.Close();
                process.Kill( true );
                process.Dispose();
                process = null;
                int maxreads = 100;
                while( checkalive() && maxreads > 0) {
                    System.Threading.Thread.Sleep(1000);
                    maxreads--;
                }
            } catch(System.NullReferenceException) {}
            // } catch(System.InvalidOperationException) {}
        }


        public void Dispose() {
            Console.WriteLine("============ disposing service =============");
            stop();
            Console.WriteLine("============ end disposing service =============");
        }
    }
}