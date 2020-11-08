using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatService.Server {
    public interface IServer {
        int _port { get; set; }
        Socket _socket { get; set; }
        IPEndPoint _endPoint { get; set; }
        int _bufferSize { get; set; }
        byte[] _buffer { get; set; }
        public void StartServer();
    }
}
