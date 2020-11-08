using System;

namespace ChatService.Client {
    class Program {
        static void Main(string[] args) {
            var client = new Client(2020);
            Console.Title = $"The Client #{ client._id }";

            Console.WriteLine($"{ client._id } trying to connect...");
            client.Connect();
            Console.WriteLine($"{ client._id } connected.");

            Console.WriteLine($"{ client._id } started to listening...");
            client.Listen();
        }
    }
}
