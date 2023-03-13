using System;

namespace KOTF.Core.Gameplay.Attribute
{
    public interface IAttribute<T>
        where T : IComparable
    {
        T Value { get; set; }
        EventHandler ValueChangedEventHandler { get; }
    }
}
