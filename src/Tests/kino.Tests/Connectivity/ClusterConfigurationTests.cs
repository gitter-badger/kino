﻿using System;
using System.Threading;
using kino.Connectivity;
using kino.Diagnostics;
using Moq;
using NUnit.Framework;

namespace kino.Tests.Connectivity
{
    [TestFixture]
    public class ClusterConfigurationTests
    {
        private ILogger logger;
        private ClusterTimingConfiguration timingConfiguration;

        [SetUp]
        public void Setup()
        {
            timingConfiguration = new ClusterTimingConfiguration
            {
                ExpectedPingInterval = TimeSpan.FromSeconds(2),
                PongSilenceBeforeRouteDeletion = TimeSpan.FromSeconds(4)
            };
            logger = new Mock<ILogger>().Object;
        }

        [Test]
        public void TestAddClusterMember()
        {
            var config = new ClusterConfiguration(timingConfiguration, logger);
            var localhost = "tcp://127.0.0.1:40";
            var ep1 = new SocketEndpoint(new Uri(localhost), Guid.NewGuid().ToByteArray());
            var ep2 = new SocketEndpoint(new Uri(localhost), Guid.NewGuid().ToByteArray());
            config.AddClusterMember(ep1);
            config.AddClusterMember(ep2);

            CollectionAssert.Contains(config.GetClusterMembers(), ep1);
            CollectionAssert.Contains(config.GetClusterMembers(), ep2);
        }

        [Test]
        public void TestDeleteClusterMember()
        {
            var config = new ClusterConfiguration(timingConfiguration, logger);
            var localhost = "tcp://127.0.0.1:40";
            var ep1 = new SocketEndpoint(new Uri(localhost), Guid.NewGuid().ToByteArray());
            var ep2 = new SocketEndpoint(new Uri(localhost), Guid.NewGuid().ToByteArray());
            config.AddClusterMember(ep1);
            config.AddClusterMember(ep2);

            CollectionAssert.Contains(config.GetClusterMembers(), ep1);
            CollectionAssert.Contains(config.GetClusterMembers(), ep2);

            config.DeleteClusterMember(ep1);

            CollectionAssert.DoesNotContain(config.GetClusterMembers(), ep1);
            CollectionAssert.Contains(config.GetClusterMembers(), ep2);


        }

        [Test]
        public void TestNodeConsideredDead_IfLastKnownPongWasLongerThanPongSilenceBeforeRouteDeletionAgo()
        {
            var config = new ClusterConfiguration(timingConfiguration, logger);
            var localhost = "tcp://127.0.0.1:40";
            var ep1 = new SocketEndpoint(new Uri(localhost), Guid.NewGuid().ToByteArray());
            var ep2 = new SocketEndpoint(new Uri(localhost), Guid.NewGuid().ToByteArray());
            config.AddClusterMember(ep1);
            config.AddClusterMember(ep2);

            var pingTime = DateTime.UtcNow;
            var pingDelay = TimeSpan.FromSeconds(3);
            Thread.Sleep(timingConfiguration.PongSilenceBeforeRouteDeletion + pingDelay);

            config.KeepAlive(ep1);
            
            CollectionAssert.Contains(config.GetDeadMembers(pingTime), ep2);
            CollectionAssert.DoesNotContain(config.GetDeadMembers(pingTime), ep1);
        }
    }
}