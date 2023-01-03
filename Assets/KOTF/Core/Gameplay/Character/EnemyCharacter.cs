using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;

namespace KOTF.Core.Gameplay.Character
{
    public class EnemyCharacter : CharacterBase
    {
        #region Object references
        private MainPlayerCharacter _mainPlayerCharacter;
        private NavMeshAgent _navMeshAgent;
        #endregion

        private void Start()
        {
            _mainPlayerCharacter = FindObjectOfType<MainPlayerCharacter>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }

        public override void Move()
        {
            _navMeshAgent.SetDestination(_mainPlayerCharacter.transform.position);
        }

        public override void Attack()
        {

        }
    }
}
