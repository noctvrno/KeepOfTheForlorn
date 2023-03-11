using System;

namespace KOTF.Core.Gameplay.Attribute
{
    public interface IGatedAttribute<T> : IAttribute<T>
        where T : IComparable
    {
        T MinimumValue { get; set; }
        T MaximumValue { get; set; }
    }

    public interface IAttribute<T>
        where T : IComparable
    {
        T Value { get; set; }
        EventHandler ValueChangedEventHandler { get; }
    }
}
