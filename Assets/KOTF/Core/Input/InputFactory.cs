using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KOTF.Core.Input
{
    public enum ActionType
    {
        Movement,
        Sprint,
        Attack
    }

    public static class InputFactory
    {
        private static Dictionary<ActionType, InputHandler> _inputTypeToHandler = new();

        public static void Create()
        {
            foreach (ActionType actionType in Enum.GetValues(typeof(ActionType)))
            {
                InputHandler inputHandler = ScriptableObject.CreateInstance<InputHandler>();
                inputHandler.Input = new();
                switch (actionType)
                {
                    case ActionType.Movement:
                        inputHandler.Input.AddCompositeBinding("3DVector")
                            .With("Forward", GetKeyboardBinding("w"))
                            .With("Backward", GetKeyboardBinding("s"))
                            .With("Left", GetKeyboardBinding("a"))
                            .With("Right", GetKeyboardBinding("d"));
                        break;
                    case ActionType.Attack:
                        inputHandler.Input.AddBinding($"{GetMouseBinding("leftButton")}");
                        break;
                    case ActionType.Sprint:
                        inputHandler.Input.AddBinding($"{GetKeyboardBinding("leftshift")}");
                        break;
                }

                inputHandler.OnEnable();
                _inputTypeToHandler.Add(actionType, inputHandler);
            }
        }

        private static string GetKeyboardBinding(string key)
        {
            return $"<Keyboard>/{key}";
        }

        private static string GetMouseBinding(string key)
        {
            return $"<Mouse>/{key}";
        }

        public static InputHandler GetInput(ActionType actionType)
        {
            if (!_inputTypeToHandler.TryGetValue(actionType, out var inputHandler))
                throw new ArgumentException($"Could not retrieve action of type {actionType}.");

            return inputHandler;
        }
    }
}
