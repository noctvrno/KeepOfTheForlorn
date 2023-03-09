using System;
using Unity.VisualScripting;

namespace KOTF.Core.Gameplay.Attribute
{
    public interface IGatedAttribute<T> : IAttribute<T>
        where T : IComparable
    {
        T MinimumValue { get; }
        T MaximumValue { get; }
    }

    public interface IAttribute<T>
        where T : IComparable
    {
        T Value { get; }

        void Update(T target);
    }
}
