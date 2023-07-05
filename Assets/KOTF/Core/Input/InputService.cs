using System;
using System.Collections.Generic;
using KOTF.Core.Services;
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
        Attack,
        Block,
        Parry
    }

    public class InputService : IService
    {
        private static readonly Dictionary<ActionType, InputHandler> _inputTypeToHandler = new();

        public InputService()
        {
            Initialize();
        }

        public void Initialize()
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
                    case ActionType.Sprint:
                        inputHandler.Input.AddBinding($"{GetKeyboardBinding("leftshift")}")
                            .WithInteraction(GetPressInteractionString(PressBehavior.ReleaseOnly));
                        break;
                    case ActionType.Attack:
                        inputHandler.Input.AddBinding($"{GetMouseBinding("leftButton")}")
                            .WithInteraction(GetPressInteractionString(PressBehavior.PressOnly));
                        break;
                    case ActionType.Block:
                        inputHandler.Input.AddBinding($"{GetMouseBinding("rightButton")}")
                            .WithInteraction(GetPressInteractionString(PressBehavior.ReleaseOnly));
                        break;
                }

                inputHandler.OnEnable();
                _inputTypeToHandler.Add(actionType, inputHandler);
            }
        }

        private static string GetPressInteractionString(PressBehavior pressBehavior)
        {
            return $"press(behavior={(int)pressBehavior})";
        }

        private static string GetKeyboardBinding(string key)
        {
            return $"<Keyboard>/{key}";
        }

        private static string GetMouseBinding(string key)
        {
            return $"<Mouse>/{key}";
        }

        public InputHandler Get(ActionType actionType)
        {
            if (!_inputTypeToHandler.TryGetValue(actionType, out var inputHandler))
                throw new ArgumentException($"Could not retrieve action of type {actionType}.");

            return inputHandler;
        }
    }
}
