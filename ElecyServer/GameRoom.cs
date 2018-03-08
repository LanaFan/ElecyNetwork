using System;
using System.Net.Sockets;
using Bindings;

namespace ElecyServer
{
    public class GameRoom
    {
        public bool active = false;
        private int roomIndex;
        private Player player1;
        private Player player2;
        private RoomStatus status = RoomStatus.Empty;

        public enum RoomStatus
        {
            Empty = 1,
            Searching = 2,
            Closed = 3
        }

        private bool p1Loaded = false;
        private bool p2Loaded = false;

        public GameRoom(int index)
        {
            roomIndex = index;
        }


        //public void lalala()
        //{
        //    p1Set = false;
        //    p2Set = false;
        //    roomIndex = j;
        //    this.player1 = new Player(player1.playerSocket, player1.index, player1.nickname, 1);
        //    this.player2 = new Player(player2.playerSocket, player2.index, player2.nickname, 2);
        //    StartGame();
        //}

        public void AddPlayer(NetPlayer player)
        {
            if (status == RoomStatus.Empty)
            {
                player.state = NetPlayer.playerState.SearchingForMatch;
                player.NetPlayerStop();
                player1 = new Player(player.playerSocket, player.index, player.nickname, 1);
                status = RoomStatus.Searching;
            }
            else if (status == RoomStatus.Searching)
            {
                player.state = NetPlayer.playerState.SearchingForMatch;
                player.NetPlayerStop();
                player2 = new Player(player.playerSocket, player.index, player.nickname, 2);
                status = RoomStatus.Closed;
                StartGame();
            }
            else
                return; // Ubrat'
                //Send alert
        }


        private void StartGame()
        {
            ServerHandleRoomData.InitializeNetworkPackages();
            player1.StartPlay();
            player2.StartPlay();
            ServerSendData.SendMatchFound(player1.GetIndex(), player2.GetIndex(), roomIndex);
        }

        public void SendTransform()
        {
            float[][] p2transform = player2.GetTransform();
            ServerSendData.SendTransform(1, roomIndex, p2transform[0], p2transform[1]);
            float[][] p1transform = player1.GetTransform();
            ServerSendData.SendTransform(2, roomIndex, p1transform[0], p1transform[1]);
        }

        #region Get And Sets
        public Socket GetP1Socket()
        {
            return player1.GetSocket();
        }

        public RoomStatus GetStatus()
        {
            return status;
        }

        public Socket GetP2Socket()
        {
            return player2.GetSocket();
        }

        public Socket GetSocket(int ID)
        {
            return (ID == 1) ? GetP1Socket() : GetP2Socket();
        }

        public void SetGameLoadData(int ID)
        {
            if (ID == 1)
            {
                p1Loaded = true;
            }
            else
            {
                p2Loaded = true;
            }

            if (p1Loaded && p2Loaded)
            {
                p1Loaded = false;
                p2Loaded = false;
                ServerSendData.SendGameData(roomIndex);
            }

        }

        public void SetGameData(int ID, float[] position, float[] rotation)
        {
            if (ID == 1)
            {
                p1Loaded = true;
            }
            else
            {
                p2Loaded = true;
            }

            if (p1Loaded && p2Loaded)
            {
                p1Loaded = false;
                p2Loaded = false;
                ServerSendData.SendRoomStart(roomIndex);
            }

        }

        public void SetTransform(int ID, float[] position, float[] rotation)
        {
            if (ID == 1)
            {
                player1.SetTransform(position, rotation);
                SendTransform();
            }
            else
            {
                player2.SetTransform(position, rotation);
                SendTransform();
            }
        }

        public string[] GetNicknames()
        {
            string[] nicknames = new string[2];
            nicknames[0] = player1.GetNickname();
            nicknames[1] = player2.GetNickname();
            return nicknames;
        }

        public float[][][] GetTransforms()
        {
            float[][][] transforms = new float[2][][];
            transforms[0] = player1.GetTransform();
            transforms[1] = player2.GetTransform();
            return transforms;
        }
        #endregion

    }

    public class Player
    {
        private int _index;
        private int _ID;
        private string _nickname;
        private float[] _position;
        private float[] _rotation;
        private Socket _socket;
        private byte[] _buffer = new byte[Constants.BUFFER_SIZE];
        private bool _playing = false;

        public Player(Socket socket, int index, string nickname, int ID)
        {
            _socket = socket;
            _index = index;
            _nickname = nickname;
            _ID = ID;
        }

        public void StartPlay()
        {
            _playing = true;
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallBack), _socket);
        }

        private void PlayerReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int received = _socket.EndReceive(ar);
                if (received <= 0)
                {
                    //Disconnect
                }
                else
                {
                    byte[] dataBuffer = new byte[received];
                    Array.Copy(_buffer, dataBuffer, received);
                    if (_playing)
                        _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallBack), _socket);
                    else
                        return;
                    ServerHandleRoomData.HandleNetworkInformation(_ID, dataBuffer);

                }
            }
            catch
            {
                Console.WriteLine("GamePlayer Disconnected");
            }
        }

        public void PlayerStop()
        {
            _playing = false;
            //Send disconnect
        }

        public void PlayerClose()
        {
            PlayerStop();
        }

        #region Gets and Sets

        public Socket GetSocket()
        {
            return _socket;
        }

        public string GetNickname()
        {
            return _nickname;
        }

        public int GetIndex()
        {
            return _index;
        }

        public float[][] GetTransform()
        {
            float[][] transform = new float[2][];
            transform[0] = _position;
            transform[1] = _rotation;
            return transform;
        }

        public void SetTransform(float[] position, float[] rotation)
        {
            _position = position;
            _rotation = rotation;
        }

        #endregion
    }
}
