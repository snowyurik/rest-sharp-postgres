using System;
using System.Collections.Generic;

namespace Common {

    public class EConfig : Exception {}
    public class EConfigParamNotFound : EConfig {}

    /**
    base class for project configuration
    */
    public class Config {

        private static Config instance = new Config();
        private Dictionary<string,string> items = new Dictionary<string,string>();

        public static string get(string name) {
            return instance._get(name);
        }

        public static void set(string name, string value) {
            instance._set(name, value);
        }

        private string _get(string name){
            try {
                return items[name];
            } catch( System.Collections.Generic.KeyNotFoundException ) {
                throw new EConfigParamNotFound();
            }
        }

        private void _set(string name, string value) {
            items[name] = value;
        }

        public static string prefix() {
            return instance._prefix();
        }

        public static void reload() {
            instance._reload();
        }

        private void _reload() {
            instance._set( "testparam", Environment.GetEnvironmentVariable( _prefix()+"_"+ new String("testparam").ToUpper() ) );
        }

        private string _prefix() {
            return "SMARTDEVAPP";
        }
    }
}
