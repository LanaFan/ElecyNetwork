using System;
using System.Collections.Generic;
using System.Text;

namespace Bindings
{
    public class PacketBuffer : IDisposable
    {

        #region Variables

        List<byte> _bufferlist;
        byte[] _readbuffer;
        int _readpos;
        bool _buffupdate = false;

        #endregion

        #region Commands

        public PacketBuffer()
        {
            _bufferlist = new List<byte>();
            _readpos = 0;
        }

        public int GetReadPos()
        {
            return _readpos;
        }

        public byte[] ToArray()
        {
            return _bufferlist.ToArray();
        }

        public int Count()
        {
            return _bufferlist.Count;
        }

        public int Lenght()
        {
            return Count() - _readpos;
        }

        public void Clear()
        {
            _bufferlist.Clear();
            _readpos = 0;
        }

        #endregion

        #region Write

        public void WriteBytes(byte[] input)
        {
            _bufferlist.AddRange(input);
            _buffupdate = true;
        }
        public void WriteByte(byte input)
        {
            _bufferlist.Add(input);
            _buffupdate = true;
        }
        public void WriteInteger(int input)
        {
            _bufferlist.AddRange(BitConverter.GetBytes(input));
            _buffupdate = true;
        }
        public void WriteShort(short input)
        {
            _bufferlist.AddRange(BitConverter.GetBytes(input));
            _buffupdate = true;
        }
        public void WriteFloat(float input)
        {
            _bufferlist.AddRange(BitConverter.GetBytes(input));
            _buffupdate = true;
        }
        public void WriteString(string input)
        {
            _bufferlist.AddRange(BitConverter.GetBytes(Encoding.UTF8.GetByteCount(input)));
            _bufferlist.AddRange(Encoding.UTF8.GetBytes(input));
            _buffupdate = true;
        }
        public void WriteBoolean(bool input)
        {
            _bufferlist.Add(Convert.ToByte(input));
            _buffupdate = true;
        }
        #endregion

        #region Read

        public short ReadShort(bool peek = true)
        {
            if(_bufferlist.Count > _readpos)
            {
                if(_buffupdate)
                {
                    _readbuffer = _bufferlist.ToArray();
                    _buffupdate = false;
                }

                short value = BitConverter.ToInt16(_readbuffer, _readpos);
                if(peek & _bufferlist.Count > _readpos)
                {
                    _readpos += 2;
                }
                return value;
            }
            else
            {
                throw new Exception("(Ex) Buffer reached it's limit :(");
            }
        }
        public int ReadInteger(bool peek = true)
        {
            if(_bufferlist.Count > _readpos)
            {
                if(_buffupdate)
                {
                    _readbuffer = _bufferlist.ToArray();
                    _buffupdate = false; 
                }

                int value = BitConverter.ToInt32(_readbuffer, _readpos);
                if(peek & _bufferlist.Count >_readpos)
                {
                    _readpos += 4;
                }
                return value;
            }
            else
            {
                throw new Exception("(Ex) Buffer reached it's limit :(");
            }
        }
        public float ReadFloat(bool peek = true)
        {
            if (_bufferlist.Count > _readpos)
            {
                if (_buffupdate)
                {
                    _readbuffer = _bufferlist.ToArray();
                    _buffupdate = false;
                }

                float value = BitConverter.ToSingle(_readbuffer, _readpos);
                if (peek & _bufferlist.Count > _readpos)
                {
                    _readpos += 4;
                }
                return value;
            }
            else
            {
                throw new Exception("(Ex) Buffer reached it's limit :(");
            }
        }
        public byte ReadByte(bool peek = true)
        {
            if (_bufferlist.Count > _readpos)
            {
                if (_buffupdate)
                {
                    _readbuffer = _bufferlist.ToArray();
                    _buffupdate = false;
                }

                byte value = _readbuffer[_readpos];
                if (peek & _bufferlist.Count > _readpos)
                {
                    _readpos += 1;
                }
                return value;
            }
            else
            {
                throw new Exception("(Ex) Buffer reached it's limit :(");
            }
        }
        public byte[] ReadBytes(int length, bool peek = true)
        {
             if (_buffupdate)
             {
                    _readbuffer = _bufferlist.ToArray();
                    _buffupdate = false;
             }

             byte[] value = _bufferlist.GetRange(_readpos, length).ToArray();
             if (peek & _bufferlist.Count > _readpos)
             {
                    _readpos += length;
             }
             return value;
        }
        public string ReadString(bool peek = true)
        {
            int length = ReadInteger(true);
            if (_buffupdate)
            {
                _readbuffer = _bufferlist.ToArray();
                _buffupdate = false;
            }

            string value = Encoding.UTF8.GetString(_readbuffer, _readpos, length);
            if (peek & _bufferlist.Count > _readpos)
            {
                _readpos += length;
            }
            return value;   
        }
        public bool ReadBool(bool peek = true)
        {
            if (_bufferlist.Count > _readpos)
            {
                if (_buffupdate)
                {
                    _readbuffer = _bufferlist.ToArray();
                    _buffupdate = false;
                }

                bool value = BitConverter.ToBoolean(_readbuffer, _readpos);
                if (peek & _bufferlist.Count > _readpos)
                {
                    _readpos += 1;
                }
                return value;
            }
            else
            {
                throw new Exception("(Ex) Buffer reached it's limit :(");
            }
        }

        #endregion

        #region Dispose

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    _bufferlist.Clear();
                }
                _readpos = 0;
            }
            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
