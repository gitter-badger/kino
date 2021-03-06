﻿using System;
using kino.Framework;
using kino.Messaging;
using kino.Rendezvous.Consensus.Messages;

namespace kino.Rendezvous.Consensus
{
    public class LeaderElectionMessageFilter
    {
        private readonly Ballot ballot;
        private readonly Func<IMessage, ILeaseMessage> payload;
        private readonly ISynodConfiguration synodConfig;

        public LeaderElectionMessageFilter(Ballot ballot,
                                           Func<IMessage, ILeaseMessage> payload,
                                           ISynodConfiguration synodConfig)
        {
            this.ballot = ballot;
            this.synodConfig = synodConfig;
            this.payload = payload;
        }

        public bool Match(IMessage message)
        {
            var messagePayload = payload(message);

            return synodConfig.BelongsToSynod(new Uri(messagePayload.SenderUri))
                   && Unsafe.Equals(messagePayload.Ballot.Identity, ballot.Identity)
                   && messagePayload.Ballot.Timestamp == ballot.Timestamp.Ticks
                   && messagePayload.Ballot.MessageNumber == ballot.MessageNumber;
        }
    }
}