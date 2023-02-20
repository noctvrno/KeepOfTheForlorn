using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Input;
using KOTF.Core.Services;
using KOTF.Utils.General;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public class ChainAttackHandler
    {
        private readonly AnimationService _animationService;
        private int _chainIndex;
        private bool _chainable = true;

        public ChainAttackHandler(AnimationService animationService)
        {
            _animationService = animationService;
        }

        public void RegisterChainPossibility()
        {
            _chainable = true;
        }

        public void Chain()
        {
            if (!_chainable)
                return;

            _animationService.TriggerAnimation(ActionType.Attack, _chainIndex++);
            _chainable = false;
        }

        public void ExitChain()
        {
            _animationService.TriggerAnimation(ActionType.Idle);
            _chainable = false;
        }

        public void ResetChain()
        {
            _chainIndex = 0;
            _chainable = true;
        }
    }
}
