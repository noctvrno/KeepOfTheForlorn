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

        public InputHandler WithStartedCallback(Action<InputAction.CallbackContext> contextAction)
        {
            Input.started += contextAction;
            return this;
        }

        public InputHandler WithPerformedCallback(Action<InputAction.CallbackContext> contextAction)
        {
            Input.performed += contextAction;
            return this;
        }

        public InputHandler WithCancelledCallback(Action<InputAction.CallbackContext> contextAction)
        {
            Input.canceled += contextAction;
            return this;
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
