/*
 * Created date: 07-11-2020
 */
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatService.Server {
    public class Server : IServer {

        public int _port { get; set; }
        public Socket _socket { get; set; }
        public IPEndPoint _endPoint { get; set; }
        public int _bufferSize { get; set; }
        public byte[] _buffer { get; set; }

        public Server(int port) {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _port = port;
            _bufferSize = 2048;
            _buffer = new byte[_bufferSize];
        }


        public void StartServer() {
            _endPoint = new IPEndPoint(IPAddress.Any, _port);
            _socket.Bind(_endPoint);

            _socket.Listen(0);
            _socket.BeginAccept(Connect, null);
        }

        private void Connect(IAsyncResult result) {
            Socket socket;
            try {
                // accept the connection and set to a new socket.
                socket = _socket.EndAccept(result);
            }
            catch(Exception ex) {
                Console.WriteLine("Error: {0}", ex.Message);
                return;
            }

            socket.BeginReceive(_buffer, 0, _bufferSize, SocketFlags.None, Listen, socket);
            _socket.BeginAccept(Connect, null);
        }

        private void Listen(IAsyncResult result) {
            Socket current = (Socket)result.AsyncState;
            int received;
            try {
                // get the text.
                received = current.EndReceive(result);
            }
            catch(Exception ex) {
                current.Close();
                Console.WriteLine("Error: {0}", ex.Message);
                return;
            }

            ShowMessage(received);

            SendInformation(current);

            // Calling same method again, recursive for obvious reasons...
            current.BeginReceive(_buffer, 0, _bufferSize, SocketFlags.None, Listen, current);
        }

        private void SendInformation(Socket currentSocket) {
            byte[] response = Encoding.ASCII.GetBytes("Message successfuly delivered.");
            currentSocket.Send(response);
        }

        private void ShowMessage(int received) {
            byte[] recBuf = new byte[received];
            Array.Copy(_buffer, recBuf, received);
            string message = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Message: " + message);
        }
    }
}