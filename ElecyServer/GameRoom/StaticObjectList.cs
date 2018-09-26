using System;
using System.Collections;
using System.Collections.Generic;
using Bindings;

namespace ElecyServer
{
    public class StaticObjectList : IEnumerable
    {

        #region Variables

        private StaticObject[] _list;
        private Dictionary<StaticTypes, int[]> _ranges;

        public int Length { get; private set; }
        public int Offset { get; private set; }

        #endregion

        #region Constructor

        public StaticObjectList()
        {
            Length = 0;
            _list = new StaticObject[Length];
            _ranges = new Dictionary<StaticTypes, int[]>();
            Offset = 0;
        }

        #endregion

        #region Commands

        public int[] Add(StaticTypes staticType, ObjectType type, BaseGameRoom room, int count, bool big, bool medium, bool small)
        {
            int number = Offset +  count;
            int[] ranges = new int[] { Offset, number - 1 };
            ChangeLength(number);
            _ranges.Add(staticType, ranges);
            while(Offset < number)
            {
                _list[Offset] = new StaticObject(Offset,
                                                staticType,
                                                type, 
                                                room.randomer.RandomHP(staticType, big, medium, small),
                                                room.randomer.RandomPosition(type),
                                                room.randomer.RandomRotation());
                Offset++;
            }
            return ranges;
        }

        public StaticObject Get(int index)
        {
            return _list[index];
        }

        public bool GetRange(StaticTypes type, out int[] ranges)
        {
            return _ranges.TryGetValue(type, out ranges);
        }

        public void Clear()
        {
            Length = 0;
            _list = new StaticObject[Length];
            _ranges = new Dictionary<StaticTypes, int[]>();
            Offset = 0;
        }

        public StaticObject this[int index]
        {
            get => _list[index];
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

        public BaseRoomObjectEnum GetEnumerator()
        {
            return new BaseRoomObjectEnum(_list);
        }

        #endregion

    }

}
