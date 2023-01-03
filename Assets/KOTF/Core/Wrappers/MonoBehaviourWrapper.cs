﻿using KOTF.Core.Gameplay.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KOTF.Core.Wrappers
{
    public class MonoBehaviourWrapper : MonoBehaviour
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