﻿using System;

namespace kino.Rendezvous.Configuration
{
    public class RendezvousConfiguration
    {
        public string ServiceName { get; set; }
        public Uri MulticastUri { get; set; }
        public Uri UnicastUri { get; set; }
        public TimeSpan PingInterval { get; set; }
    }
}