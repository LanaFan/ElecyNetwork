using System;
using System.Net.Sockets;
using Bindings;

namespace ElecyServer
{
    public class GameRoom
    {
        public bool active = false;

        private Player player1;
        private Player player2;

        private float[] p1pos;
        private float[] p1rot;

        private float[] p2pos;
        private float[] p2rot;
        
        public GameRoom(NetPlayer player1, NetPlayer player2)
        {
            p1pos = new float[3];
            p1pos[0] = -10f;
            p1pos[1] = 0.5f;
            p1pos[2] = 0f;
            p2pos = new float[3];
            p2pos[0] = 10f;
            p2pos[1] = 0.5f;
            p2pos[2] = 0f;
            p1rot = new float[4];
            p2rot = new float[4];
            this.player1 = new Player(player1.playerSocket, player1.index, player1.nickname, p1pos, p1rot);
            this.player2 = new Player(player2.playerSocket, player2.index, player2.nickname, p2pos, p2rot);
            //StartGame();
        }

        private void StartGame()
        {
            player1.StartPlay();
            player2.StartPlay();
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
    }

    public class Player
    {
        private int _index;
        private string _nickname;
        private float[] _position;
        private float[] _rotation;
        private Socket _socket;
        private byte[] _buffer = new byte[Constants.BUFFER_SIZE];
        private bool _playing = false;

        public Player(Socket socket, int index, string nickname, float[] position, float[] rotation)
        {
            _socket = socket;
            _index = index;
            _nickname = nickname;
            _position = position;
            _rotation = rotation;
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
                    ServerHandleNetworkData.HandleNetworkInformation(_index, dataBuffer);
                    if (!_playing)
                        _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(PlayerReceiveCallBack), _socket);
                    else
                        return;
                }
            }
            catch
            {

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

        public string GetNickname()
        {
            return _nickname;
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
