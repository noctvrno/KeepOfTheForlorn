using System.Collections.Generic;

namespace KOTF.Core.Services
{
    public class CharacterColliderService : IService
    {
        private HashSet<int> _registeredCollisionIds = new();

        public void RegisterCollision(int blockedCollisionId)
        {
            _registeredCollisionIds.Add(blockedCollisionId);
        }

        public bool IsRegistered(int collisionId)
        {
            return _registeredCollisionIds.Contains(collisionId);
        }

        public void Reset()
        {
            _registeredCollisionIds.Clear();
        }
    }
}
