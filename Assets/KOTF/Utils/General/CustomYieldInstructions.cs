using UnityEngine;

namespace KOTF.Utils.General
{
    public class WaitForFrame : CustomYieldInstruction
    {
        private readonly int _targetFrameCount;

        public WaitForFrame(int frameNumber)
        {
            _targetFrameCount = Time.frameCount + frameNumber;
        }

        public override bool keepWaiting => Time.frameCount < _targetFrameCount;
    }
}