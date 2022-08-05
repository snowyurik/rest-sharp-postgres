using System;
using Xunit;
using Server.Model;
using Server.Misc;

namespace Server.Test
{
    public class ConfigTest {

        [Fact]
        public void testConfig() {
            Assert.Throws<Common.EConfigParamNotFound>( ()=>Config.get("testparam") );
            Config.set("testparam", "testvalue");
            string param = Config.get("testparam");
            Assert.Equal( "testvalue", param );
            Environment.SetEnvironmentVariable(Config.prefix()+"_TESTPARAM", "testvalueFromEnv");
            Assert.Equal( "testvalue", Config.get("testparam") );
            Config.reload();
            Assert.Equal( "testvalueFromEnv", Config.get("testparam") );
        }
    }
}