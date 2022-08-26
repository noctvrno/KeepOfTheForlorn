using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KOTF.Core.Input
{
    public enum InputActionType
    {
        Movement,
        Attack
    }

    public static class InputFactory
    {
        private static Dictionary<InputActionType, InputHandler> _inputTypeToHandler = new();

        public static void Create()
        {
            foreach (InputActionType actionType in Enum.GetValues(typeof(InputActionType)))
            {
                InputHandler inputHandler = ScriptableObject.CreateInstance<InputHandler>();
                inputHandler.Input = new();
                switch (actionType)
                {
                    case InputActionType.Movement:
                        inputHandler.Input.AddCompositeBinding("3DVector")
                            .With("Forward", GetKeyboardBinding("w"))
                            .With("Backward", GetKeyboardBinding("s"))
                            .With("Left", GetKeyboardBinding("a"))
                            .With("Right", GetKeyboardBinding("d"));
                        break;
                    case InputActionType.Attack:
                        inputHandler.Input.AddBinding($"{GetMouseBinding("leftButton")}");
                        break;
                }

                inputHandler.OnEnable();
                _inputTypeToHandler.Add(actionType, inputHandler);
            }
        }

        private static String GetKeyboardBinding(String key)
        {
            return $"<Keyboard>/{key}";
        }

        private static String GetMouseBinding(String key)
        {
            return $"<Mouse>/{key}";
        }

        public static InputHandler GetInput(InputActionType actionType)
        {
            if (!_inputTypeToHandler.TryGetValue(actionType, out var inputHandler))
                return null;

            return inputHandler;
        }
    }
}
