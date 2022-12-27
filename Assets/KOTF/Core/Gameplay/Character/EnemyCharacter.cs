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
        public void Move()
        {
            throw new NotImplementedException();
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        private void OnCollisionEnter(Collision collision)
        {
            Weapon attackerWeapon = collision.gameObject.GetComponent<Weapon>();
            if (attackerWeapon == null)
                return;

            Debug.Log($"Enemy character has been hit by {collision.gameObject.name}");
        }
    }
}
