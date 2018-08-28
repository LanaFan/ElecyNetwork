using Bindings;

namespace ElecyServer
{
    public class TestRoom : BaseGameRoom
    {

        #region Constructor

        public TestRoom(ClientTCP client, int mapIndex) : base(client, 1, mapIndex) { }

        protected internal override void RoomType()
        {
            this.roomType = RoomTypes.TestRoom;
        }

        #endregion

    }
}
