using System;
using System.Collections.Generic;
using System.Linq;
using Bindings;

namespace ElecyServer
{

    public class ArenaRandomGenerator
    {
        private float[] xRange;
        private float[] zRange;
        private List<Space> spaces;
        private Random rnd;

        public ArenaRandomGenerator(Map map)
        {
            rnd = new Random();
            spaces = new List<Space>();
            SetXRange(map.MapHeight * 10f);
            SetZRange(map.MapWidth * 10f);
            foreach(SpawnPoint pos in map.SpawnPoints)
            {
                spaces.Add(new Space(pos.PositionX, pos.PositionY, 0));
            }
        }

        //public static int NumberOfObjects(ObjectType type)
        //{
        //    Random rnd = new Random();
        //    switch (type)
        //    {
        //        case ObjectType.rock:
        //            return rnd.Next(15, 25);
        //        case ObjectType.tree:
        //            return rnd.Next(15, 20);
        //    }
        //    return 0;
        //}

        public float[] RandomPosition(ObjectType type)
        {
            float[] pos = new float[3];
            bool randomed = true;
            do
            {
                pos[0] = (float)(rnd.NextDouble() * (xRange[1] - xRange[0]) + xRange[0]);
                pos[1] = 0.5f;
                pos[2] = (float)(rnd.NextDouble() * (zRange[1] - zRange[0]) + zRange[0]);
                randomed = true;
                for (int i = 0; i < spaces.Count(); i++)
                {
                    if ((pos[0] <= spaces.ElementAt(i).xTo && pos[0] >= spaces.ElementAt(i).xFrom) && (pos[2] <= spaces.ElementAt(i).zTo && pos[2] >= spaces.ElementAt(i).zFrom))
                    {
                        randomed = false;
                        break;
                    }
                }
            }
            while (!randomed);
            spaces.Add(new Space(pos[0], pos[2], type));
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

        public int RandomHP(StaticTypes type, bool big, bool medium, bool small)
        {
            int min = 0;
            int max = 0;
            switch (type)
            {
                case StaticTypes.tree:
                    if(big)
                    {
                        max = Constants.bTreeHP;
                        min = Constants.bTreeHP - Constants.treeDiff;
                    }
                    if(medium)
                    {
                        if (!big)
                            max = Constants.mTreeHP;
                        min = Constants.mTreeHP - Constants.treeDiff;
                    }
                    if(small)
                    {
                        if (!big && !medium)
                            max = Constants.sTreeHP;
                        min = Constants.sTreeHP - Constants.treeDiff;
                    }
                    break;
                case StaticTypes.rock:
                    if (big)
                    {
                        max = Constants.bRockHP;
                        min = Constants.bRockHP - Constants.rockDiff;
                    }
                    if (medium)
                    {
                        if (!big)
                            max = Constants.mRockHP;
                        min = Constants.mRockHP - Constants.rockDiff;
                    }
                    if (small)
                    {
                        if (!big && !medium)
                            max = Constants.sRockHP;
                        min = Constants.sRockHP - Constants.rockDiff;
                    }
                    break;
            }
            return rnd.Next(min, max);
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

    class Space
    {
        public float xFrom { get; private set; }
        public float zFrom { get; private set; }
        public float xTo { get; private set; }
        public float zTo { get; private set; }

        private float posX;
        private float posZ;
        private ObjectType type;

        public Space(float posX, float posZ, ObjectType type)
        {
            this.posX = posX;
            this.posZ = posZ;
            this.type = type;
            SetSpace();
        }

        private void SetSpace()
        {
            switch (type)
            {
                case ObjectType.player:
                    xFrom = posX - 10f;
                    zFrom = posZ - 10f;
                    xTo = posX + 10f;
                    zTo = posZ + 10f;
                    break;
                case ObjectType.staticObjects:
                    xFrom = posX - 5f;
                    zFrom = posZ - 5f;
                    xTo = posX + 5f;
                    zTo = posZ + 5f;
                    break;
            }
        }

    }

}
