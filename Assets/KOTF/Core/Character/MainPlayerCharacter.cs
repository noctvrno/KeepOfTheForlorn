using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using KOTF.Utils.Extensions;

namespace KOTF.Core.Character
{
    public class MainPlayerCharacter : MonoBehaviour, ICharacter
    {
        [Header("Movement")]
        [SerializeField] private InputAction _movementInput = null;
        [SerializeField] private float _movementSpeed = 10.0f;

        private void Update()
        {
            ProcessInput();
        }

        #region Create Wrapper for the InputSystem
        private void OnEnable()
        {
            _movementInput.Enable();
        }

        private void OnDisable()
        {
            _movementInput.Disable();
        }
        #endregion

        private void ProcessInput()
        {
            Move();
        }

        public void Move()
        {
            // Read Input using new Input System.
            float longitudinalValue = _movementInput.ReadValue<Vector3>().z;
            float lateralValue = _movementInput.ReadValue<Vector3>().x;

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
