using System;
using System.Collections.Generic;

namespace Common {

    public class EConfig : Exception {
        public EConfig():base() {}
        public EConfig(string msg):base(msg) {}
    }
    public class EConfigParamNotFound : EConfig {
        public EConfigParamNotFound():base(){}
        public EConfigParamNotFound(string msg):base(msg){}
    }

    /**
    base class for project configuration
    */
    public class Config {

        private Dictionary<string,string> _items;

        public Config() {
            reload();
        }

        /**
        Override this method to create list of parameters with default values
        defaults can be overriden with appsetings.json
        result can be again overriden with environment variables
        */
        public virtual Dictionary<string,string> createParams() {
            throw new EConfig("createParams should be overriden in child class");
            return new Dictionary<string,string>();
        }

        public string get(string name){
            checkParamExist(name);
            return _items[name];
        }

        public void set(string name, string value) {
            checkParamExist(name);
            _items[name] = value;
        }

        public void checkParamExist(string name) {
            if( !_items.ContainsKey(name)) {
                throw new EConfigParamNotFound("Invalid configuration parameter "+name+" , use/override Config.createParams() to specify list of parameters with default values");
            }
        }


        public void reload() {
            _items = createParams(); 
            loadConfigFile();
            loadEnvironmentVariables();
        }

        /**
        load configuration from appsettings.json
        */
        public void loadConfigFile() { // TODO implement loading from appsettings.json
            // not implemented
        }

        public void loadEnvironmentVariables() {
            foreach( string key in _items.Keys ) {
                string value = Environment.GetEnvironmentVariable( prefix()+"_"+ new String(key).ToUpper() );
                if( String.IsNullOrEmpty(value) ) {
                    continue;
                }
                _items[key] = value;
            }
        }

        public virtual string prefix() {
            return "SMARTDEVAPP"; // TODO magic
        }
    }
}
