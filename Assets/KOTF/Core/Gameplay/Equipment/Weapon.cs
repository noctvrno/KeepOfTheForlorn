﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Character;
using KOTF.Core.Services;
using UnityEngine;

namespace KOTF.Core.Gameplay.Equipment
{
    public class Weapon : MonoBehaviour, IEquipment
    {
        private CharacterColliderService _characterColliderService;
        private Collider _collider;
        public Collider Collider => _collider;
        public string Name { get; set; }
        public string Description { get; set; }
        public CharacterBase Owner { get; set; }

        [SerializeField]
        private int _baseDamage = 500;
        public int BaseDamage => _baseDamage;

        private void Start()
        {
            if (!TryGetComponent(out _collider))
                Debug.LogError("The collider of the weapon has not been placed.");

            var serviceProvider = ServiceProvider.GetInstance();
            _characterColliderService = serviceProvider.Get<CharacterColliderService>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent(out CharacterBase victimCharacter))
                return;
            
            _characterColliderService.RegisterCollisionBetween(this, victimCharacter);
        }
    }
}
