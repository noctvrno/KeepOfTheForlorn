using System;
using System.Collections;
using KOTF.Core.Services;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public class AttributeDiminisher : AttributeModifierBase
    {
        public override void Modify()
        {
            if (UpdateRate == 0.0f)
            {
                Attribute.Value -= ValuePerUpdate;
                return;
            }

            // Modify based on UpdateRate.
        }
    }

    [Serializable]
    public class AttributeEnhancer : AttributeModifierBase
    {
        public override void Modify()
        {
            if (UpdateRate == 0.0f)
            {
                Attribute.Value += ValuePerUpdate;
                return;
            }

            // Modify based on UpdateRate.
            CoroutineService.Start(Update());
        }

        private IEnumerator Update()
        {
            while (Attribute.Value < Threshold)
            {
                yield return new WaitForSeconds(UpdateRate);
                Attribute.Value += ValuePerUpdate;
            }
        }
    }

    [Serializable]
    public class AttributeModifier
    {
        [field: SerializeField]
        public AttributeDiminisher AttributeDiminisher { get; set; }

        [field: SerializeField]
        public AttributeEnhancer AttributeEnhancer { get; set; }

        public void Diminish()
        {
            AttributeDiminisher.Modify();
        }

        public void Enhance()
        {
            AttributeEnhancer.Modify();
        }
    }
}
