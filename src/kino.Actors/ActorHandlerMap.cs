﻿using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using kino.Connectivity;
using kino.Framework;

namespace kino.Actors
{
    public class ActorHandlerMap : IActorHandlerMap
    {
        private readonly ConcurrentDictionary<MessageIdentifier, MessageHandler> messageHandlers;

        public ActorHandlerMap()
        {
            messageHandlers = new ConcurrentDictionary<MessageIdentifier, MessageHandler>();
        }

        public IEnumerable<MessageIdentifier> Add(IActor actor)
        {
            var tmp = new List<MessageIdentifier>();
            foreach (var reg in GetActorRegistrations(actor))
            {
                if (messageHandlers.TryAdd(reg.Key, reg.Value))
                {
                    tmp.Add(reg.Key);
                }
                else
                {
                    throw new DuplicatedKeyException(reg.Key.ToString());
                }
            }

            return tmp;
        }

        public MessageHandler Get(MessageIdentifier identifier)
        {
            MessageHandler value;
            if (messageHandlers.TryGetValue(identifier, out value))
            {
                return value;
            }

            throw new KeyNotFoundException(identifier.ToString());
        }

        public IEnumerable<MessageIdentifier> GetMessageHandlerIdentifiers()
        {
            return messageHandlers.Keys;
        }

        private static IEnumerable<KeyValuePair<MessageIdentifier, MessageHandler>> GetActorRegistrations(IActor actor)
            => actor
                .GetInterfaceDefinition()
                .Select(messageMap =>
                        new KeyValuePair<MessageIdentifier, MessageHandler>(
                            new MessageIdentifier(messageMap.Message.Version,
                                                         messageMap.Message.Identity),
                            messageMap.Handler));
    }
}