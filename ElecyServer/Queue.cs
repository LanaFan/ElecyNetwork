using Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElecyServer
{
    public static class Queue
    {
        public static void StopSearch(int index)
        {
            for(int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if(Global.normalQueue[i] == index)
                {
                    Global.normalQueue[i] = 0;
                    break;
                }
            }
        }

        public static void StopSearch(int index1, int index2)
        {
            bool completed = false;
            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (Global.normalQueue[i] == index1)
                {
                    Global.normalQueue[i] = 0;
                    if(completed)
                        break;
                    completed = true;
                }
                if (Global.normalQueue[i] == index2)
                {
                    Global.normalQueue[i] = 0;
                    if (completed)
                        break;
                    completed = true;
                }
            }
        }

    }
}
