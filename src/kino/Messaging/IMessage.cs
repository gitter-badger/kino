﻿using System;

namespace kino.Messaging
{
    public interface IMessage
    {
        T GetPayload<T>() where T : IPayload, new();

        DistributionPattern Distribution { get; }
        byte[] Version { get; }
        byte[] Identity { get; }
        byte[] CorrelationId { get; }
        byte[] ReceiverIdentity { get; }
        byte[] CallbackIdentity { get; }
        byte[] CallbackReceiverIdentity { get; }
        TimeSpan TTL { get; set; }
        MessageTraceOptions TraceOptions { get; set; }
        byte[] Body { get; }
    }
}