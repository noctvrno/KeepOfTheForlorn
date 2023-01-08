using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KOTF.Core.Wrappers
{
    public class KotfGameObject : MonoBehaviour
    {
        public new T GetComponent<T>()
        {
            return ValidateComponent(base.GetComponent<T>());
        }

        public new T GetComponentInChildren<T>()
        {
            return ValidateComponent(base.GetComponentInChildren<T>());
        }

        private T ValidateComponent<T>(T component)
        {
            if (component == null)
                throw new ArgumentException($"Could not get {typeof(T).Name} from {name}.");

            return component;
        }

        public new T FindObjectOfType<T>()
            where T : Object
        {
            T @object = Object.FindObjectOfType<T>();
            if (@object == null)
                throw new ArgumentException($"Couldn't find object of type {typeof(T).Name} requested by {name}.");

            return @object;
        }
    }
}
