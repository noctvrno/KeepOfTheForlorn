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
        private Animator _animator;
        private CharacterController _characterController;

        private ServiceProvider _serviceProvider;
        private EquipmentService _equipmentService;
        private CharacterColliderService _characterColliderService;

        private void Awake()
        {
            new Initialization.Initializer().Initialize();

            _movementInput = InputFactory.GetInput(InputActionType.Movement);
            _attackInput = InputFactory.GetInput(InputActionType.Attack);
            if (_movementInput == null)
                Debug.LogError($"{_movementInput} is null which is not permitted!");

            _serviceProvider = ServiceProvider.GetInstance();
            _equipmentService = _serviceProvider.Get<EquipmentService>();

            _equipmentService.AttachEquipmentTo<Weapon>(WeaponPrefabNames.LONGSWORD, gameObject);
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();

            // Update the Animator to make sure that all references and properties are correct.
            _animator.runtimeAnimatorController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

            var serviceProvider = ServiceProvider.GetInstance();
            _characterColliderService = serviceProvider.Get<CharacterColliderService>();
        }

        protected override void Update()
        {
            base.Update();
            Attack();
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
            _animator.SetBool(AnimationConstants.ATTACK, Convert.ToBoolean(_attackInput.Input.ReadValue<float>()));
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }

        public void OnEnterAttackWindow()
        {
            WieldedWeapon.Collider.enabled = true;
        }

        public void OnExitAttackWindow()
        {
            WieldedWeapon.Collider.enabled = false;
            _characterColliderService.Reset();
        }
    }
}
