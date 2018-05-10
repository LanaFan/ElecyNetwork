using System;
using System.Threading;
using Bindings;

namespace ElecyServer
{
    public class DynamicList
    {
        DynamicGameObject[] objects;
        public int Length { get; private set; }
        public int Offset { get; private set; }
        
        public DynamicList()
        {
            Length = 100;
            objects = new DynamicGameObject[Length];
            Offset = 0;
        }

        public void Add(int index, float[] pos, float[] rot, int ID)
        {
            for(int i = 0; i < Length; i++)
            {
                if (objects[i] != null)
                {
                    objects[i] = new DynamicGameObject(index, i, ID, pos, rot);
                    Offset++;
                    CheckLength();
                    return;
                }
            }
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



    }

}
