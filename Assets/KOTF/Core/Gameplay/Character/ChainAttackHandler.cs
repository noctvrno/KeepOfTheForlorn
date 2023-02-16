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
        private readonly int _frames;
        private int _chainIndex;
        public bool Chainable { get; private set; } = true;

        public ChainAttackHandler(AnimationService animationService, int frames)
        {
            _frames = frames;
            _animationService = animationService;
        }

        public IEnumerator RegisterChainPossibilityCoroutine()
        {
            BeforeChain();
            yield return new WaitForFrame(_frames);
            AfterChain();
        }

        private void BeforeChain()
        {
            Debug.Log($"Waiting at: {Time.frameCount}");
            Chainable = true;
        }

        private void AfterChain()
        {
            Debug.Log($"Done waiting at: {Time.frameCount}");
            if (!Chainable)
                return;

            ResetChain();
        }

        public void Chain()
        {
            Debug.Log($"Chain Index: {_chainIndex}");
            _animationService.TriggerAnimation(ActionType.Attack, _chainIndex++);
            Chainable = false;
        }

        public void ResetChain()
        {
            _animationService.TriggerAnimation(ActionType.Idle);
            _chainIndex = 0;
            Chainable = true;
        }
    }
}
