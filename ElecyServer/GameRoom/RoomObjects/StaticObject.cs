using Bindings;

namespace ElecyServer
{
    public class StaticObject : BaseRoomObject
    {
        int currHP;
        int maxHP;
        StaticTypes staticType;

        #region Constructor

        public StaticObject(int index, StaticTypes staticType, ObjectType type, int hp, float[] position, float[] rotation) : base(index, type, hp, position, rotation)
        {
            maxHP = currHP = hp;
            this.staticType = staticType;
            this.rotation = SetRotation(staticType, rotation);
        }

        public override void Destroy(BaseGameRoom room)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public (int, float[], float[]) GetInfo()
        {
            this.position.GetValue(out UpdateContainer<float[]> value, out int index);
            return (currHP, value.value, rotation);
        }

        public override void TakeDamage(ClientTCP Client, int index, int PhysicDamage, int IgnisDamage, int TerraDamage, int AquaDamage, int CaeliDamage, int PureDamage, bool Heal)
        {
            if(Heal)
            {
                currHP += PhysicDamage + IgnisDamage + TerraDamage + CaeliDamage + AquaDamage + PureDamage;
                if (currHP > maxHp)
                    currHP = maxHp;
            }
            else
            {
                currHP -= PhysicDamage + IgnisDamage + TerraDamage + CaeliDamage + AquaDamage + PureDamage;
            }
        }

        public override void UpdateHP()
        {
            throw new System.NotImplementedException();
        }


        #region Protected Helpers

        protected float[] SetRotation(StaticTypes type, float[] rot)
        {
            switch (type)
            {
                case StaticTypes.tree:
                    return new float[] { 0, rot[1], 0, 1 };
                case StaticTypes.rock:
                    return new float[] { rot[0], rot[1], rot[2], 1 };
                default:
                    return rot;
            }
        }

        #endregion

    }
}
