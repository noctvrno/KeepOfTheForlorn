using UnityEngine;
using Object = UnityEngine.Object;

namespace KOTF.Core.Wrappers
{
    public class KotfGameObject : MonoBehaviour
    {
        public new T GetComponent<T>()
        {
            T component = base.GetComponent<T>();
            if (component == null)
                Debug.LogError($"Could not get {nameof(T)} from {name}.");

            return component;
        }

        public new T FindObjectOfType<T>()
            where T : Object
        {
            T @object = Object.FindObjectOfType<T>();
            if (@object == null)
                Debug.LogError($"Couldn't find object of type {nameof(T)} requested by {name}.");

            return @object;
        }
    }
}
