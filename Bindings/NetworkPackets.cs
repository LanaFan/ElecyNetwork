

namespace Bindings
{
    //get send from server to client
    public enum ServerPackets
    {
        SConnectionOK = 1,SRegisterOK = 2,SLoginOK = 3,
    }

    //get send from client to server
    public enum ClientPackets
    {
        CConnectcomplite = 1,CRegisterTry = 2,CLoginTry = 3,
    }
}
