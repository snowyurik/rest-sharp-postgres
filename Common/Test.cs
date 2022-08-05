using System;
using Newtonsoft.Json;
 

namespace Common {

    public class Test {
        protected void Log( string message, string? caller = null ) {
            if(caller == null) {
                caller = (new System.Diagnostics.StackTrace())!.GetFrame(1)!.GetMethod()!.Name;
            } 
            System.Console.WriteLine("[" + DateTime.Now + "]["+caller+"] "+message);
        }

        protected void Log( object obj ) {
            Log( JsonConvert.SerializeObject( obj, Formatting.Indented ), (new System.Diagnostics.StackTrace())!.GetFrame(1)!.GetMethod()!.Name );
        }

    }
}