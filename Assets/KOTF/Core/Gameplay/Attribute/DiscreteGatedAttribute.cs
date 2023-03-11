using System;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public class DiscreteGatedAttribute<T> : IGatedAttribute<T>
        where T : IComparable
    {
        [SerializeField] private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                ValueChangedEventHandler?.Invoke(this, EventArgs.Empty);
            }
        }

        [SerializeField] private T _minimumValue;
        public T MinimumValue
        {
            get => _minimumValue;
            set => _minimumValue = value;
        }

        [SerializeField] private T _maximumValue;
        public T MaximumValue
        {
            get => _maximumValue;
            set => _maximumValue = value;
        }

        public EventHandler ValueChangedEventHandler { get; set; }
    }
}
