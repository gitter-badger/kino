using ProtoBuf;
using rawf.Framework;

namespace rawf.Messaging.Messages
{
	[ProtoContract]
	public class UnregisterMessageHandlersRoutingMessage : Payload
	{
		public static readonly byte[] MessageIdentity = "UNREGROUTE".GetBytes();
		
		[ProtoMember(1)]
		public string Uri { get; set; }
		
		[ProtoMember(2)]
		public byte[] SocketIdentity { get; set; }
	}
}