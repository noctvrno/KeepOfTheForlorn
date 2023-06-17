using KOTF.Core.Input;
using KOTF.Core.Services;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public class BlockAttackHandler
    {
        private readonly AttributeUpdaterService _attributeUpdaterService;
        private readonly CharacterBase _character;

        public BlockAttackHandler(AttributeUpdaterService attributeUpdaterService, CharacterBase character)
        {
            _attributeUpdaterService = attributeUpdaterService;
            _character = character;
        }

        public void Block()
        {
            _character.CharacterAnimationHandler.TriggerAnimation(ActionType.Block);
            _attributeUpdaterService.Enable(_character.WieldedWeapon.DamageReductionAttribute);
        }

        public void Release()
        {
            _character.CharacterAnimationHandler.TriggerAnimation(ActionType.Idle);
            _attributeUpdaterService.Disable(_character.WieldedWeapon.DamageReductionAttribute);
        }
    }
}
