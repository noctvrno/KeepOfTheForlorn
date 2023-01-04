using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Equipment;
using KOTF.Core.Wrappers;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public abstract class CharacterBase : KotfGameObject
    {
        [Header("Stats")]
        [SerializeField] private int _healthPoints = 1000;
        public int HealthPoints
        {
            get => _healthPoints;
            set => _healthPoints = value;
        }

        protected virtual void Update()
        {
            Move();
        }

        public Weapon WieldedWeapon { get; set; }
        public abstract void Move();
        public abstract void Attack();

        public virtual void Die()
        {
            Debug.Log($"{gameObject.name} has died.");
            Destroy(gameObject);
        }
    }
}
