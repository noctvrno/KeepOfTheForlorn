using KOTF.Core.Input;
using KOTF.Core.Services;

namespace KOTF.Core.Gameplay.Character
{
    public class ChainAttackHandler
    {
        private readonly CharacterAnimationHandler _characterAnimationHandler;
        private int _chainIndex;
        private bool _chainable = true;

        public ChainAttackHandler(CharacterAnimationHandler characterAnimationHandler)
        {
            _characterAnimationHandler = characterAnimationHandler;
        }

        public void RegisterChainPossibility()
        {
            _chainable = true;
        }

        public void Chain()
        {
            if (!_chainable)
                return;

            _characterAnimationHandler.TriggerAnimation(ActionType.Attack, _chainIndex++);
            _chainable = false;
        }

        public void ExitChain()
        {
            _characterAnimationHandler.TriggerAnimation(ActionType.Idle);
            _chainable = false;
        }

        public void ResetChainPossibility()
        {
            _chainIndex = 0;
            _chainable = true;
        }
    }
}
