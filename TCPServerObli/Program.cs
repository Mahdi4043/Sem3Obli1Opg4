using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks.Dataflow;

Console.WriteLine("TCP Server:");

TcpListener listener = new TcpListener(IPAddress.Any, 7);
listener.Start();
while (true)
{
    TcpClient socket = listener.AcceptTcpClient();
    Task.Run(() => HandleClient(socket));
}

void HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);
    try
    {
        while (socket.Connected)
        {
            string message = reader.ReadLine().ToLower();
            Console.WriteLine($"C: {message}");
            //writer.WriteLine($"C: {message}");

            if (message == "random")
            {
                writer.WriteLine("input 2 numbers");
                writer.Flush();

                string tal = reader.ReadLine();
                string[] split = tal.Split(" ");

                if (split.Length == 2 && int.TryParse(split[0], out int a) && int.TryParse(split[1], out int b))
                {
                    Random rand = new Random();
                    writer.WriteLine(rand.Next(a, b));
                }
                else
                {
                    writer.WriteLine("Invalid input. Please input two numbers.");
                }
            }
            else if (message == "add")
            {
                writer.WriteLine("input 2 numbers");
                writer.Flush();

                string tal = reader.ReadLine();
                string[] split = tal.Split(" ");

                if (split.Length == 2 && int.TryParse(split[0], out int a) && int.TryParse(split[1], out int b))
                {
                    writer.WriteLine(a + b);
                }
                else
                {
                    writer.WriteLine("Invalid input. Please input two numbers.");
                }
            }
            else if (message == "subtract")
            {
                writer.WriteLine("input 2 numbers");
                writer.Flush();

                string tal = reader.ReadLine();
                string[] split = tal.Split(" ");

                if (split.Length == 2 && int.TryParse(split[0], out int a) && int.TryParse(split[1], out int b))
                {
                    writer.WriteLine(a - b);
                }
                else
                {
                    writer.WriteLine("Invalid input. Please input two numbers.");
                }
            }
            else if (message == "close")
            {
                socket.Close();
            }
            else
            {
                writer.WriteLine("Unknown command");
                writer.Flush();
            }
            writer.Flush();

        }
    }
    catch (Exception e)
    {
        Console.WriteLine($"Error: {e.Message}");
    }
    finally
    {
        socket.Close();
    }
}