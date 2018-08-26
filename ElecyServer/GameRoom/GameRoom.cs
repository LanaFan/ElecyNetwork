using System;
using System.Collections.Generic;
using System.Threading;
using Bindings;

namespace ElecyServer
{
    public class GameRoom : BaseGameRoom
    {

        #region Constructor

        public GameRoom(ClientTCP client) : base(client, 2) { }

        protected internal override void RoomType()
        {
            this.roomType = RoomTypes.GameRoom;
        }

        #endregion

    }

}
