namespace KOTF.Core.Gameplay.Character
{
    public interface IChainCapable : IAggressive
    {
        void OnExitChainPossibility();
        ChainAttackHandler ChainAttackHandler { get; }
    }
}
