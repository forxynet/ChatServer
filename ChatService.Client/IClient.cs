using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ChatService.Client {
    public interface IClient {
        int _port { get; set; }
        Socket _socket { get; set; }
        int _id { get; set; }
        DateTime _lastMessageTime { get; set; }
        int _tryCount { get; set; }
        string message { get; set; }
        public void Connect();
        public void Listen();
        public void SendMessage(string message);
       
    }
}
