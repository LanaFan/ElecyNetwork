using System;
using System.Net.Sockets;
using Bindings;
using System.Threading;
using System.Collections.Generic;

namespace ElecyServer
{
    public class GameRoom
    {

        #region Randomed

        private bool _rockRandomed = false;
        private bool _treeRandomed = false;

        #endregion

        private GameObjectList objects;
        private int roomIndex;
        private int mapIndex;
        private Player player1;
        private Player player2;
        private Timer timer;
        private ArenaRandomGenerator spawner;
        private Dictionary<NetworkGameObject.Type, int[]> ranges;
        private bool p1Loaded = false;
        private bool p2Loaded = false;
        private float scaleX;
        private float scaleZ;
        private float[] firstSpawnPointPos;
        private float[] secondSpawnPointPos;
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
            ranges = new Dictionary<NetworkGameObject.Type, int[]>();
            mapIndex = new Random().Next(3, 2 + Constants.MAPS_COUNT); 
        }

        public void AddPlayer(NetPlayer player)
        {
            if (status == RoomStatus.Empty)
            {
                player1 = new Player(player.playerSocket, player.index, player.nickname, 1, 0f);
                status = RoomStatus.Searching;
                Global.serverForm.AddGameRoom(roomIndex + "");
            }
            else if (status == RoomStatus.Searching)
            {
                player2 = new Player(player.playerSocket, player.index, player.nickname, 2, 0f);
                status = RoomStatus.Closed;
                ServerSendData.SendMatchFound(mapIndex, player1.GetIndex(), player2.GetIndex(), roomIndex);
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
                Global.serverForm.RemoveGameRoom(roomIndex + "");
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

        public void GameRoomInstatiate(int objectID, int instanceType, string objectPath, float[] pos, float[] rot)
        {
            ///Here comes the object adding to list and sent to players instantiate
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

        #region Load

        public void SetGameLoadData(int ID)
        {
            int[] scale = Global.data.GetMapScale(mapIndex);
            float[][] spawnPos = Global.data.GetSpawnPos(mapIndex);
            if (ID == 1)
            {
                p1Loaded = true;
                SetTransform(ID, spawnPos[0], new float[] { 0, 0, 0, 1 });
                this.scaleX = scale[0] * 10f;
                this.scaleZ = scale[1] * 10f;
                firstSpawnPointPos = spawnPos[0];
                secondSpawnPointPos = spawnPos[1];
            }
            else
            {
                p2Loaded = true;
                SetTransform(ID, spawnPos[1], new float[] { 0, 0, 0, 1 });
                this.scaleX = scale[0] * 10f;
                this.scaleZ = scale[1] * 10f;
                firstSpawnPointPos = spawnPos[0];
                secondSpawnPointPos = spawnPos[1];
            }

            if (p1Loaded && p2Loaded)
            {
                float[][] spawnRot = Global.data.GetSpawnRot(mapIndex);
                p1Loaded = false;
                p2Loaded = false;
                spawner = new ArenaRandomGenerator(this.scaleX, this.scaleZ, firstSpawnPointPos, secondSpawnPointPos);
                ServerSendData.SendGameData(roomIndex, player1.GetNickname(), player2.GetNickname(), spawnPos, spawnRot);
            }

        }

        public void SpawnTree(int ID)
        {
            if(!_treeRandomed)
            {
                _treeRandomed = true;
                int[] range = objects.Add(NetworkGameObject.Type.tree, roomIndex);
                ranges.Add(NetworkGameObject.Type.tree, range);
            }

            ServerSendData.SendTreeSpawned(ID, roomIndex, ranges[NetworkGameObject.Type.tree]);
        }

        public void SpawnRock(int ID)
        {
            if(!_rockRandomed)
            {
                _rockRandomed = true;
                int[] range = objects.Add(NetworkGameObject.Type.rock, roomIndex);
                ranges.Add(NetworkGameObject.Type.rock, range);
            }

            ServerSendData.SendRockSpawned(ID, roomIndex, ranges[NetworkGameObject.Type.rock]);
        }

        public void LoadComplete(int ID)
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

        #endregion

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

        #region Gets And Sets

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

        public Player GetPlayer(int number)
        {
            return number == 1 ? player1 : player2;
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

        public int GetRoomIndex()
        {
            return roomIndex;
        }

        public string GetSize()
        {
            return scaleX + "x" + scaleZ;
        }

        public ArenaRandomGenerator GetRandom()
        {
            return spawner;
        }

        public GameObjectList GetObjectsList()
        {
            return objects;
        }

        public void SetLoadProgress(int ID, float loadProgress)
        {
            if(ID == 1)
            {
                player1.SetLoad(loadProgress);
                ServerSendData.SendEnemyProgress(1, roomIndex, player2.GetLoad());
            } else
            {
                player2.SetLoad(loadProgress);
                ServerSendData.SendEnemyProgress(2, roomIndex, player1.GetLoad());
            }
        }

        #endregion

    }

    public class Player
    {
        private int _index;
        private float _load;
        private int _ID;
        private string _nickname;
        private float[] _position;
        private float[] _rotation;
        private Socket _socket;
        private byte[] _buffer = new byte[Constants.BUFFER_SIZE];
        private bool _playing = false;

        public Player(Socket socket, int index, string nickname, int ID, float load)
        {
            _socket = socket;
            _index = index;
            _nickname = nickname;
            _ID = ID;
            _load = load;
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

        public float GetLoad()
        {
            return _load;
        }

        public void SetLoad(float load)
        {
            _load = load;
        }

        #endregion
    }
}
