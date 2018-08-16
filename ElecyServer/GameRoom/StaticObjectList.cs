using System;
using System.Collections;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    public class StaticObjectList : IEnumerable
    {

        #region Variables

        private NetworkGameObject[] _list;
        private Dictionary<ObjectType, int[]> _ranges;

        public int Length { get; private set; }
        public int Offset { get; private set; }

        #endregion

        #region Constructor

        public StaticObjectList()
        {
            Length = 0;
            _list = new NetworkGameObject[Length];
            _ranges = new Dictionary<ObjectType, int[]>();
            Offset = 0;
        }

        #endregion

        #region Commands

        public void Add(ObjectType type, GameRoom room, int count, bool big, bool medium, bool small)
        {
            int number = Offset +  count;
            ChangeLength(number);
            _ranges.Add(type, new int[] { Offset, number - 1 });
            while(Offset < number)
            {
                _list[Offset] = new NetworkGameObject(Offset, type, room, room.Spawner.RandomHP(type, big, medium, small));
                Offset++;
            }
        }

        public NetworkGameObject Get(int index)
        {
            return _list[index];
        }

        public int[] GetRange(ObjectType type)
        {
            return _ranges[type];
        }

        public void Clear()
        {
            Length = 0;
            _list = new NetworkGameObject[Length];
            _ranges = new Dictionary<ObjectType, int[]>();
            Offset = 0;
        }

        #endregion

        #region Private Commands

        private void ChangeLength(int number)
        {
            Length += number;
            Array.Resize(ref _list, Length);
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
