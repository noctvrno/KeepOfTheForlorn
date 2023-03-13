using KOTF.Core.Gameplay.Character;
using UnityEngine;

namespace KOTF.Core.UI.HUD
{
    /// <summary>
    /// Acts as a bridge between the player and the visual HUD objects.
    /// </summary>
    public class HudObjectHandler : ScriptableObject
    {
        private MainPlayerCharacter _player;
        private HealthBar _healthBar;

        public void Initialize()
        {
            _player = FindObjectOfType<MainPlayerCharacter>();
            _healthBar = FindObjectOfType<HealthBar>();
            _healthBar.UpdateHudBar(_player.HealthAttribute);

            RegisterUpdates();
        }

        public void RegisterUpdates()
        {
            _player.HealthAttribute.ValueChangedEventHandler += _healthBar.OnValueChanged;
        }
    }
}
