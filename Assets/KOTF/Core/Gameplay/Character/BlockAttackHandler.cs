using KOTF.Core.Input;
using KOTF.Core.Services;

namespace KOTF.Core.Gameplay.Character
{
    public class BlockAttackHandler
    {
        private readonly AttributeUpdaterService _attributeUpdaterService;
        private readonly CharacterBase _character;

        public bool IsWithinParryWindow { get; private set; }

        public BlockAttackHandler(AttributeUpdaterService attributeUpdaterService, CharacterBase character)
        {
            _attributeUpdaterService = attributeUpdaterService;
            _character = character;
        }

        public void Block()
        {
            RegisterParryPossibility();
            _character.CharacterAnimationHandler.TriggerAnimation(ActionType.Block);
            _attributeUpdaterService.Enable(_character.WieldedWeapon.DamageReductionAttribute);
        }

        public void Release()
        {
            ResetParryPossibility();
            _character.CharacterAnimationHandler.TriggerAnimation(ActionType.Idle);
            _attributeUpdaterService.Disable(_character.WieldedWeapon.DamageReductionAttribute);
        }

        public void RegisterParryPossibility()
        {
            IsWithinParryWindow = true;
        }

        public void ResetParryPossibility()
        {
            IsWithinParryWindow = false;
        }
    }
}
