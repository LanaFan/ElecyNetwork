using System;
using System.Net.Sockets;
using Bindings;
using System.Threading;

namespace ElecyServer
{

    public class GameRoom
    {
        public GameObjectList ObjectsList { get; private set; }
        public int RoomIndex { get; private set; }
        public ArenaRandomGenerator Spawner { get; private set; }
        public RoomStatus Status { get; private set; }

        private int mapIndex;
        private Player player1;
        private Player player2;
        private Timer timer;
        private Timer closeTimer;
        private Timer loadTimer;
        private bool p1Loaded;
        private bool p2Loaded;
        private float scaleX;
        private float scaleZ;
        private float[] firstSpawnPointPos;
        private float[] secondSpawnPointPos;

        #region Randomed

        private enum Randoming
        {
            unrandomed = 0,
            randoming = 1,
            randomed = 2
        }

        private Randoming _rockRandomed;
        private Randoming _treeRandomed;

        object expectant = new object(); // list for multiple players

        #endregion

        public enum RoomStatus
        {
            Empty = 1,
            Searching = 2,
            MatchEnded = 3,
            Closed = 4
        }

        public GameRoom(int roomIndex)
        {
            RoomIndex = roomIndex;
            Status = RoomStatus.Empty;
            ObjectsList = new GameObjectList();
            mapIndex = new Random().Next(3, 2 + Constants.MAPS_COUNT);
            _rockRandomed = Randoming.unrandomed;
            _treeRandomed = Randoming.unrandomed;
            p1Loaded = false;
            p2Loaded = false;
        }

        public void AddPlayer(NetPlayer player, string race)
        {
            if (Status == RoomStatus.Empty)
            {
                player1 = new Player(player.Socket, this, player.Index, player.Nickname, 1, 0f, race);
                player.Searching(RoomIndex);
                Status = RoomStatus.Searching;
                Global.serverForm.AddGameRoom(RoomIndex + "");
            }
            else if (Status == RoomStatus.Searching)
            {
                player2 = new Player(player.Socket, this, player.Index, player.Nickname, 2, 0f, race);
                player.Searching(RoomIndex);
                Status = RoomStatus.Closed;
                ServerSendData.SendMatchFound(mapIndex, player1.Index, player2.Index, RoomIndex);
            }
            else
                return; // Ubrat'
                //Send alert
        }

        public void DeletePlayer(int index)
        {
            if (player1 != null && player1.Index == index)
            {
                player1 = null;
                Global.players[index].InMainLobby();
                Status = RoomStatus.Empty;
                Global.serverForm.RemoveGameRoom(RoomIndex + "");
            }
            else if (player2 != null && player2.Index == index) 
            {
                player2 = null;
                Global.players[index].InMainLobby();
                Status = RoomStatus.Searching;
            }
        }

        public void StartGame()
        {
            //timer = new Timer(SendTransform, null, 0, 1000/Constants.UPDATE_RATE);
        }

        public void GameRoomInstatiate(int objectID, int instanceType, string objectPath, float[] pos, float[] rot)
        {
            ///Here comes the object adding to list and sent to players instantiate
        }

