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

        public override void TakeDamage(int damage)
        {
            
        }

        public override void UpdateHP()
        {
            throw new System.NotImplementedException();
        }
    }
}
