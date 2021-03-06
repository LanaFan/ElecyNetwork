﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bindings;

namespace ElecyServer
{
    public class RoomPlayer : BaseRoomObject
    {
        public BaseUpdate<int> synergyPoints;

        #region Constructor

        public RoomPlayer(int index, ObjectType type, int hp, int synergy, float[] position, float[] rotation) : base(index, type, hp, position, rotation)
        {
            this.synergyPoints = new BaseUpdate<int>(synergy);
        }

        public override void Destroy(BaseGameRoom room)
        {
            throw new NotImplementedException();
        }

        public override void TakeDamage(ClientTCP client, int index, int PhysicDamage, int IgnisDamage, int TerraDamage, int AquaDamage, int CaeliDamage, int PureDamage, bool Heal)
        {
            SendDataTCP.SendDamage(client, index, type, index, PhysicDamage, IgnisDamage, TerraDamage, CaeliDamage, AquaDamage, PureDamage, Heal);
        }

        public override void UpdateHP()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
