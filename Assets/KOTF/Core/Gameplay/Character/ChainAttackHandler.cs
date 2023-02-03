using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Services;
using KOTF.Utils.General;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public class ChainAttackHandler
    {
        private readonly AnimationService _animationService;
        private readonly int _frames;
        public bool Chainable { get; private set; }

        public ChainAttackHandler(AnimationService animationService, int frames)
        {
            _frames = frames;
            _animationService = animationService;
        }

        public IEnumerator RegisterChainPossibilityCoroutine()
        {
            Debug.Log($"Waiting at: {Time.frameCount}");
            Chainable = true;
            yield return new WaitForFrame(_frames);
            Debug.Log($"Done waiting at: {Time.frameCount}");
            Chainable = false;
        }

        public void Chain(Animator animator)
        {

        }
    }
}
