using System;
using System.Net.Sockets;
using Bindings;
using System.Threading;

namespace ElecyServer
{
    public class GameRoom
    {
        private GameObjectList objects;
        private int roomIndex;
        private Player player1;
        private Player player2;
        private Timer timer;
        private ArenaRandomGenerator spawner;
        private bool p1Loaded = false;
        private bool p2Loaded = false;
        private float scaleX;
        private float scaleZ;
        private RoomStatus status;

        public enum RoomStatus
        {
            Empty = 1,
            Searching = 2,
            Closed = 3
        }

        public GameRoom(int index)
        {
            roomIndex = index;
            status = RoomStatus.Empty;
            objects = new GameObjectList();
        }

        public void AddPlayer(NetPlayer player)
        {
            if (status == RoomStatus.Empty)
            {
                player1 = new Player(player.playerSocket, player.index, player.nickname, 1);
                status = RoomStatus.Searching;
            }
            else if (status == RoomStatus.Searching)
            {
                player2 = new Player(player.playerSocket, player.index, player.nickname, 2);
                status = RoomStatus.Closed;
                ServerSendData.SendMatchFound(player1.GetIndex(), player2.GetIndex(), roomIndex);
            }
            else
                return; // Ubrat'
                //Send alert
        }

        public void DeletePlayer(int index)
        {
            if (player1.GetIndex() == index)
            {
                player1 = null;
                Global.players[index].state = NetPlayer.playerState.InMainLobby;
                status = RoomStatus.Empty;
            }
            else if (player2.GetIndex() == index) 
            {
                player2 = null;
                Global.players[index].state = NetPlayer.playerState.InMainLobby;
                status = RoomStatus.Searching;
            }
        }

        public void StartGame()
        {
            timer = new Timer(SendTransform, null, 0, 1000/Constants.UPDATE_RATE);
        }

        public void StartReceive(int index)
        {
            if(index == player1.GetIndex())
            {
                player1.StartPlay();
                StopNetPlayer(index);
            }
            else
            {
                player2.StartPlay();
                StopNetPlayer(index);
            }
        }

        public void StopTimer()
        {
            try
            {
                timer.Dispose();
            }
            catch { }
        }

        public void SpawnTree(int ID)
        {
            if (ID == 1)
                p1Loaded = true;
            else
                p2Loaded = true;

            if(p1Loaded == true && p2Loaded == true)
            {
                p1Loaded = false;
                p2Loaded = false;
                objects.Add(NetworkGameObject.Type.tree, roomIndex);
            }
        }

        private void StopNetPlayer(int index)
        {
            Global.players[index].NetPlayerStop();
        }

        private void SendTransform(Object o)
        {
            float[][] p2transform = player2.GetTransform();
            ServerSendData.SendTransform(1, roomIndex, p2transform[0], p2transform[1]);
            float[][] p1transform = player1.GetTransform();
            ServerSendData.SendTransform(2, roomIndex, p1transform[0], p1transform[1]);
        }

        #region Get And Sets

        public void SetGameLoadData(int ID, float scaleX, float scaleZ)
        {
            if (ID == 1)
            {
                p1Loaded = true;
                if(this.scaleX != scaleX && this.scaleZ != scaleZ)
                {
                    this.scaleX = scaleX;
                    this.scaleZ = scaleZ;
                }
            }
            else
            {
                p2Loaded = true;
                if (this.scaleX != scaleX && this.scaleZ != scaleZ)
                {
                    this.scaleX = scaleX;
                    this.scaleZ = scaleZ;
                }
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
                SetTransform(ID, position, rotation);
            }
            else
            {
                p2Loaded = true;
                SetTransform(ID, position, rotation);
            }

            if (p1Loaded && p2Loaded)
            {
                p1Loaded = false;
                p2Loaded = false;
                spawner = new ArenaRandomGenerator(scaleX, scaleZ, player1.GetPosition(), player2.GetPosition());
                objects.Add(NetworkGameObject.Type.rock, roomIndex);
            }

        }

        public void SetLoadComplete(int ID)
        {
            if (ID == 1)
                p1Loaded = true;
            else
                p2Loaded = true;

            if(p1Loaded && p2Loaded)
            {
                p1Loaded = false;
                p2Loaded = false;
                ServerSendData.SendRoomStart(roomIndex);
                StartGame();
            }
        }

        public void SetTransform(int ID, float[] position, float[] rotation)
        {
            if (ID == 1)
            {
                player1.SetTransform(position, rotation);
            }
            else
            {
                player2.SetTransform(position, rotation);
            }
        }

        public RoomStatus GetStatus()
        {
            return status;
        }

        public Socket GetP1Socket()
        {
            return player1.GetSocket();
        }

        public Socket GetP2Socket()
        {
            return player2.GetSocket();
        }

        public Socket GetSocket(int ID)
        {
            return (ID == 1) ? GetP1Socket() : GetP2Socket();
        }

        public string[] GetNicknames()
        {
            string[] nicknames = new string[2];
            nicknames[0] = player1.GetNickname();
            nicknames[1] = player2.GetNickname();
            return nicknames;
        }

        public ArenaRandomGenerator GetRandom()
        {
            return spawner;
        }

        public GameObjectList GetObjectsList()
        {
            return objects;
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
                Global.serverForm.Debug("GamePlayer Disconnected");
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

        public float[] GetPosition()
        {
            return _position;
        }

        public void SetTransform(float[] position, float[] rotation)
        {
            _position = position;
            _rotation = rotation;
        }

        #endregion
    }
}
