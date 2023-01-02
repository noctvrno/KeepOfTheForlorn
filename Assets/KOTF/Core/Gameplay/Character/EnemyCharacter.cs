using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Equipment;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public class EnemyCharacter : CharacterBase
    {
        #region Serializable fields
        [SerializeField] private float _movementSpeed;
        #endregion

        #region Object references
        private MainPlayerCharacter _mainPlayerCharacter;
        #endregion

        private void Start()
        {
            _mainPlayerCharacter = FindObjectOfType<MainPlayerCharacter>();
            if (_mainPlayerCharacter == null)
                Debug.LogError($"Couldn't find object of type {nameof(MainPlayerCharacter)}");
        }

        public override void Move()
        {
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = _mainPlayerCharacter.transform.position;
            Vector3 distanceToPlayer = currentPosition - targetPosition;

            // Update angular and linear position properties.
            transform.rotation = Quaternion.LookRotation(distanceToPlayer);
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, _movementSpeed);
        }

        public override void Attack()
        {

        }
    }
}
