using System;
using System.Collections;
using Bindings;

namespace ElecyServer
{
    public class DynamicObjectList : IEnumerable
    {

        #region Variables

        private NetworkGameObject[] _list;
        private BaseGameRoom _room;

        public int Length { get; private set; }

        #endregion

        #region Constructor

        public DynamicObjectList(BaseGameRoom room)
        {
            Length = 10;
            _list = new NetworkGameObject[Length];
            _room = room;
        }

        #endregion

        #region Commands

        public void Add(BaseGameRoom room, int spellIndex, int parentIndex, float[] spawnPos, float[] targetPos, float[] rot, int hp, string nickname)
        {
            int index = Add(room, hp, spawnPos, rot);
            SendDataTCP.SendInstantiate(room, spellIndex, index, parentIndex, spawnPos, targetPos, rot, hp, nickname);
        }

        public void Destroy(int index)
        {
            lock(_list)
            {
                try
                {
                    if (_list[index] != null)
                    {
                        _list[index] = null;
                    }
                    SendDataTCP.SendDestroy(_room, index);
                }
                catch(IndexOutOfRangeException) { }
            }
        }

        public NetworkGameObject Get(int index)
        {
            return _list[index];
        }

        #endregion

        #region Private Commands

        private int Add(BaseGameRoom room, int hp, float[] pos, float[] rot)
        {
            lock (_list)
            {
                int index = -1;
                for (int i = 0; i < Length; i++)
                {
                    if (_list[i] == null)
                    {
                        _list[i] = new NetworkGameObject(i, ObjectType.spell, room, hp, pos, rot);
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    Length++;
                    Array.Resize(ref _list, Length);
                    index = Length - 1;
                    _list[index] = new NetworkGameObject(index, ObjectType.spell, room, hp, pos, rot);
                }
                return index;
            }
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public NetworkGameObjectEnum GetEnumerator()
        {
            return new NetworkGameObjectEnum(_list);
        }

        #endregion

    }
}
