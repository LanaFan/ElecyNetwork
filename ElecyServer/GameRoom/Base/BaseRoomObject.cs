using Bindings;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ElecyServer
{

    public abstract class BaseRoomObject
    {
        public int index;
        public ObjectType type;
        public bool isDestroyed;

        public BaseUpdate<float[]> position;
        public BaseUpdate<float[]> rotation;
        public BaseUpdate<int> healthPoints;

        public int maxHp;
        public int curHp;

        protected object expectant;

        public BaseRoomObject(int index, ObjectType type, int hp, float[] position, float[] rotation)
        {
            this.index = index;
            this.type = type;
            isDestroyed = false;
            this.position = new BaseUpdate<float[]>(position);
            this.rotation = new BaseUpdate<float[]>(rotation);
            this.healthPoints = new BaseUpdate<int>(hp);
            expectant = new object();
        }

        #region HP update

        public abstract void UpdateHP();

        public abstract void TakeDamage(int damage);

        #endregion
    }

    #region Enumerator

    public class BaseRoomObjectEnum : IEnumerator
    {
        private BaseRoomObject[] _objects;

        int position = -1;

        public BaseRoomObjectEnum(BaseRoomObject[] list)
        {
            _objects = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _objects.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public BaseRoomObject Current
        {
            get
            {
                try
                {
                    return _objects[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    #endregion

}
