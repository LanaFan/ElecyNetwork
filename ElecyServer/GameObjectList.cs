using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecyServer
{
    public class GameObjectList
    {
        NetworkGameObject[] objects;
        Dictionary<NetworkGameObject.ObjectType, int[]> ranges;
        public int Length { get; private set; }
        public int Offset { get; private set; }

        public GameObjectList()
        {
            Length = 100;
            objects = new NetworkGameObject[Length];
            ranges = new Dictionary<NetworkGameObject.ObjectType, int[]>();
            Offset = 0;
        }

        public void Add(NetworkGameObject.ObjectType type, int roomIndex)
        {
            int number = Offset +  ArenaRandomGenerator.NumberOfObjects(type);
            ranges.Add(type, new int[] { Offset, number - 1 });
            while(Offset < number)
            {
                objects[Offset] = new NetworkGameObject(Offset, type, roomIndex);
                CheckLength();
                Offset++;
            }
        }

        public NetworkGameObject Get(int index)
        {
            return objects[index];
        }

        public int[] GetRange(NetworkGameObject.ObjectType type)
        {
            return ranges[type];
        }

        public void Clear()
        {
            Length = 100;
            objects = new NetworkGameObject[Length];
            ranges = new Dictionary<NetworkGameObject.ObjectType, int[]>();
            Offset = 0;
        }

        private void CheckLength()
        {
            if(Offset + 10 >= Length)
            {
                NetworkGameObject[] oldObjects = objects;
                Length *= 2;
                objects = new NetworkGameObject[Length];
                oldObjects.CopyTo(objects, 0);
            }
        }
    }

    public class NetworkGameObject
    {
        public int Index { get; private set; }
        public float[] Position { get; private set; }
        public float[] Rotation { get; private set; }
        public bool IsDestroyed { get; private set; }
        public int HP { get; private set; }
        public int RoomIndex { get; private set; } // use 
        public ObjectType Type { get; private set; } // use

        public enum ObjectType
        {
            unsigned = 0,
            tree = 1,
            rock = 2,
            spell = 3,
        }

        public NetworkGameObject(int index, ObjectType type, int roomIndex)
        {
            Index = index;
            Type = type;
            RoomIndex = roomIndex;
            SetTransform();
            SetHP();
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
}
