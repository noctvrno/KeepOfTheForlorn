using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Equipment;
using UnityEngine;
using KOTF.Utils.Extensions;
using KOTF.Core.Input;
using KOTF.Core.Services;
using KOTF.Utils.StringConstants;

namespace KOTF.Core.Gameplay.Character
{
    public class MainPlayerCharacter : CharacterBase
    {
        [Header("Movement")]
        [SerializeField] private float _movementSpeed = 10.0f;
        private InputHandler _movementInput;
        private InputHandler _attackInput;
        private CharacterController _characterController;

        private EquipmentService _equipmentService;

        protected override void Awake()
        {
            new Initialization.Initializer().Initialize();
            base.Awake();

            _movementInput = InputFactory.GetInput(InputActionType.Movement);
            _attackInput = InputFactory.GetInput(InputActionType.Attack);
            if (_movementInput == null)
                Debug.LogError($"{_movementInput} is null which is not permitted!");

            _equipmentService = ServiceProvider.Get<EquipmentService>();

            _equipmentService.AttachEquipmentTo<Weapon>(WeaponPrefabNames.LONGSWORD, gameObject);
        }

        protected override void Start()
        {
            base.Start();
            _characterController = GetComponent<CharacterController>();

            // Update the Animator to make sure that all references and properties are correct.
            Animator.runtimeAnimatorController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        }

        public override void Move()
        {
            // Read Input using new Input System.
            Vector3 inputMovement = _movementInput.Input.ReadValue<Vector3>();
            float longitudinalValue = inputMovement.z;
            float lateralValue = inputMovement.x;

            // Apply corresponding forces.
            Vector3 computedMovementVector = ComputeMovementVector(longitudinalValue, lateralValue);
            _characterController.Move(new Vector3(computedMovementVector.x, 0.0f, computedMovementVector.z));
        }

        private Vector3 ComputeMovementVector(float longitudinalValue, float lateralValue)
        {
            return (lateralValue * transform.right + longitudinalValue * transform.forward).ToDeltaTime() * _movementSpeed;
        }

        public override void Attack()
        {
            TriggerAttackAnimation(Convert.ToBoolean(_attackInput.Input.ReadValue<float>()));
        }
    }
}
