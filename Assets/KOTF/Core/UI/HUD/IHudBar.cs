using System;
using TMPro;
using UnityEngine.UI;

namespace KOTF.Core.UI.HUD
{
    public interface IHudBar
    {
        Slider Slider { get; }
        TextMeshProUGUI Label { get; }
        void OnValueChanged(object sender, EventArgs e);
    }
}
