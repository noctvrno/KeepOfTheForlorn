using KOTF.Core.Gameplay.Equipment;

namespace KOTF.Core.Gameplay.Character
{
    public interface ICharacter
    {
        Weapon WieldedWeapon { get; set; }
        public void Move();
        public void Attack();
    }
}
