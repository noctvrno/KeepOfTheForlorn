using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Utils.General;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public class ChainAttackHandler
    {
        private readonly int _frames;

        public ChainAttackHandler(int frames)
        {
            _frames = frames;
        }

        public IEnumerator ChainCoroutine()
        {
            Debug.Log($"Waiting at: {Time.frameCount}");
            yield return new WaitForFrame(_frames);
            Debug.Log($"Done waiting at: {Time.frameCount}");
        }
    }
}
