/*
 * Created date: 07-11-2020
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatService.Client {
    public class Client : IClient {
        public int _port { get; set; }
        public Socket _socket { get; set; }
        public int _id { get; set; }
        public DateTime _lastMessageTime { get; set; }
        public int _tryCount { get; set; }
        public string message { get; set; }

        public Client(int port) {
            _port = port;
            _id = new Random().Next(0, 100);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect() {
            while(!_socket.Connected) {
                try {
                    _socket.Connect(IPAddress.Loopback, _port);
                }
                catch(Exception ex) {
                    Console.WriteLine($"an error occurred while attempting to connect to server port:{_port} ex:{ex.Message.ToString()}");
                    return;
                }
            }
        }
        public void Listen() {
            //we should listen our client, so this is an infinite loop
            while(true) {
                message = GetMessage();
                SendMessage(message);
            }
        }
        private string GetMessage() {
            Console.WriteLine("Type your message:...");
            return Console.ReadLine();
        }
        public void SendMessage(string message) {
            CheckMessageGap();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            _socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            _lastMessageTime = DateTime.Now;
        }
        private void CheckMessageGap() {
            DateTime now = DateTime.Now;
            if(now.Second == _lastMessageTime.Second) {
                Console.WriteLine("You can only send 1 message per second, next time you will be fired!");
                _tryCount++;
                if(_tryCount >1) {
                    Exit();
                }
            }
        }
        private void Exit() {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
            Environment.Exit(0);
        }      
    }
}
