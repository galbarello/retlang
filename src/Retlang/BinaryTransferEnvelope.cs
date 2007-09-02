using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Retlang
{
    public class BinaryTransferEnvelope: ITransferEnvelope
    {
        private readonly Type _messageType;
        private readonly byte[] _msg;
        private readonly IMessageHeader _header;

        public BinaryTransferEnvelope(object topic, object msg, object replyTo)
        {
            _messageType = msg.GetType();
            _header = new MessageHeader(topic, replyTo);
            _msg = ConvertToBytes(msg);
        }

        protected byte[] ConvertToBytes(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            stream.Flush();
            return stream.ToArray();
        }

        public Type MessageType
        {
            get { return _messageType; }
        }

        public object ResolveMessage()
        {
            MemoryStream stream = new MemoryStream(_msg);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
        }

        public IMessageHeader Header
        {
            get { return _header; }
        }
    }
}