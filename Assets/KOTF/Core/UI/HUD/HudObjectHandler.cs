using System;
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
        private StaminaBar _staminaBar;

        public void Initialize()
        {
            _player = FindObjectOfType<MainPlayerCharacter>();
            _healthBar = FindObjectOfType<HealthBar>();
            _staminaBar = FindObjectOfType<StaminaBar>();

            _healthBar.UpdateHudBar(_player.HealthAttribute);
            _staminaBar.UpdateHudBar(_player.StaminaAttribute);

            RegisterUpdates();
        }

        private void RegisterUpdates()
        {
            _player.HealthAttribute.ValueChangedEventHandler += _healthBar.OnValueChanged;
            _player.StaminaAttribute.ValueChangedEventHandler += _staminaBar.OnValueChanged;
        }

        private void OnDisable()
        {
            _player.HealthAttribute.ValueChangedEventHandler -= _healthBar.OnValueChanged;
            _player.StaminaAttribute.ValueChangedEventHandler -= _staminaBar.OnValueChanged;
        }
    }
}
