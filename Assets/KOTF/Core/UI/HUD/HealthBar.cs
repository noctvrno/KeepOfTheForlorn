using System;
using KOTF.Core.Gameplay.Attribute;
using KOTF.Core.Wrappers;
using UnityEngine.UI;

namespace KOTF.Core.UI.HUD
{
    public class HealthBar : KotfGameObject, IHudBar
    {
        public Slider Slider { get; private set; }

        private void Awake()
        {
            // We are getting this component on Awake because the HUD is initialized on MainPlayerCharacter, as it is directly tied to it.
            Slider = GetComponent<Slider>();
        }

        public void UpdateValues(IGatedAttribute<float> target)
        {
            Slider.minValue = target.MinimumValue;
            Slider.maxValue = target.MaximumValue;
            Slider.value = target.Value;
        }

        public void OnValueChanged(object sender, EventArgs e)
        {
            if (sender is not IGatedAttribute<float> healthAttribute)
                return;

            UpdateValues(healthAttribute);
        }
    }
}
