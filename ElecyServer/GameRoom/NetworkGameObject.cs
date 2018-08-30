using System;
using System.Collections;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{

    public class NetworkGameObject
    {
        public readonly int index;
        public readonly int maxHP;
        public readonly BaseGameRoom room;
        public readonly ObjectType type;

        public float[] position;
        public float[] rotation;
        public bool isDestroyed;
        public int currHP;

        public Dictionary<int, MovementUpdate> positionUpdate;
        float[] _currentPosition;
        int _currentIndex;
        object expectant;

        public NetworkGameObject()
        {

        }

        public NetworkGameObject(int index, ObjectType type, BaseGameRoom room, int hp, float[] position = null, float[] rotation = null)
        {
            this.index = index;
            this.type = type;
            this.room = room;
            maxHP = currHP = hp;
            isDestroyed = false;

            float[] pos = room.randomer.RandomPosition(type);
            float[] rot = room.randomer.RandomRotation();
            this.position = position ?? new float[] { pos[0], 0.5f, pos[1] };
            _currentPosition = this.position;

            if (type == ObjectType.tree)
            {
                this.rotation = rotation ?? new float[] { 0, rot[1], 0, 1 };
            }
            else
            {
                this.rotation = rotation ?? new float[] { rot[0], rot[1], rot[2], 1 };
            }
            positionUpdate = new Dictionary<int, MovementUpdate>();
            positionUpdate.Add(1, new MovementUpdate(new float[] { _currentPosition[0], _currentPosition[1], _currentPosition[2] }));
            _currentIndex = 1;
            expectant = new object();
        }

        public void SetPosition(float[] Position, int Index)
        {
            lock (expectant)
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
            lock (expectant)
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
            lock (expectant)
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

        public (int, float[], float[]) GetInfo()
        {
            return (currHP, position, rotation);
        }
    }

    public class NetworkGameObjectEnum : IEnumerator
    {
        private NetworkGameObject[] _objects;

        int position = -1;

        public NetworkGameObjectEnum(NetworkGameObject[] list)
        {
            _objects = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _objects.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public NetworkGameObject Current
        {
            get
            {
                try
                {
                    return _objects[position];
                }
                catch(IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

}
