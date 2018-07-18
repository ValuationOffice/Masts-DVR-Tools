using System;
using masts_dvr_tool.Services.Contract;
using masts_dvr_tool.Services.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace masts_dvr_tool.Tests.Services
{
    [TestClass]
    public class PasswordGeneratorTests
    {

        [TestInitialize]
        public void TestSetup()
        {
           
        }

        [TestMethod]
        public void GeneratePassword_ShouldReturn14CharacterString_WhenRun()
        {
            string returnValue = PasswordGenerator.GeneratePassword();

            Assert.AreEqual(14, returnValue.Length);
        }
    }
}
