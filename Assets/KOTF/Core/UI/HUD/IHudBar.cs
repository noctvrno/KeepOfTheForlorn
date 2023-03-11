using System;
using UnityEngine.UI;

namespace KOTF.Core.UI.HUD
{
    public interface IHudBar
    {
        Slider Slider { get; }
        void OnValueChanged(object sender, EventArgs e);
    }
}
