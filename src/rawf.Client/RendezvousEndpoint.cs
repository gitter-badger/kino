﻿namespace rawf.Client
{
    public class RendezvousEndpoint
    {
        public string BroadcastUri { get; set; }
        public string UnicastUri { get; set; }
        public byte[] UnicastSocketIdentity { get; set; }
    }
}