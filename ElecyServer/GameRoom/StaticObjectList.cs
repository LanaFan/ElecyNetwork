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

        public int[] Add(ObjectType type, BaseGameRoom room, int count, bool big, bool medium, bool small)
        {
            int number = Offset +  count;
            int[] ranges = new int[] { Offset, number - 1 };
            ChangeLength(number);
            _ranges.Add(type, ranges);
            while(Offset < number)
            {
                _list[Offset] = new NetworkGameObject(Offset, type, room, room.randomer.RandomHP(type, big, medium, small));
                Offset++;
            }
            return ranges;
        }

        public NetworkGameObject Get(int index)
        {
            return _list[index];
        }

        public bool GetRange(ObjectType type, out int[] ranges)
        {
            return _ranges.TryGetValue(type, out ranges);
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
