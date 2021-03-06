﻿using System;
using System.Collections.Concurrent;
using kino.Diagnostics;
using kino.Messaging;

namespace kino.Rendezvous.Consensus
{
    public class Listener : IObservable<IMessage>, IDisposable
    {
        private readonly ConcurrentDictionary<IObserver<IMessage>, object> observers;
        private readonly Action<Listener> unsubscribe;
        private readonly ILogger logger;

        public Listener(Action<Listener> unsubscribe, ILogger logger)
        {
            observers = new ConcurrentDictionary<IObserver<IMessage>, object>();
            this.unsubscribe = unsubscribe;
            this.logger = logger;
        }

        public void Notify(IMessage message)
        {
            foreach (var observer in observers.Keys)
            {
                try
                {
                    observer.OnNext(message);
                }
                catch (Exception err)
                {
                    logger.Error(err);
                }
            }
        }

        public IDisposable Subscribe(IObserver<IMessage> observer)
        {
            observers[observer] = null;

            return new Unsubscriber(observers, observer);
        }

        public void Dispose()
        {
            unsubscribe(this);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly ConcurrentDictionary<IObserver<IMessage>, object> observers;
            private readonly IObserver<IMessage> observer;

            public Unsubscriber(ConcurrentDictionary<IObserver<IMessage>, object> observers, IObserver<IMessage> observer)
            {
                this.observer = observer;
                this.observers = observers;
            }

            public void Dispose()
            {
                if (observer != null)
                {
                    object val;
                    observers.TryRemove(observer, out val);
                }
            }
        }
    }
}