using Bindings;

namespace ElecyServer
{
    public class StaticObject : BaseRoomObject
    {
        int currHP;
        float[] rotation;
        StaticTypes staticType;

        #region Constructor

        public StaticObject(int index, StaticTypes staticType, ObjectType type, int hp, float[] position, float[] rotation) : base(index, type, hp, position, rotation)
        {
            currHP = hp;
            this.staticType = staticType;
            this.rotation = SetRotation(staticType, rotation);
        }

        #endregion

        public (int, float[], float[]) GetInfo()
        {
            return (currHP, curPosition, rotation);
        }

        public override void TakeDamage(int damage)
        {
            throw new System.NotImplementedException();
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
