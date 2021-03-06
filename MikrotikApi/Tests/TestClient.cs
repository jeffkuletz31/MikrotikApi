﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MikrotikApi.Tests
{
    [TestFixture]
    public class TestClient
    {
        Client testClient = null;

        [SetUp]
        public void SetUp()
        {
            testClient = new Client("192.168.0.250");
        }

        [TearDown]
        public void TearDown()
        {
            testClient.Close();
        }

        [Test]
        public void TestLogin()
        {
            testClient.Login("admin", "");
            Assert.Pass("Login OK");
        }

        [Test]
        public void TestDoCommand()
        {
            testClient.Login("admin", "");
            var responseData = testClient.DoCommand("/system/package/getall");
            Assert.Pass("DoCommand OK");
        }

        [Test]
        public void TestDoExport()
        {
            testClient.Login("admin", "");
            var responseData = testClient.DoCommand("/export");
            Assert.Pass("Config exported OK");
        }
    }
}
