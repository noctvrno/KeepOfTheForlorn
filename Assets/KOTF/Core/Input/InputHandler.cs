using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KOTF.Core.Input
{
    public class InputHandler : ScriptableObject
    {
        public InputAction Input { get; set; }

        public InputHandler()
        {
            Input = new InputAction();
        }

        public void OnEnable()
        {
            Input.Enable();
        }

        private void OnDisable()
        {
            Input.Disable();
        }
    }
}
