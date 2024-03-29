﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Attribute;
using KOTF.Core.Gameplay.Character;
using KOTF.Core.Services;
using KOTF.Core.Wrappers;
using UnityEngine;

namespace KOTF.Core.Gameplay.Equipment
{
    public class Weapon : KotfGameObject, IEquipment
    {
        private CharacterColliderService _characterColliderService;
        private Collider _collider;
        private AttributeUpdaterService _attributeUpdaterService;
        public Collider Collider => _collider;
        public string Name { get; set; }
        public string Description { get; set; }
        public CharacterBase Owner { get; set; }

        [field: Header("Stats")]
        [field: SerializeField]
        public float BaseDamage { get; private set; }

        [field: SerializeField]
        public int ChainAttackFrame { get; private set; }

        [field: SerializeField]
        public DiscreteAttributeModifier StaminaDiminisher { get; private set; }

        [field: SerializeField]
        public GatedAttribute<float> DamageReductionAttribute { get; private set; }

        private void Start()
        {
            if (!TryGetComponent(out _collider))
                Debug.LogError("The collider of the weapon has not been placed.");

            Owner = GetComponentInParent<CharacterBase>();

            var serviceProvider = ServiceProvider.GetInstance();
            _characterColliderService = serviceProvider.Get<CharacterColliderService>();
            _attributeUpdaterService = serviceProvider.Get<AttributeUpdaterService>();

            _attributeUpdaterService.Disable(DamageReductionAttribute);

            if (Owner is not MainPlayerCharacter player)
                return;

            StaminaDiminisher.Initialize(player.StaminaAttribute);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent(out CharacterBase victimCharacter))
                return;
            
            _characterColliderService.RegisterCollisionBetween(this, victimCharacter);
        }
    }
}
