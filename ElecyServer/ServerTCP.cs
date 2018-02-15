using System;
using System.Net.Sockets;
using System.Net;
using Bindings;

namespace ElecyServer
{
    //Server startup
    class ServerTCP
    {
        //Creating the main variables
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] _buffer = new byte[1024];

        //Creating clients array
        public static Client[] _clients = new Client[Constants.MAX_PLAYERS];

        //Server Setup
        public static void SetupServer()
        {
            Console.WriteLine("Сервер запущен на Ip адресе {0} и порте {1}.", GetLocalIPAddress(), 24985);
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                _clients[i] = new Client();
            }
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 24985));
            _serverSocket.Listen(10);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            
        }

        //Get the info about connetcion
        private static void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = _serverSocket.EndAccept(ar);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);

            //Creating a copy of Client class for every clients 
            for(int i = 1; i < Constants.MAX_PLAYERS; i++)
            {
                if(_clients[i].socket == null)
                {
                    _clients[i].socket = socket;
                    _clients[i].index = i;
                    _clients[i].ip = socket.RemoteEndPoint.ToString();
                    _clients[i].StartClient();
                    Console.WriteLine("Соединение с '{0}' установлено.", _clients[i].ip);
                    SendConnetionOK(i);
                    return;
                }
            }
        } 

        //Method to avoid packages drop
        public static void SendDataTo(int index, byte[] data)
        {
            byte[] sizeinfo = new byte[4];
            sizeinfo[0] = (byte)data.Length;
            sizeinfo[1] = (byte)(data.Length >> 8);
            sizeinfo[2] = (byte)(data.Length >> 16);
            sizeinfo[3] = (byte)(data.Length >> 24);

            _clients[index].socket.Send(sizeinfo);
            _clients[index].socket.Send(data);
        }

        public static void SendConnetionOK(int index)
        {
            PacketBuffer buffer = new PacketBuffer();
            buffer.WriteInteger((int)ServerPackets.SConnectionOK);
            buffer.WriteString("Connection to server with IP '" + GetLocalIPAddress() + "' succesfull.");
            SendDataTo(index, buffer.ToArray());
            buffer.Dispose();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
    //Client class for every client what connected
    public class Client
    {
        public int index;
        public string ip;
        public Socket socket = null;
        public bool closing = false;
        private byte[] _buffer = new byte[1024];

        //Starting client 
        public void StartClient()
        {
            socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            closing = false;
        }

        //Get the callback from client
        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                int received = socket.EndReceive(ar);

                if(received <= 0)
                {
                    CloseClient(index);
                }
                else
                {
                    byte[] dataBuffer = new byte[received];
                    Array.Copy(_buffer, dataBuffer, received);
                    ServerHandleNetworkData.HandleNetworkInformation(index, dataBuffer);
                    socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                }
            }
            catch
            {
                Console.WriteLine("catch");
                CloseClient(index);
            }
        }

        private void CloseClient(int index)
        {
            closing = true;
            Console.WriteLine("Соединение от {0} было разорвано.", ip);
            //PlayerLeft();
            socket.Close();
        }
    }
}
