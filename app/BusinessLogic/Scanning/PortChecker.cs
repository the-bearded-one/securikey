using System.Collections.Concurrent;
using System.Net.Sockets;

namespace BusinessLogic.Scanning
{
    public class PortChecker : IChecker
    {
        public event Action<List<int>>? OnCompletion;
        public List<ScanResult> ScanResults { get; private set; } = new List<ScanResult>();
        public bool RequiresElevatedPrivilege { get; } = false;

        public void Scan()
        {
            ScanResults.Clear();
            // TODO
        }

        public async Task CheckPortsAsync(string host, int startPort, int endPort, CancellationToken cancellationToken)
        {
            var openPorts = new ConcurrentBag<int>();
            var tasks = new List<Task>();

            for (int port = startPort; port <= endPort; port++)
            {
                var task = CheckPortAsync(host, port, openPorts, cancellationToken);
                tasks.Add(task);
            }

            try
            {
                await Task.WhenAll(tasks);
                if (tasks.All(t => t.IsCompleted))
                {
                    List<int> openPortsList = openPorts.ToList();
                    OnCompletion?.Invoke(openPortsList);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation was cancelled.");
            }

            //        return new List<int>(openPorts);
        }

        private async Task CheckPortAsync(string host, int port, ConcurrentBag<int> openPorts, CancellationToken cancellationToken)
        {
            using (TcpClient tcpClient = new TcpClient())
            {
                try
                {
                    await tcpClient.ConnectAsync(host, port).ConfigureAwait(false);
                    cancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine($"Port {port} is open");
                    openPorts.Add(port);
                }
                catch (SocketException)
                {
                    // Socket couldn't be read

                }
                catch (OperationCanceledException)
                {
                    // Task was cancelled
                }
                catch (Exception)
                {
                    // Port is closed or couldn't connect
                }
            }
        }
    }
}