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
            if (IsRegistered(victimId) || IsInvulnerable(aggressorWeapon.Owner, victim))
                return;

            _registeredCollisionIds.Add(victimId);

            float damageValue = ComputeDamage(aggressorWeapon, victim);
            victim.HealthAttribute.Value -= damageValue;
            Debug.Log($"{aggressorWeapon.Owner.name} inflicted {damageValue} damage upon {victim.gameObject.name}.");
            if (victim.HealthAttribute.Value.Equals(victim.HealthAttribute.MinimumValue))
                victim.Die();
        }

        private static bool IsInvulnerable(CharacterBase aggressor, CharacterBase victim)
        {
            if (!victim.BlockAttackHandler.IsWithinParryWindow)
                return false;

            victim.Parry();
            aggressor.Parried();
            Debug.Log($"{victim.name} parried {aggressor.name}.");
            return true;
        }

        private static float ComputeDamage(Weapon aggressorWeapon, IAggressive victim)
        {
            return aggressorWeapon.BaseDamage * (1.0f - victim.WieldedWeapon.DamageReductionAttribute.Value);
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
