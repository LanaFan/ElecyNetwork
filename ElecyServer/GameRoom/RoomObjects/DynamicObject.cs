using Bindings;

namespace ElecyServer
{
    public class DynamicObject : BaseRoomObject
    {
        int caster;

        public DynamicObject(int index, ObjectType type, int hp, float[] position, float[] rotation, int caster) : base(index, type, hp, position, rotation)
        {
            this.caster = caster;
        }

        public override void Destroy(BaseGameRoom room)
        {
            SendDataTCP.SendDestroy(room, index, type);
            room.dynamicObjectsList.Destroy(index);
        }

        public override void TakeDamage(ClientTCP client, int index, int PhysicDamage, int IgnisDamage, int TerraDamage, int AquaDamage, int CaeliDamage, int PureDamage, bool Heal)
        {
            SendDataTCP.SendDamage(client, caster, type, index, PhysicDamage, IgnisDamage, TerraDamage, CaeliDamage, AquaDamage, PureDamage, Heal);
        }

        public override void UpdateHP()
        {
            throw new System.NotImplementedException();
        }
    }
}
