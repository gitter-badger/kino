using System.Collections.Generic;
using kino.Messaging;

namespace kino.Connectivity
{
    public class AsyncMessageContext
    {
        public IEnumerable<IMessage> OutMessages { get; internal set; }
        public byte[] CorrelationId { get; internal set; }
        public byte[] CallbackIdentity { get; internal set; }
        public byte[] CallbackReceiverIdentity { get; internal set; }
        public IEnumerable<SocketEndpoint> MessageHops { get; internal set; }
    }
}