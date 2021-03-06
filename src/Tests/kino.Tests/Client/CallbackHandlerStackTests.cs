﻿using System;
using System.Linq;
using kino.Client;
using kino.Diagnostics;
using kino.Framework;
using kino.Messaging;
using kino.Messaging.Messages;
using kino.Tests.Actors.Setup;
using Moq;
using NUnit.Framework;
using MessageIdentifier = kino.Connectivity.MessageIdentifier;

namespace kino.Tests.Client
{
    [TestFixture]
    public class CallbackHandlerStackTests
    {
        [Test]
        public void TestAddingHandlersForExistingCorrelation_ThrowsDuplicatedKeyException()
        {
            var logger = new Mock<ILogger>();
            var callbackHandlerStack = new CallbackHandlerStack(new ExpirableItemCollection<CorrelationId>(logger.Object));

            var correlationId = new CorrelationId(Guid.NewGuid().ToByteArray());
            var promise = new Promise();
            callbackHandlerStack.Push(correlationId, promise, Enumerable.Empty<MessageIdentifier>());

            Assert.Throws<DuplicatedKeyException>(() =>
                                                  {
                                                      callbackHandlerStack.Push(correlationId, promise, Enumerable.Empty<MessageIdentifier>());
                                                  });
        }

        [Test]
        public void TestPopCallBackHandlerForSpecificMessage_RemovesAllOtherHandlersForThisCorrelationId()
        {
            var logger = new Mock<ILogger>();
            var callbackHandlerStack = new CallbackHandlerStack(new ExpirableItemCollection<CorrelationId>(logger.Object));

            var correlationId = new CorrelationId(Guid.NewGuid().ToByteArray());
            var promise = new Promise();
            var messageHandlerIdentifiers = new[]
                                            {
                                                new MessageIdentifier(Message.CurrentVersion, SimpleMessage.MessageIdentity),
                                                new MessageIdentifier(Message.CurrentVersion, ExceptionMessage.MessageIdentity)
                                            };
            callbackHandlerStack.Push(correlationId, promise, messageHandlerIdentifiers);

            var handler = callbackHandlerStack.Pop(new CallbackHandlerKey
            {
                Identity = SimpleMessage.MessageIdentity,
                Version = Message.CurrentVersion,
                Correlation = correlationId.Value
            });

            Assert.IsNotNull(handler);

            handler = callbackHandlerStack.Pop(new CallbackHandlerKey
            {
                Identity = ExceptionMessage.MessageIdentity,
                Version = Message.CurrentVersion,
                Correlation = correlationId.Value
            });
            Assert.IsNull(handler);
        }
    }
}