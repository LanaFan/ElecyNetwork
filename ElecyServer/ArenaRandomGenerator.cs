using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecyServer
{
    public class ArenaRandomGenerator
    {
        float sizeX;
        float sizeZ;
        float[] player1Pos;
        float[] player2Pos;
        float[] xRange;
        float[] zRange;
        List<SpaceX> xSpaces;
        List<SpaceZ> zSpaces;
        Random rnd;

        public ArenaRandomGenerator(float scaleX, float scaleZ, float[] player1Pos, float[] player2Pos)
        {
            rnd = new Random();
            sizeX = scaleX;
            sizeZ = scaleZ;
            this.player1Pos = player1Pos;
            this.player2Pos = player2Pos;
            SetXRange(sizeX);
            SetZRange(sizeZ);
            xSpaces = new List<SpaceX>();
            zSpaces = new List<SpaceZ>();
            SetXSpace(player1Pos[0], 0);
            SetZSpace(player1Pos[1], 0);
            SetXSpace(player2Pos[0], 0);
            SetZSpace(player2Pos[1], 0);
        }

        public static int NumberOfObjects(NetworkGameObject.Type type)
        {
            Random rnd = new Random();
            switch (type)
            {
                case NetworkGameObject.Type.rock:
                    return rnd.Next(15, 25);
                case NetworkGameObject.Type.tree:
                    return rnd.Next(15, 20);
            }
            return 0;
        }

        public float[] RandomPosition(NetworkGameObject.Type type)
        {
            float[] pos = new float[2];
            bool randomed = true;
            do
            {
                pos[0] = (float)(rnd.NextDouble() * (xRange[1] - xRange[0]) + xRange[0]);
                pos[1] = (float)(rnd.NextDouble() * (zRange[1] - zRange[0]) + zRange[0]);
                randomed = true;
                for (int i = 0; i < xSpaces.Count(); i++)
                {
                    if ((pos[0] <= xSpaces.ElementAt(i).xTo && pos[0] >= xSpaces.ElementAt(i).xFrom) && (pos[1] <= zSpaces.ElementAt(i).zTo && pos[1] >= zSpaces.ElementAt(i).zFrom))
                    {
                        randomed = false;
                        break;
                    }
                }
            }
            while (!randomed);
            xSpaces.Add(new SpaceX(pos[0], type));
            zSpaces.Add(new SpaceZ(pos[1], type));
            return pos;
        }

        public float[] RandomRotation()
        {
            float[] rot = new float[3];
            rot[0] = (float)(rnd.NextDouble() * 360);
            rot[1] = (float)(rnd.NextDouble() * 360);
            rot[2] = (float)(rnd.NextDouble() * 360);
            return rot;
        }

        private void SetXSpace(float pos, NetworkGameObject.Type type)
        {
            xSpaces.Add(new SpaceX(pos, type));
        }

        private void SetZSpace(float pos, NetworkGameObject.Type type)
        {
            zSpaces.Add(new SpaceZ(pos, type));
        }

        private void SetXRange(float size)
        {
            xRange = new float[2];
            xRange[0] = -((size / 2f) - 1f);
            xRange[1] = ((size / 2f) - 1f);
        }

        private void SetZRange(float size)
        {
            zRange = new float[2];
            zRange[0] = -((size / 2f) - 1f);
            zRange[1] = ((size / 2f) - 1f);
        }
    }

    class SpaceX
    {
        public float xFrom;
        public float xTo;
        float pos;
        NetworkGameObject.Type type;

        public SpaceX(float pos, NetworkGameObject.Type type)
        {
            this.pos = pos;
            this.type = type;
            SetSpace();
        }

        private void SetSpace()
        {
            switch (type)
            {
                case NetworkGameObject.Type.unsigned:
                    xFrom = pos - 10;
                    xTo = pos + 10;
                    break;
                case NetworkGameObject.Type.rock:
                    xFrom = pos - 5;
                    xTo = pos + 5;
                    break;
                case NetworkGameObject.Type.tree:
                    xFrom = pos - 7;
                    xTo = pos + 7;
                    break;
            }
        }

    }

    class SpaceZ
    {
        public float zFrom;
        public float zTo;
        float pos;
        NetworkGameObject.Type type;

        public SpaceZ(float pos, NetworkGameObject.Type type)
        {
            this.pos = pos;
            this.type = type;
            SetSpace();
        }

        private void SetSpace()
        {
            switch (type)
            {
                case NetworkGameObject.Type.unsigned:
                    zFrom = pos - 10;
                    zTo = pos + 10;
                    break;
                case NetworkGameObject.Type.rock:
                    zFrom = pos - 5;
                    zTo = pos + 5;
                    break;
                case NetworkGameObject.Type.tree:
                    zFrom = pos - 7;
                    zTo = pos + 7;
                    break;
            }
        }
    }



}