        public void StartReceive(int index)
        {
            if(index == player1.Index)
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

        public void AbortGameSession(int ID)
        {
            if (Status != RoomStatus.MatchEnded)
            {
                Status = RoomStatus.MatchEnded;
                StopTimer();
                closeTimer = new Timer(EndGameSession, null, 300000, Timeout.Infinite);
                if (ID == 1)
                {
                    player1 = null;
                    ServerSendData.SendMatchEnded(2, RoomIndex, player2.Nickname);
                }
                else
                {
                    player2 = null;
                    ServerSendData.SendMatchEnded(1, RoomIndex, player1.Nickname);
                }
            }
            else
            {
                if (ID == 1)
                {
                    player1 = null;
                }
                else
                {
                    player2 = null;
                }
                if(player1 == null && player2 == null)
                {
                    ClearRoom();
                }
            }
        }

        public void Surrended(int ID)
        {
            Status = RoomStatus.MatchEnded;
            StopTimer();
            if(ID == 1)
            {
                ServerSendData.SendMatchEnded(RoomIndex, player2.Nickname);
            }
            else
            {
                ServerSendData.SendMatchEnded(RoomIndex, player1.Nickname);
            }
        }

        public void BackToNetPlayer(int ID)
        {
            if(ID == 1)
            {
                Global.players[player1.Index].StartPlayer();
                player1 = null;
            }
            else
            {
                Global.players[player2.Index].StartPlayer();
                player2 = null;
            }
            if (player1 == null && player2 == null)
            {
                ClearRoom();
            }
        }

        public void CloseRoom()
        {
            StopTimers();
            if (player1 != null)
                player1.PlayerClose();
            if (player2 != null)
                player2.PlayerClose();
            Global.arena[RoomIndex] = null;
        }

        private void StopNetPlayer(int index)
        {
            Global.players[index].NetPlayerStop();
        }

        //private void SendTransform(Object o)
        //{
        //    float[][] p2transform = player2.Transform;
        //    ServerSendData.SendTransform(1, RoomIndex, p2transform[0], p2transform[1]);
        //    float[][] p1transform = player1.Transform;
        //    ServerSendData.SendTransform(2, RoomIndex, p1transform[0], p1transform[1]);
        //}

        private void StopTimers()
        {
            if (timer != null)
                timer.Dispose();
            if (closeTimer != null)
                closeTimer.Dispose();
        }

        private void EndGameSession(Object o)
        {
            closeTimer.Dispose();
            if (player1 != null)
            {
                ServerSendData.SendRoomLogOut(1, RoomIndex);
                BackToNetPlayer(1);
            }
            if (player2 != null)
            {
                ServerSendData.SendRoomLogOut(2, RoomIndex);
                BackToNetPlayer(2);
            }
            ClearRoom();
        }

        private void ClearRoom()
        {
            Global.serverForm.RemoveGameRoom(RoomIndex + "");
            player1 = player2 = null;
            Status = RoomStatus.Empty;
            ObjectsList = new GameObjectList();
            mapIndex = new Random().Next(3, 2 + Constants.MAPS_COUNT);
            _rockRandomed = Randoming.unrandomed;
            _treeRandomed = Randoming.unrandomed;
            p1Loaded = false;
            p2Loaded = false;
        }

        #region Load

        public void SetGameLoadData(int ID)
        {
            int[] scale = Global.data.GetMapScale(mapIndex);
            float[][] spawnPos = Global.data.GetSpawnPos(mapIndex);
            float[][] spawnRot = Global.data.GetSpawnRot(mapIndex);

            if (ID == 1)
            {
                SetTransform(ID, spawnPos[0], spawnRot[0]);
                scaleX = scale[0] * 10f;
                scaleZ = scale[1] * 10f;
                firstSpawnPointPos = spawnPos[0];
                secondSpawnPointPos = spawnPos[1];
                Global.players[player1.Index].Playing();
                p1Loaded = true;
            }
            else
            {
                SetTransform(ID, spawnPos[1], spawnRot[1]);
                scaleX = scale[0] * 10f;
                scaleZ = scale[1] * 10f;
                firstSpawnPointPos = spawnPos[0];
                secondSpawnPointPos = spawnPos[1];
                Global.players[player2.Index].Playing();
                p2Loaded = true;
            }

            if (p1Loaded && p2Loaded)
            {
                p1Loaded = false;
                p2Loaded = false;
                loadTimer = new Timer(LoadPulse, null, 0, 1000);
                Spawner = new ArenaRandomGenerator(scaleX, scaleZ, firstSpawnPointPos, secondSpawnPointPos);
                ServerSendData.SendGameData(RoomIndex, player1.Nickname, player2.Nickname, spawnPos, spawnRot);
            }
        }

        public void SpawnTree(int ID)
        {
            lock (expectant)
            {
                if (_treeRandomed != Randoming.randomed)
                {
                    if (_treeRandomed != Randoming.randoming)
                    {
                        _treeRandomed = Randoming.randoming;
                        ObjectsList.Add(NetworkGameObject.ObjectType.tree, RoomIndex);
                        _treeRandomed = Randoming.randomed;
                        lock (expectant)
                            Monitor.Pulse(expectant);
                    }
                    else
                    {
                        lock (expectant)
                            Monitor.Wait(expectant);
                    }

                }
            }
            ServerSendData.SendTreeSpawned(ID, RoomIndex, ObjectsList.GetRange(NetworkGameObject.ObjectType.tree));
        }

        public void SpawnRock(int ID)
        {
            lock (expectant)
            {
                if (_rockRandomed != Randoming.randomed)
                {
                    if (_rockRandomed != Randoming.randoming)
                    {
                        _rockRandomed = Randoming.randoming;
                        ObjectsList.Add(NetworkGameObject.ObjectType.rock, RoomIndex);
                        _rockRandomed = Randoming.randomed;
                        lock (expectant)
                            Monitor.Pulse(expectant);
                    }
                    else
                    {
                        lock (expectant)
                            Monitor.Wait(expectant);
                    }
                }
            }
            ServerSendData.SendRockSpawned(ID, RoomIndex, ObjectsList.GetRange(NetworkGameObject.ObjectType.rock));
        }

        public void LoadSpells(int ID)
        {
            int[] spellsToLoadFirst = Global.data.GetSkillBuildData(player1.Nickname, player1.Race);
            int[] spellsToLoadSecond = Global.data.GetSkillBuildData(player2.Nickname, player2.Race);
            ServerSendData.SendLoadSpells(ID, RoomIndex, spellsToLoadFirst, spellsToLoadSecond);
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
                loadTimer.Dispose();
                ServerSendData.SendRoomStart(RoomIndex);
                StartGame();
            }
        }

