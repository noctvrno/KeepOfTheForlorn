using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [SerializeField]
        [Tooltip("The movement speed value. This controls how fast the character walks.")]
        private float _movementSpeed;
        [SerializeField]
        [Tooltip("The ratio of the movement speed while sprinting to walking. If the ratio is 2.0, that means that sprinting will be twice as fast as walking.")]
        private float _sprintToMovementSpeedRatio;
        [SerializeField]
        [Tooltip("How fast the movement speed reaches the sprinting speed.")]
        private float _acceleration;

        // These fields should be readonly but Unity does not support their usage.
        private float _minMovementSpeed;
        private float _maxMovementSpeed;

        private InputHandler _movementInput;
        private InputHandler _attackInput;
        private InputHandler _sprintInput;
        private CharacterController _characterController;

        private EquipmentService _equipmentService;

        protected override void Awake()
        {
            new Initialization.Initializer().Initialize();
            base.Awake();

            _minMovementSpeed = _movementSpeed;
            _maxMovementSpeed = _sprintToMovementSpeedRatio * _movementSpeed;

            InitializeInputs();
            _equipmentService = ServiceProvider.Get<EquipmentService>();

            _equipmentService.AttachEquipmentTo<Weapon>(WeaponPrefabNames.LONGSWORD, gameObject);
        }

        private void InitializeInputs()
        {
            _movementInput = InputFactory.GetInput(ActionType.Movement);
            _attackInput = InputFactory.GetInput(ActionType.Attack);
            _sprintInput = InputFactory.GetInput(ActionType.Sprint);
        }

        protected override void Start()
        {
            base.Start();

            AnimationService.ValidateAnimator(this);

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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            _movementSpeed = _movementSpeed.AlterWithinRangeTol(_minMovementSpeed, _maxMovementSpeed, _acceleration,
                Convert.ToBoolean(_sprintInput.Input.ReadValue<float>()));
            //Debug.Log($"Movement speed: {_movementSpeed}");
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
