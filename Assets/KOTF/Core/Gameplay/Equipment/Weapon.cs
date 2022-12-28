using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Character;
using UnityEngine;

namespace KOTF.Core.Gameplay.Equipment
{
    public class Weapon : MonoBehaviour, IEquipment
    {
        private Collider _collider;
        public Collider Collider => _collider;
        public string Name { get; set; }
        public string Description { get; set; }
        public ICharacter Owner { get; set; }

        private void Start()
        {
            if (!TryGetComponent(out _collider))
                Debug.LogError("The collider of the weapon has not been placed.");
        }

        private void OnCollisionEnter(Collision collision)
        {
            ICharacter victim = collision.gameObject.GetComponent<ICharacter>();
            if (victim == null)
                return;

            Debug.Log($"Hit {collision.gameObject.name}");
        }
    }
}
