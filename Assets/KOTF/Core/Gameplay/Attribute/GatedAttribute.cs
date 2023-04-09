using System;
using KOTF.Utils.Extensions;
using UnityEngine;

namespace KOTF.Core.Gameplay.Attribute
{
    [Serializable]
    public class GatedAttribute<T> : IAttribute<T>
        where T : IComparable
    {
        [SerializeField] private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value.Clamp(MinimumValue, MaximumValue);
                ValueChangedEventHandler?.Invoke(this, EventArgs.Empty);
            }
        }

        [field: SerializeField]
        public T MinimumValue { get; set; }

        [field: SerializeField]
        public T MaximumValue { get; set; }

        public EventHandler ValueChangedEventHandler { get; set; }
    }
}
