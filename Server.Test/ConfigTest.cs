using System;
using Xunit;
using Server.Model;
using Server.Misc;

namespace Server.Test
{
    public class ConfigTest {

        [Fact]
        public void testConfig() {
            Config config = new Config();
            string evnName = config.prefix()+"_DBCONNECTION";
            string evnValue = Environment.GetEnvironmentVariable(evnName); // preserve current param value
            
            Assert.Throws<Common.EConfigParamNotFound>( ()=>config.get("nonexistingParameter") );
            config.set("dbconnection", "testvalue");
            string param = config.get("dbconnection");
            Assert.Equal( "testvalue", param );
            Environment.SetEnvironmentVariable(evnName, "testvalueFromEnv");
            Assert.Equal( "testvalue", config.get("dbconnection") );
            config.reload();
            Assert.Equal( "testvalueFromEnv", config.get("dbconnection") );
            Environment.SetEnvironmentVariable( evnName, evnValue );
        }
    }
}