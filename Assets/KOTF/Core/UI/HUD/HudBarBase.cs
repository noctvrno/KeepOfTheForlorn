using System;
using KOTF.Core.Gameplay.Attribute;
using KOTF.Core.Wrappers;
using TMPro;
using UnityEngine.UI;

namespace KOTF.Core.UI.HUD
{
    public abstract class HudBarBase : KotfGameObject
    {
        public Slider Slider { get; set; }
        public TextMeshProUGUI Label { get; set; }

        private void Awake()
        {
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
