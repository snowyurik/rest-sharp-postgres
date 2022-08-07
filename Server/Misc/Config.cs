using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

using Npgsql;


namespace Server.Misc {

    public class Config : Common.Config {

        private static Config _instance = new Config();

        /**
        create default parameter values
        they can be overriden with appsetings.json
        result can be again overriden with environment variables
        */
        public override Dictionary<string,string> createParams() {
            return new Dictionary<string,string> {
                ["dbconnection"] = "Host=localhost;Database=rest-sharp-postgres;Username=rest-sharp-postgres;Password=rest-sharp-postgresBRAEKME"
            };
        }

        public static string getStatic(string name) {
            return _instance.get(name);
        }
    }
}