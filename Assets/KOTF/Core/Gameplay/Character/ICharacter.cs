using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public interface ICharacter
    {
        public void Move();
        public void Attack();
    }
}
