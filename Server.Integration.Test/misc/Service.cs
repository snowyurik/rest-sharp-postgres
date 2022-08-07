using System;
using System.Diagnostics;
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
            } catch( EHttp ) {
                // Console.WriteLine( "Service.checkalive: "+e.Message );
            }
            return false;
        }

        public void start() {

            Console.WriteLine("================ starting "+getProjectName()+"  ============");
            if( process == null ) {
                process = new Process();
            }
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
                Console.WriteLine("No responce yet, assuming build not finished, wait and retry ..");
                System.Threading.Thread.Sleep(1000);
                maxreads--;
            }
            Console.WriteLine("================ "+getProjectName()+" started ============");
        }

        public void stop() {
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
        }


        public void Dispose() {
            Console.WriteLine("============ disposing service =============");
            stop();
            Console.WriteLine("============ end disposing service =============");
        }
    }
}