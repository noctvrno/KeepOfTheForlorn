using KOTF.Core.Gameplay.Equipment;

namespace KOTF.Core.Gameplay.Character
{
    public interface IAggressive
    {
        Weapon WieldedWeapon { get; }
        void OnEnterAttackWindow();
        void OnExitAttackWindow();
        void OnExitAttackAnimation();
    }
}
