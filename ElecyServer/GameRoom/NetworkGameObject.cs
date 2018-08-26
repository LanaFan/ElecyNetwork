using System;
using System.Collections;
using Bindings;

namespace ElecyServer
{

    public struct NetworkGameObject
    {
        public readonly int index;
        public readonly int maxHP;
        public readonly BaseGameRoom room;
        public readonly ObjectType type;

        public float[] position;
        public float[] rotation;
        public bool isDestroyed;
        public int currHP;

        public NetworkGameObject(int index, ObjectType type, BaseGameRoom room, int hp, float[] position = null, float[] rotation = null)
        {
            this.index = index;
            this.type = type;
            this.room = room;
            maxHP = currHP = hp;
            isDestroyed = false;

            float[] pos = room.randomer.RandomPosition(type);
            float[] rot = room.randomer.RandomRotation();
            this.position = position ?? new float[] { pos[0], 0.5f, pos[1] };

            if (type == ObjectType.tree)
            {
                this.rotation = rotation ?? new float[] { 0, rot[1], 0, 1 };
            }
            else
            {
                this.rotation = rotation ?? new float[] { rot[0], rot[1], rot[2], 1 };
            }
        }

        public (int, float[], float[]) GetInfo()
        {
            return (currHP, position, rotation);
        }
    }

    public class NetworkGameObjectEnum : IEnumerator
    {
        private NetworkGameObject[] _objects;

        int position = -1;

        public NetworkGameObjectEnum(NetworkGameObject[] list)
        {
            _objects = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _objects.Length);
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

        public NetworkGameObject Current
        {
            get
            {
                try
                {
                    return _objects[position];
                }
                catch(IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

}
