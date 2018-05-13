using System;
using System.Collections;

namespace ElecyServer
{
    public class DynamicList : IEnumerable
    {
        DynamicGameObject[] objects;
        public int Length { get; private set; }
        public int Offset { get; private set; }
        private int roomIndex;
        
        public DynamicList(int roomIndex)
        {
            this.roomIndex = roomIndex;
            Length = 100;
            objects = new DynamicGameObject[Length];
            Offset = 0;
        }

        public int Add(int spell_Index, float[] pos, float[] rot, int ID)
        {
            for(int i = 0; i < Length; i++)
            {
                if (objects[i] != null)
                {
                    objects[i] = new DynamicGameObject(spell_Index, i, ID, pos, rot);
                    Offset++;
                    CheckLength();
                    return i;
                }
            }
            return -1;
        }

        public void Delete(int index)
        {
            if(objects[index] != null)
            {
                objects[index] = null;
                Offset--;
            }
        }

        public DynamicGameObject this[int index]
        {
            get
            {
                return objects[index];
            }
        }

        private void CheckLength()
        {
            if(Offset + 10 >= Length)
            {
                Array.Resize(ref objects, Length * 2);
            }
        }

        #region IEnumeration

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public DynamicEnum GetEnumerator()
        {
            return new DynamicEnum(objects);
        }

        #endregion
    }

    public class DynamicGameObject
    {
        public int Spell_Index { get; private set; }
        public int Index { get; private set; }
        public int Owner { get; private set; }
        public float[] Position { get; private set; }
        public float[] Rotation { get; private set; }

        public DynamicGameObject(int spell_index, int index, int ID, float[] pos, float[] rot)
        {
            Spell_Index = index;
            Index = index;
            Owner = ID;
            Position = pos;
            Rotation = rot;
        }

        public void Update(float[] pos, float[] rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }

    public class DynamicEnum : IEnumerator
    {
        DynamicGameObject[] objects;

        int position = -1;

        public DynamicEnum(DynamicGameObject[] list)
        {
            objects = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < objects.Length);
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

        public DynamicGameObject Current
        {
            get
            {
                try
                {
                    return objects[position];
                }
                catch (IndexOutOfRangeException e)
                {
                    Global.serverForm.Debug("Alert in DynamicList : \n" + e.Message);
                    return null; // don't forget to check
                }
            }
        }
    }

}
