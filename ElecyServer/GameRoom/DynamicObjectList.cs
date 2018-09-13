using System;
using System.Collections;
using Bindings;

namespace ElecyServer
{
    public class DynamicObjectList : IEnumerable
    {

        #region Variables

        private DynamicObject[] _list;
        private BaseGameRoom _room;

        public int Length { get; private set; }

        #endregion

        #region Constructor

        public DynamicObjectList(BaseGameRoom room)
        {
            Length = 10;
            _list = new DynamicObject[Length];
            _room = room;
        }

        #endregion

        #region Commands

        public void Add(BaseGameRoom room, int spellIndex, int parentIndex, float[] spawnPos, float[] targetPos, float[] rot, int hp, string nickname)
        {
            int caster = -1;
            for (int i = 0; i < room.PlayersCount; i++)
            {
                if (room.playersTCP[i].nickname.Equals(nickname))
                {
                    caster = i;
                    break;
                }
            }
            int index = Add(room, hp, spawnPos, rot, caster);
            SendDataTCP.SendInstantiate(room, spellIndex, index, parentIndex, spawnPos, targetPos, rot, hp, nickname);
        }

        public void Destroy(int index)
        {
            lock (_list)
            {
                try
                {
                    if (_list[index] != null)
                    {
                        _list[index] = null;
                    }
                    SendDataTCP.SendDestroy(_room, index);
                }
                catch (IndexOutOfRangeException) { }
            }
        }

        public DynamicObject Get(int index)
        {
            return _list[index];
        }

        #endregion

        #region Private Commands

        private int Add(BaseGameRoom room, int hp, float[] pos, float[] rot, int caster)
        {
            lock (_list)
            {
                int index = -1;
                for (int i = 0; i < Length; i++)
                {
                    if (_list[i] == null)
                    {
                        _list[i] = new DynamicObject(i, ObjectType.spell, hp, pos, rot, caster);
                        index = i;
                        break;
                    }
                }
                if (index == -1)
                {
                    Length++;
                    Array.Resize(ref _list, Length);
                    index = Length - 1;
                    _list[index] = new DynamicObject(index, ObjectType.spell, hp, pos, rot, caster);
                }
                return index;
            }
        }

        public DynamicObject this[int index]
        {
            get => _list[index];
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public BaseRoomObjectEnum GetEnumerator()
        {
            return new BaseRoomObjectEnum(_list);
        }

        #endregion

    }
}
