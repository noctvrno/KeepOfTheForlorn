using System.Collections.Generic;
using KOTF.Core.Gameplay.Character;
using KOTF.Core.Gameplay.Equipment;
using UnityEngine;

namespace KOTF.Core.Services
{
    public class CharacterColliderService : IService
    {
        private readonly HashSet<int> _registeredCollisionIds = new();

        public void RegisterCollisionBetween(Weapon aggressorWeapon, CharacterBase victim)
        {
            int victimId = victim.GetInstanceID();
            if (IsRegistered(victimId))
                return;

            _registeredCollisionIds.Add(victimId);

            victim.HealthAttribute.Value -= aggressorWeapon.BaseDamage;
            Debug.Log($"{aggressorWeapon.Owner.name} inflicted {aggressorWeapon.BaseDamage} base damage upon {victim.gameObject.name}.");
            if (victim.HealthAttribute.Value.Equals(victim.HealthAttribute.MinimumValue))
                victim.Die();
        }

        private bool IsRegistered(int collisionId)
        {
            return _registeredCollisionIds.Contains(collisionId);
        }

        public void Reset()
        {
            _registeredCollisionIds.Clear();
        }
    }
}
