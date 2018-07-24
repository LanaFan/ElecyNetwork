using System.Collections.Generic;

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

        public void Add(NetworkGameObject.ObjectType type, GameRoom room, int count, bool big = false, bool medium = true, bool small = false)
        {
            int number = Offset +  count;
            ranges.Add(type, new int[] { Offset, number - 1 });
            while(Offset < number)
            {
                objects[Offset] = new NetworkGameObject(Offset, type, room, room.Spawner.RandomHP(type, big, medium, small));
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
                oldObjects = null;
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
        public GameRoom Room { get; private set; } // use 
        public ObjectType Type { get; private set; } // use

        public enum ObjectType
        {
            unsigned = 0,
            tree = 1,
            rock = 2,
            spell = 3,
        }

        public NetworkGameObject(int index, ObjectType type, GameRoom room, int hp)
        {
            Index = index;
            Type = type;
            Room = room;
            HP = hp;
            SetTransform();
        }

        private void SetTransform()
        {
            float[] pos = Room.Spawner.RandomPosition(Type);
            float[] rot = Room.Spawner.RandomRotation();
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

        public (int, float[], float[]) GetInfo()
        {
            return (HP, Position, Rotation);
        }

    }
}
