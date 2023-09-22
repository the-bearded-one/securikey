using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

public class PortChecker
{
    public static async Task<List<int>> CheckPortsAsync(string host, int startPort, int endPort)
    {
        var openPorts = new List<int>();
        var tasks = new List<Task>();

        for (int port = startPort; port <= endPort; port++)
        {
            var task = CheckPortAsync(host, port, openPorts);
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        return openPorts;
    }

    private static async Task CheckPortAsync(string host, int port, List<int> openPorts)
    {
        using (TcpClient tcpClient = new TcpClient())
        {
            try
            {
                await tcpClient.ConnectAsync(host, port);
                Console.WriteLine($"Port {port} is open");
                openPorts.Add(port);
            }
            catch (Exception)
            {
                // Port is closed or couldn't connect
            }
        }
    }
}
