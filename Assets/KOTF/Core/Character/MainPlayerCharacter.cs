using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KOTF.Utils.Extensions;
using KOTF.Core.Input;

namespace KOTF.Core.Character
{
    public class MainPlayerCharacter : MonoBehaviour, ICharacter
    {
        [Header("Movement")]
        [SerializeField] private float _movementSpeed = 10.0f;
        private InputHandler _movementInput = null;

        private void Awake()
        {
            InputFactory.Create();

            _movementInput = InputFactory.GetInput(Input.InputActionType.Movement);
            if (_movementInput == null)
                Debug.LogError($"{_movementInput} is null which is not permitted!");
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            Move();
        }

        public void Move()
        {
            // Read Input using new Input System.
            float longitudinalValue = _movementInput.Input.ReadValue<Vector3>().z;
            float lateralValue = _movementInput.Input.ReadValue<Vector3>().x;

            // Log Input.
            Debug.Log($"Longitudinal value: {longitudinalValue}");
            Debug.Log($"Lateral value: {lateralValue}");

            // Apply corresponding forces.
            gameObject.transform.Translate(ComputeMovementVector(longitudinalValue, lateralValue));
        }

        private Vector3 ComputeMovementVector(float longitudinalValue, float lateralValue)
        {
            return (lateralValue * Vector3.right + longitudinalValue * Vector3.forward).ToDeltaTime() * _movementSpeed;
        }
    }
}
