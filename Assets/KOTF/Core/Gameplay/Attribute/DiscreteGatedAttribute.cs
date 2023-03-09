using System;

namespace KOTF.Core.Gameplay.Attribute
{
    public class DiscreteGatedAttribute<T> : IGatedAttribute<T>
        where T : IComparable
    {
        public T Value { get; private set; }
        public T MinimumValue { get; }
        public T MaximumValue { get; }

        public DiscreteGatedAttribute(T value, T minimumValue, T maximumValue)
        {
            Value = value;
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
        }

        public void Update(T target)
        {
            Value = target;
        }
    }
}
