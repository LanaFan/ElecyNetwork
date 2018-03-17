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
        Dictionary<NetworkGameObject.Type, int[]> ranges;
        int length;
        int offset;

        public GameObjectList()
        {
            length = 100;
            objects = new NetworkGameObject[length];
            offset = 0;
        }

        public void Add(NetworkGameObject.Type type, int roomIndex)
        {
            int number = offset +  ArenaRandomGenerator.NumberOfObjects(NetworkGameObject.Type.rock); 
            int[] range = new int[2];
            range[0] = offset;
            while(offset < number)
            {
                objects[offset] = new NetworkGameObject(offset, type, roomIndex);
                CheckLength();
                offset++;
            }
            range[1] = offset - 1;
            ranges.Add(type, range);
            SendAdded(type, roomIndex, range[0], range[1]);
        }

        public void SendAdded(NetworkGameObject.Type type, int roomIndex, int start, int end)
        {
            switch (type)
            {
                case NetworkGameObject.Type.rock:
                    ServerSendData.SendRockSpawned(roomIndex, start, end);
                    break;
                case NetworkGameObject.Type.tree:
                    ServerSendData.SendTreeSpawned(roomIndex, start, end);
                    break;
            }
        }

        public NetworkGameObject Get(int index)
        {
            return objects[index];
        }

        public void Clear()
        {
            length = 100;
            objects = new NetworkGameObject[length];
            offset = 0;
        }

        private void CheckLength()
        {
            if(offset + 10 >= length)
            {
                NetworkGameObject[] oldObjects = new NetworkGameObject[length];
                objects.CopyTo(oldObjects, 0);
                length *= 2;
                objects = new NetworkGameObject[length];
                oldObjects.CopyTo(objects, 0);
            }
        }
    }

    public class NetworkGameObject
    {
        int index;
        int roomIndex;
        int hp;
        float posX;
        float posY;
        float posZ;
        float rotX;
        float rotY;
        float rotZ;
        float rotW;
        Type type;
        bool isDestroyed;

        public enum Type
        {
            unsigned = 0,
            tree = 1,
            rock = 2,
        }

        public NetworkGameObject(int index, Type type, int roomIndex)
        {
            this.index = index;
            this.type = type;
            this.roomIndex = roomIndex;
            SetTransform();
            SetHP();
        }

        private void SetTransform()
        {
            float[] pos = Global.arena[roomIndex].GetRandom().RandomPosition();
            float[] rot = Global.arena[roomIndex].GetRandom().RandomRotation();
            posX = pos[0];
            posY = 0.5f;
            posZ = pos[1];
            rotX = rot[0];
            rotY = rot[1];
            rotZ = rot[2];
            rotW = 1;
        }

        private void SetHP()
        {
            switch (type)
            {
                case Type.unsigned:
                    break;
                case Type.tree:
                    hp = 20;
                    break;
                case Type.rock:
                    hp = 30;
                    break;
            }

        }

        public float[] GetPos()
        {
            return new float[] { posX, posY, posZ };
        }

        public float[] GetRot()
        {
            return new float[] { rotX, rotY, rotZ, rotW };
        }

        public int GetIndex()
        {
            return index;
        }

    }
}
