using System;
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
        private Collider _collider;
        private CharacterColliderService _characterColliderService;
        public Collider Collider => _collider;
        public string Name { get; set; }
        public string Description { get; set; }
        public ICharacter Owner { get; set; }

        private void Start()
        {
            if (!TryGetComponent(out _collider))
                Debug.LogError("The collider of the weapon has not been placed.");

            var serviceProvider = ServiceProvider.GetInstance();
            _characterColliderService = serviceProvider.Get<CharacterColliderService>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.TryGetComponent(out ICharacter _))
                return;

            int collisionInstanceId = collision.gameObject.GetInstanceID();
            if (_characterColliderService.IsRegistered(collisionInstanceId))
                return;

            _characterColliderService.RegisterCollision(collisionInstanceId);
            Debug.Log($"Hit {collision.gameObject.name}");
        }
    }
}
