using System;
using System.Collections;
using KOTF.Core.Gameplay.Attribute;
using KOTF.Core.Gameplay.Equipment;
using UnityEngine;
using KOTF.Utils.Extensions;
using KOTF.Core.Input;
using KOTF.Core.Services;
using KOTF.Core.UI.HUD;
using KOTF.Utils.StringConstants;

namespace KOTF.Core.Gameplay.Character
{
    public class MainPlayerCharacter : CharacterBase, IChainCapable
    {
        #region Serializable fields
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

        [field: SerializeField]
        public GatedAttribute<float> StaminaAttribute { get; private set; }

        [field: SerializeField]
        public AnalogAttributeModifier BaseStaminaEnhancer { get; private set; }
        #endregion

        // These fields should be readonly but Unity does not support their usage.
        private float _minMovementSpeed;
        private float _maxMovementSpeed;

        private InputHandler _movementInput;
        private InputHandler _attackInput;
        private InputHandler _sprintInput;

        private CharacterController _characterController;
        private EquipmentService _equipmentService;
        private AttributeUpdaterService _attributeUpdaterService;

        public ChainAttackHandler ChainAttackHandler { get; private set; }

        protected override void Awake()
        {
            new Initialization.Initializer().Initialize();
            base.Awake();
        }

        protected override void InitializeServices()
        {
            base.InitializeServices();

            _equipmentService = ServiceProvider.Get<EquipmentService>();
            _equipmentService.AttachEquipmentTo<Weapon>(WeaponPrefabNames.LONGSWORD, gameObject);

            _attributeUpdaterService = ServiceProvider.Get<AttributeUpdaterService>();
        }

        protected override void InitializeFields()
        {
            base.InitializeFields();

            InitializeInputs();

            _minMovementSpeed = _movementSpeed;
            _maxMovementSpeed = _sprintToMovementSpeedRatio * _movementSpeed;

            ScriptableObject.CreateInstance<HudObjectHandler>().Initialize(); // TODO: Perhaps this would be better as a service.

            ChainAttackHandler = new ChainAttackHandler(CharacterAnimationHandler);

            _characterController = GetComponent<CharacterController>();

            BaseStaminaEnhancer.Initialize(StaminaAttribute, StaminaAttribute.MaximumValue);

            // Update the Animator to make sure that all references and properties are correct.
            Animator.runtimeAnimatorController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        }

        private void InitializeInputs()
        {
            _movementInput = InputFactory.GetInput(ActionType.Movement);
            _attackInput = InputFactory.GetInput(ActionType.Attack);
            _sprintInput = InputFactory.GetInput(ActionType.Sprint);
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
            ComputeMovementSpeed();
            Transform currentTransform = transform;
            return (lateralValue * currentTransform.right + longitudinalValue * currentTransform.forward).ToDeltaTime() * _movementSpeed;
        }

        private void ComputeMovementSpeed()
        {
            _movementSpeed = Convert.ToBoolean(_sprintInput.Input.ReadValue<float>())
                ? Mathf.Clamp(_movementSpeed + _movementSpeed * Time.deltaTime * _acceleration, _minMovementSpeed,
                    _maxMovementSpeed)
                : Mathf.Clamp(_movementSpeed - _movementSpeed * Time.deltaTime * _acceleration, _minMovementSpeed,
                    _maxMovementSpeed);
        }

        public override void Attack()
        {
            if (!Convert.ToBoolean(_attackInput.Input.ReadValue<float>()))
                return;

            ChainAttackHandler.Chain();
        }

        public override void OnEnterAttackWindow()
        {
            base.OnEnterAttackWindow();
            _attributeUpdaterService.Diminish(WieldedWeapon.StaminaDiminisher);
        }

        public override void OnExitAttackWindow()
        {
            base.OnExitAttackWindow();
            ChainAttackHandler.RegisterChainPossibility();
        }

        public void OnExitChainPossibility()
        {
            ChainAttackHandler.ExitChain();
        }

        public override void OnExitAttackAnimation()
        {
            ChainAttackHandler.ResetChain();
            _attributeUpdaterService.Enhance(BaseStaminaEnhancer);
        }
    }
}
