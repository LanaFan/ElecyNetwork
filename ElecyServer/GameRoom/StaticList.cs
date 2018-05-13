using System;
using System.Collections.Generic;
using System.Collections;

namespace ElecyServer
{
    public class StaticList : IEnumerable
    {
        StaticGameObject[] objects;
        Dictionary<StaticGameObject.ObjectType, int[]> ranges;
        public int Length { get; private set; }
        public int Offset { get; private set; }

        public StaticList()
        {
            Length = 100;
            objects = new StaticGameObject[Length];
            ranges = new Dictionary<StaticGameObject.ObjectType, int[]>();
            Offset = 0;
        }

        public void Add(StaticGameObject.ObjectType type, int roomIndex)
        {
            int number = Offset +  ArenaRandomGenerator.NumberOfObjects(type);
            ranges.Add(type, new int[] { Offset, number - 1 });
            while(Offset < number)
            {
                objects[Offset] = new StaticGameObject(Offset, type, roomIndex);
                CheckLength();
                Offset++;
            }
        }

        public StaticGameObject this[int index]
        {
            get
            {
                return objects[index];
            }
        }

        public int[] GetRange(StaticGameObject.ObjectType type)
        {
            return ranges[type];
        }

        public void Clear()
        {
            Length = 100;
            objects = new StaticGameObject[Length];
            ranges = new Dictionary<StaticGameObject.ObjectType, int[]>();
            Offset = 0;
        }

        private void CheckLength()
        {
            if(Offset + 10 >= Length)
            {
                StaticGameObject[] oldObjects = objects;
                Length *= 2;
                objects = new StaticGameObject[Length];
                oldObjects.CopyTo(objects, 0);
            }
        }

        #region IEnumeration

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public StaticEnum GetEnumerator()
        {
            return new StaticEnum(objects);
        }

        #endregion

    }

    public class StaticGameObject
    {
        public int Index { get; private set; }
        public int[] effects;
        public float[] Position { get; private set; }
        public float[] Rotation { get; private set; }
        public bool IsDestroyed { get; private set; }
        public int HP { get; private set; }
        public int RoomIndex { get; private set; }
        public ObjectType Type { get; private set; }

        public enum ObjectType
        {
            unsigned = 0,
            tree = 1,
            rock = 2,
            spell = 3,
        }
        
        public StaticGameObject(int index, ObjectType type, int roomIndex)
        {
            Index = index;
            Type = type;
            RoomIndex = roomIndex;
            effects = new int[] { 0, 0 };
            SetTransform();
            SetHP();
        }

        public void UpdateGameObjects(int hp, int[] effects)
        {
            HP = hp;
            this.effects = effects;
        }

        private void SetTransform()
        {
            float[] pos = Global.arena[RoomIndex].Spawner.RandomPosition(Type);
            float[] rot = Global.arena[RoomIndex].Spawner.RandomRotation();
            Position = new float[] { pos[0], 0.5f, pos[1] };
            if(Type == ObjectType.tree)
            {
                Rotation = new float[] { 0, rot[1], 0, 1 };
            }
            else
            {
                Rotation = new float[] { rot[0], rot[1], rot[2], 1 };
            }
        }

        private void SetHP()
        {
            switch (Type)
            {
                case ObjectType.unsigned:
                    break;
                case ObjectType.tree:
                    HP = 20;
                    break;
                case ObjectType.rock:
                    HP = 30;
                    break;
            }
        }

    }

    public class StaticEnum : IEnumerator
    {

        public StaticGameObject[] objects;

        int position = -1;

        public StaticEnum(StaticGameObject[] list)
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

        public StaticGameObject Current
        {
            get
            {
                try
                {
                    return objects[position];
                }
                catch (IndexOutOfRangeException e)
                {
                    Global.serverForm.Debug("Alert in StaticList: \n" + e.Message);
                    return null; // don't forget to check
                }
            }
        }
    }

}
