using System;
using System.Collections;
using Bindings;

namespace ElecyServer
{
    public class DynamicObjectList : IEnumerable
    {

        #region Variables

        private NetworkGameObject[] _list;

        public int Length { get; private set; }

        #endregion

        #region Constructor

        public DynamicObjectList()
        {
            Length = 10;
            _list = new NetworkGameObject[Length];
        }

        #endregion

        #region Commands

        public void Add(GameRoom room, int spellIndex, int parentIndex, float[] pos, float[] rot, int hp, string nickname)
        {
            int index = Add(room, hp, pos, rot);
            ServerSendData.SendInstantiate(room, spellIndex, index, parentIndex, pos, rot, hp, nickname);
        }

        #endregion

        #region Private Commands

        private int Add(GameRoom room, int hp, float[] pos, float[] rot)
        {
            lock (_list)
            {
                int index = -1;
                for (int i = 0; i < Length; i++)
                {
                    if (_list[i].Equals(default(NetworkGameObject)))
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
