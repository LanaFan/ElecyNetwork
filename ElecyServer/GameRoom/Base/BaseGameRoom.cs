using Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ElecyServer
{
    public abstract class BaseGameRoom
    {

        #region Public Properties

        public ClientTCP[] playersTCP { get; }
        public GamePlayerUDP[] playersUDP { get; }
        public RoomPlayer[] roomPlayers { get; }

        public DynamicObjectList dynamicObjectsList { get; }
        public StaticObjectList staticObjectsList { get; }

        public ArenaRandomGenerator randomer { get; }
        public RoomState Status { get; private set; }

        public RoomTypes roomType;

        public int PlayersCount { get; }

        public Map map;

        #endregion

        #region Private Members

        private int _mapIndex;
        private float _scaleX;
        private float _scaleZ;

        private bool[] _loadedTCP;
        private bool[] _loadedUDP;

        private Timer _updateTimer;
        private Timer _closeTimer;

        private object expectant;

        #endregion

        #region Constructor

        public BaseGameRoom(ClientTCP client, int playersCount, int mapIndex = 0)
        {
            // Total number of players that can be in one room
            PlayersCount = playersCount;

            expectant = new object();

            RoomType();

            // Create lists of Players (TCP, UDP and struct contains player's game parameters)
            playersTCP = new ClientTCP[playersCount];
            playersUDP = new GamePlayerUDP[playersCount];
            roomPlayers = new RoomPlayer[playersCount];

            //Create list of dynamic objects
            dynamicObjectsList = new DynamicObjectList(this);

            //Create list of static objects
            staticObjectsList = new StaticObjectList();

            //Randomize mapIndex if it's not specified
            _mapIndex = mapIndex == 0 ? new Random().Next(1, Constants.MAPS_COUNT) : mapIndex;

            //Default room status
            Status = RoomState.Searching;

            // Load indicators
            _loadedTCP = new bool[playersCount];
            _loadedUDP = new bool[playersCount];

            // Get Map
            map = Global.data.GetMap(_mapIndex);

            //Create randomizer
            randomer = new ArenaRandomGenerator(map);

            // Adds player to the room
            AddPlayer(client);
        }

        #endregion

        #region Public Methods

        public bool AddPlayer(ClientTCP client)
        {
            lock(expectant)
            {
                for(int i = 0; i < PlayersCount; i++)
                {
                    if(playersTCP[i] == null)
                    {
                        playersTCP[i] = client;
                        client.EnterRoom(this, playersUDP[i] = new GamePlayerUDP(client.ip, i, this), i);
                        if (PlayersCount == playersTCP.Count(c => c != null))
                            StartLoad();
                        return true;
                    }
                }
                return false;
            }
        }

        public void DeletePlayer(ClientTCP client)
        {
            lock(expectant)
            {
                client.LeaveRoom();
                if (playersTCP[client.ID] != null)
                {
                    playersTCP[client.ID] = null;
                    playersUDP[client.ID] = null;
                    roomPlayers[client.ID] = null;
                }
                if(playersTCP.Count(c => c != null) == 0)
                {
                    CloseRoom();
                }
            }
        }

        public void UDPConnected(int index)
        {
            _loadedUDP[index] = true;
        }

        #endregion

        #region Loading

        public void SetGameArea(ClientTCP client)
        {
            SendDataTCP.SendMapData(_mapIndex, client);
        }

        public void SetPlayers(ClientTCP client)
        {
            SendDataTCP.SendPlayersSpawned(client, this);
        }

        public void SpawnRock(ClientTCP client, int rockCount, bool bigRock, bool mediumRock, bool smallRock)
        {
            int[] ranges;
            lock(expectant)
            {
                if(!staticObjectsList.GetRange(ObjectType.rock, out ranges))
                {
                    ranges = staticObjectsList.Add(ObjectType.rock, this, rockCount, bigRock, mediumRock, smallRock);
                }
            }
            SendDataTCP.SendRockSpawned(client, ranges);
        }

        public void SpawnTree(ClientTCP client, int treeCount, bool bigTree, bool mediumTree, bool smallTree)
        {
            int[] ranges;
            lock (expectant)
            {
                if (!staticObjectsList.GetRange(ObjectType.tree, out ranges))
                {
                    ranges = staticObjectsList.Add(ObjectType.tree, this, treeCount, bigTree, mediumTree, smallTree);
                }
            }
            SendDataTCP.SendTreeSpawned(client, ranges);
        }

        public void LoadSpells(ClientTCP client)
        {
            short[][] spellBuilds = new short[PlayersCount][];
            int totalNumber = 0;
            for(int i = 0; i < PlayersCount; i++)
            {
                spellBuilds[i] = client.accountData.AccountSkillBuilds.IgnisBuild.ToArray();
                totalNumber += spellBuilds[i].Length;
            }
            SendDataTCP.SendLoadSpells(client, spellBuilds, totalNumber);
        }

        public void LoadComplite(ClientTCP client)
        {
            lock(expectant)
            {
                _loadedTCP[client.ID] = true;
                foreach(bool loaded in _loadedTCP)
                {
                    if (!loaded)
                        return;
                }
                foreach(bool loaded in _loadedUDP)
                {
                    if (!loaded)
                        return;
                }
                Status = RoomState.Playing;
                SendDataTCP.SendRoomStart(this);
                StartUpdate();
            }
        }

        public void SetLoadProgress(ClientTCP client, float loadProgress)
        { 
            SendDataTCP.SendEnemyProgress(this, client, client.load = loadProgress);
        }

        #endregion

        #region Update Timer

        public void StartUpdate()
        {
            _updateTimer = new Timer(UpdateTimerCallback, null, 5000, 1000 / Constants.UPDATE_RATE);
        }

        public void UpdateTimerCallback(object o)
        {
            for (int i = 0; i < roomPlayers.Length; i++)
            {
                if (roomPlayers[i].GetPosition(out MovementUpdate update, out int index))
                {
                    SendDataUDP.SendTransformUpdate(this, ObjectType.player, i, update.position, index);
                }
            }
            for (int i = 0; i < dynamicObjectsList.Length; i++)
            {
                if(dynamicObjectsList.Get(i) != null) 
                {
                    if (dynamicObjectsList.Get(i).GetPosition(out MovementUpdate update, out int index))
                    {
                        SendDataUDP.SendTransformUpdate(this, ObjectType.spell, i, update.position, index);
                    }
                }
            }
        }

        protected void CloseUpdateTimer()
        {
            try
            {
                _updateTimer.Dispose();
            }
            catch { }
        }

        #endregion

        #region Finalization

        public void Surrended(ClientTCP client)
        {
            CloseUpdateTimer();
            Status = RoomState.MatchEnded;
            foreach (ClientTCP player in playersTCP)
                player.playerState = NetPlayerState.EndPlaying;
            _closeTimer = new Timer(EndGameSession, null, 300000, Timeout.Infinite);
            SendDataTCP.SendMatchEnded(client.nickname, this);
        }

        public void AbortGameSession(ClientTCP client)
        {
            if (Status != RoomState.MatchEnded)
            {
                CloseUpdateTimer();
                Status = RoomState.MatchEnded;
                foreach (ClientTCP player in playersTCP)
                    player.playerState = NetPlayerState.EndPlaying;
                _closeTimer = new Timer(EndGameSession, null, 300000, Timeout.Infinite);
                DeletePlayer(client);
                SendDataTCP.SendMatchEnded(client.nickname, this);
            }
            else
            {
                DeletePlayer(client);
            }
        }

        public void CloseRoom()
        {
            StopTimers();
            Global.roomsList.Remove(this);
        }


        #endregion

        #region Private Helpers

        private void StartLoad()
        {
            Status = RoomState.Loading;
            foreach(ClientTCP player in playersTCP)
            {
                player.clientState = ClientTCPState.GameRoom;
                player.playerState = NetPlayerState.Playing;
            }
            SendDataTCP.SendMatchFound(this);
        }

        private void StopTimers()
        {
            if (_closeTimer != null)
                _closeTimer.Dispose();
            CloseUpdateTimer();
        }

        protected internal abstract void RoomType();

        #endregion

        #region Close Timer Callback

        private void EndGameSession(Object o)
        {
            _closeTimer.Dispose();
            for(int i = 0; i < PlayersCount; i++)
            {
                if(playersTCP[i] != null)
                {
                    SendDataTCP.SendRoomLogOut(playersTCP[i]);
                    DeletePlayer(playersTCP[i]);
                }
            }
        }

        #endregion
    }

    public class RoomPlayer
    {
        float[] _currentPosition;
        int _currentIndex = 1;
        object expectant;

        protected Dictionary<int, MovementUpdate> positionUpdate = new Dictionary<int, MovementUpdate>();

        public RoomPlayer(float[] StartPosition)
        {
            
            positionUpdate.Add(1, new MovementUpdate(new float[] { StartPosition[0], 0.5f, StartPosition[1] }));
            _currentPosition = new float[] { StartPosition[0], 0.5f, StartPosition[1] };
            expectant = new object();
        }

        public void SetPosition(float[] Position, int Index)
        {
            lock(expectant)
            {
                if (_currentIndex < Index)
                {
                    if (positionUpdate.Count > 20)
                    {
                        if (positionUpdate.TryGetValue(1, out MovementUpdate buffer))
                        {
                            positionUpdate.Clear();
                            positionUpdate.Add(1, buffer);
                        }
                        else
                            Global.serverForm.Debug("There is no start position in memory");
                    }
                    _currentIndex = Index;
                    _currentPosition[0] = Position[0];
                    _currentPosition[1] = Position[1];
                    _currentPosition[2] = Position[2];
                    positionUpdate.Add(_currentIndex, new MovementUpdate(_currentPosition));
                }
                else
                {
                    positionUpdate.Add(Index, new MovementUpdate(Position));
                }
            }
        }

        public bool GetPosition(out MovementUpdate update, out int index)
        {
            lock(expectant)
            {
                index = _currentIndex;
                if (positionUpdate.TryGetValue(_currentIndex, out update))
                    if (!update.sent)
                    {
                        update.sent = true;
                        return true;
                    }
                return false;
            }
        }

        public void UdpateStepBack(int Index)
        {
            lock(expectant)
            {
                if (positionUpdate.TryGetValue(1, out MovementUpdate buffer))
                {
                    if (positionUpdate.TryGetValue(Index, out MovementUpdate stepBackBuffer))
                    {
                        _currentIndex = Index;
                        _currentPosition = stepBackBuffer.position;
                        positionUpdate.Clear();
                        positionUpdate.Add(1, buffer);
                        positionUpdate.Add(Index, stepBackBuffer);
                    }
                    else
                    {
                        Global.serverForm.Debug("There is no stepback point");
                    }
                }
                else
                {
                    Global.serverForm.Debug("There is no start point");
                }
            }
        }
    }

    public struct MovementUpdate
    {
        public float[] position { get; }
        public bool sent { get; set; }

        public MovementUpdate(float[] Position)
        {
            position = Position;
            sent = false;
        }
    }

}
