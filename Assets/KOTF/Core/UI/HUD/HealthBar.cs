using System;
using KOTF.Core.Gameplay.Attribute;
using KOTF.Core.Wrappers;
using TMPro;
using UnityEngine.UI;

namespace KOTF.Core.UI.HUD
{
    public class HealthBar : KotfGameObject, IHudBar
    {
        public Slider Slider { get; private set; }
        public TextMeshProUGUI Label { get; private set; }

        private void Awake()
        {
            // We are getting these components on Awake because the HUD is initialized on MainPlayerCharacter, as it is directly tied to it.
            Slider = GetComponent<Slider>();
            Label = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void OnValueChanged(object sender, EventArgs e)
        {
            if (sender is not GatedAttribute<float> healthAttribute)
                return;

            UpdateHudBar(healthAttribute);
        }

        public void UpdateHudBar(GatedAttribute<float> healthAttribute)
        {
            UpdateValues(healthAttribute);
            UpdateLabel(healthAttribute);
        }

        private void UpdateValues(GatedAttribute<float> healthAttribute)
        {
            Slider.minValue = healthAttribute.MinimumValue;
            Slider.maxValue = healthAttribute.MaximumValue;
            Slider.value = healthAttribute.Value;
        }

        private void UpdateLabel(GatedAttribute<float> healthAttribute)
        {
            Label.text = $"{healthAttribute.Value}/{healthAttribute.MaximumValue}";
        }
    }
}