        private void LoadPulse(object o)
        {
            lock (expectant)
                Monitor.Pulse(expectant);
        }

        #endregion

        #region Gets And Sets

        // Get

        public Player GetPlayer(int ID)
        {
            return ID == 1 ? player1 : player2;
        }

        public Socket GetSocket(int ID)
        {
            return (ID == 1) ? player1.Socket : player2.Socket;
        }

        public Socket Player1Socket
        {
            get { return player1.Socket; }
        }

        public Socket Player2Socket
        {
            get { return player2.Socket; }
        }

        public string Size
        {
            get { return scaleX + "x" + scaleZ; }
        }

        // Set

        public void SetTransform(int ID, float[] position, float[] rotation)
        {
            if (ID == 1)
            {
                player1.Transform = new float[][] { position, rotation };
            }
            else
            {
                player2.Transform = new float[][] { position, rotation };
            }
        }

        public void SetLoadProgress(int ID, float loadProgress)
        {
            if(ID == 1)
            {
                player1.Load = loadProgress;
                ServerSendData.SendEnemyProgress(1, RoomIndex, player2.Load);
            } else
            {
                player2.Load = loadProgress;
                ServerSendData.SendEnemyProgress(2, RoomIndex, player1.Load);
            }
        }

        #endregion

    }

    public class Player
    {
        #region Public Var

        public float[][] Transform
        {
            get { return new float[][] { _position, _rotation }; }
            set { _position = value[0]; _rotation = value[1]; }
        }

        public float[] Position // do not used (can be deleted) P.S. check it, mb already used
        {
            get { return _position; }
        }

        public string Race
        {
            get { return _race; }
        }

        public float Load
        {
            get { return _load; }
            set { _load = value; }
        }

        #endregion

        #region Private Var
        public string Nickname { get; private set; }
        public int Index { get; private set; }
        public Socket Socket { get; private set; }
        public int ID { get; private set; }
        public GameRoom Room { get; private set; }

        private float _load;
        private float[] _position;
        private float[] _rotation;
        private byte[] _buffer = new byte[Constants.BUFFER_SIZE];
        private bool _playing = false;
        private string _race;

        #endregion

        public Player(Socket socket, GameRoom room, int index, string nickname, int ID, float load, string race)
        {
            Socket = socket;
            Index = index;
            Nickname = nickname;
            Room = room;
            this.ID = ID;
            _load = load;
            _race = race;
        }

        public void StartPlay()
        {
            _playing = true;
            Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallBack), Socket);
        }

        private void PlayerReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int received = Socket.EndReceive(ar);
                if (received <= 0)
                {
                    Global.serverForm.Debug("GamePlayer " + Nickname + " lost connection");
                    PlayerClose();
                    Room.AbortGameSession(ID);
                }
                else
                {
                    byte[] dataBuffer = new byte[received];
                    Array.Copy(_buffer, dataBuffer, received);
                    if (_playing)
                        Socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallBack), Socket);
                    else
                        return;
                    ServerHandleRoomData.HandleNetworkInformation(ID, Room, dataBuffer);

                }
            }
            catch
            {
                Global.serverForm.Debug("GamePlayer " + Nickname + " lost connection");
                PlayerClose();
                Room.AbortGameSession(ID);
            }
        }

        public void PlayerStop()
        {
            _playing = false;
        }

        public void PlayerClose()
        {
            Global.players[Index].ClosePlayer();
            PlayerStop();   
        }

    }

}
