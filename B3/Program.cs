using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class UDPServer
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        // Tạo Server EndPoint
        IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);

        // Tạo Server Socket
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Bind Server Socket với Server EndPoint
        serverSocket.Bind(serverEndPoint);

        Console.WriteLine("UDP Server đã khởi động...");

        byte[] buffer = new byte[1024];
        EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            // Nhận dữ liệu từ client
            int receivedBytes = serverSocket.ReceiveFrom(buffer, ref remote);
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, receivedBytes);

            // Phân tích dữ liệu nhận được từ client
            string[] parts = dataReceived.Split(',');
            if (parts.Length != 3)
            {
                Console.WriteLine("Dữ liệu không hợp lệ từ client.");
                continue;
            }

            try
            {
                double a = double.Parse(parts[0]);
                double b = double.Parse(parts[1]);
                double c = double.Parse(parts[2]);

                // Giải phương trình bậc hai
                double delta = b * b - 4 * a * c;
                double x1, x2;

                if (delta > 0)
                {
                    x1 = (-b + Math.Sqrt(delta)) / (2 * a);
                    x2 = (-b - Math.Sqrt(delta)) / (2 * a);
                    string resultMessage = $"Phương trình có 2 nghiệm: x1 = {x1}, x2 = {x2}";
                    byte[] responseData = Encoding.ASCII.GetBytes(resultMessage);
                    serverSocket.SendTo(responseData, remote);
                }
                else if (delta == 0)
                {
                    x1 = -b / (2 * a);
                    string resultMessage = $"Phương trình có nghiệm kép: x = {x1}";
                    byte[] responseData = Encoding.ASCII.GetBytes(resultMessage);
                    serverSocket.SendTo(responseData, remote);
                }
                else
                {
                    string resultMessage = "Phương trình vô nghiệm";
                    byte[] responseData = Encoding.ASCII.GetBytes(resultMessage);
                    serverSocket.SendTo(responseData, remote);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xử lý phép tính: {ex.Message}");
            }
        }
    }
}
