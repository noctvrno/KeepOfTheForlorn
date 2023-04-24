using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace KOTF.Core.Input
{
    public enum ActionType
    {
        Idle,
        Movement,
        Sprint,
        Attack
    }

    public static class InputFactory
    {
        private static readonly Dictionary<ActionType, InputHandler> _inputTypeToHandler = new();

        public static void Create()
        {
            foreach (ActionType actionType in Enum.GetValues(typeof(ActionType)))
            {
                var inputHandler = ScriptableObject.CreateInstance<InputHandler>();
                inputHandler.Input = new InputAction();
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
                        inputHandler.Input.AddBinding($"{GetMouseBinding("leftButton")}")
                            .WithInteraction($"press(behavior={(int)PressBehavior.PressOnly})");
                        break;
                    case ActionType.Sprint:
                        inputHandler.Input.AddBinding($"{GetKeyboardBinding("leftshift")}")
                            .WithInteraction($"press(behavior={(int)PressBehavior.ReleaseOnly})");
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
