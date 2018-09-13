using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bindings;

namespace ElecyServer
{
    public class RoomPlayer : BaseRoomObject
    {

        #region Constructor

        public RoomPlayer(int index, ObjectType type, int hp, float[] position, float[] rotation) : base(index, type, hp, position, rotation)
        {
        }

        public override void TakeDamage(int damage)
        {
            
        }

        public override void UpdateHP()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
