using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Equipment;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public class EnemyCharacter : MonoBehaviour, ICharacter
    {
        public Weapon WieldedWeapon { get; set; }

        public void Move()
        {
            throw new NotImplementedException();
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }
    }
}
