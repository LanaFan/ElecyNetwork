using Bindings;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ElecyServer
{

    public abstract class BaseRoomObject
    {
        public int index;
        public ObjectType type;
        public bool isDestroyed;

        public Dictionary<int, MovementUpdate> positionUpdate;
        public float[] curPosition;
        public int curPosIndex;

        public int maxHp;
        public int curHp;

        protected object expectant;

        public BaseRoomObject(int index, ObjectType type, int hp, float[] position, float[] rotation)
        {
            this.index = index;
            this.type = type;
            isDestroyed = false;
            curPosition = new float[] { position[0], 0.5f, position[1] };
            curPosIndex = 1;
            expectant = new object();

            positionUpdate = new Dictionary<int, MovementUpdate>();
            positionUpdate.Add(curPosIndex, new MovementUpdate(curPosition));

        }

        #region Position update

        public void SetPosition(float[] Position, int Index)
        {
            lock (expectant)
            {
                if (positionUpdate.ContainsKey(Index))
                    return;
                if (curPosIndex < Index)
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
                    curPosIndex = Index;
                    curPosition[0] = Position[0];
                    curPosition[1] = Position[1];
                    curPosition[2] = Position[2];
                    positionUpdate.Add(curPosIndex, new MovementUpdate(curPosition));
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
                index = curPosIndex;
                if (positionUpdate.TryGetValue(curPosIndex, out update))
                    if (!update.sent)
                    {
                        update.sent = true; // mb not change the value (check bro)
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
                        curPosIndex = Index;
                        curPosition = stepBackBuffer.position;
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

        #endregion

        #region HP update

        public abstract void UpdateHP();

        public abstract void TakeDamage(int damage);

        #endregion

    }

    #region Enumerator

    public class BaseRoomObjectEnum : IEnumerator
    {
        private BaseRoomObject[] _objects;

        int position = -1;

        public BaseRoomObjectEnum(BaseRoomObject[] list)
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

        public BaseRoomObject Current
        {
            get
            {
                try
                {
                    return _objects[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    #endregion

}
