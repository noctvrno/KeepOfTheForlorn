using KOTF.Utils.StringConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace KOTF.Core.Gameplay.Character
{
    public class EnemyCharacter : CharacterBase
    {
        #region Serializable fields
        [SerializeField] private float _aggroRadius;
        #endregion

        #region Object references
        private MainPlayerCharacter _mainPlayerCharacter;
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        #endregion

        private void Start()
        {
            _mainPlayerCharacter = FindObjectOfType<MainPlayerCharacter>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        public override void Move()
        {
            Vector3 targetPosition = _mainPlayerCharacter.transform.position;

            // If the target gets inside the aggroRadius, then chase endlessly.
            if (Vector3.Distance(targetPosition, transform.position) <= _aggroRadius)
                _navMeshAgent.SetDestination(_mainPlayerCharacter.transform.position);
        }

        public override void Attack()
        {
            _animator.SetBool(AnimationConstants.ATTACK, true);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _aggroRadius);
        }

        public void OnEnterAttackWindow()
        {

        }

        public void OnExitAttackWindow()
        {

        }
    }
}
