using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StackExchange.Redis;
using System.Configuration;
using RSMQ;

namespace RSMQTests
{
    [TestClass]
    public class RSMQTets
    {
        [TestInitialize]
        public void BeforeAll() {
            if (ConfigurationManager.ConnectionStrings["redis"] == null)
                throw new Exception("Add App.config file and add redis to connectionstrings section");
        }

        [TestMethod]
        public void SendMessageTest()
        {
            var rsmq = new RedisSMQ(ConnectionMultiplexer.Connect(ConfigurationManager.ConnectionStrings["redis"].ConnectionString));
            var messageId = rsmq.SendMessage(new MessageOptions
            {
                Message = @"{ ""kioskId"": ""GmZWMn3fifcjEqSyAAAC"", ""type"": ""reload"" }",
                QueueName = "kiosk-queue"
            });

            Assert.IsNotNull(messageId);
        }
    }
}
