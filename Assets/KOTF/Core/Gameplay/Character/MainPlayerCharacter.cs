using System;
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
        private InputHandler _movementInput;

        private CharacterController _characterController;
        private EquipmentService _equipmentService;
        private InputService _inputService;

        #region Serializable fields
        [field: SerializeField]
        public GatedAttribute<float> MovementSpeedAttribute { get; private set; }

        [field: SerializeField]
        public AnalogAttributeModifier MovementSpeedEnhancer { get; private set; }

        [field: SerializeField]
        public AuxiliaryAnalogAttributeModifier MovementSpeedDiminisher { get; private set; }

        [field: SerializeField]
        public DiscreteAttributeModifier AttackingMovementSpeedModifier { get; private set; }

        [field: SerializeField]
        public GatedAttribute<float> StaminaAttribute { get; private set; }

        [field: SerializeField]
        public AnalogAttributeModifier BaseStaminaEnhancer { get; private set; }
        #endregion

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
            _inputService = ServiceProvider.Get<InputService>();
            _equipmentService.AttachEquipmentTo<Weapon>(WeaponPrefabNames.LONGSWORD, gameObject);
        }

        protected override void InitializeFields()
        {
            base.InitializeFields();

            InitializeInputs();

            ScriptableObject.CreateInstance<HudObjectHandler>().Initialize(); // TODO: Perhaps this would be better as a service.

            ChainAttackHandler = new ChainAttackHandler(CharacterAnimationHandler);

            _characterController = GetComponent<CharacterController>();

            BaseStaminaEnhancer.Initialize(StaminaAttribute);
            MovementSpeedEnhancer.Initialize(MovementSpeedAttribute);
            MovementSpeedDiminisher.Initialize(MovementSpeedAttribute);
            AttackingMovementSpeedModifier.Initialize(MovementSpeedAttribute);

            // Update the Animator to make sure that all references and properties are correct.
            Animator.runtimeAnimatorController = new AnimatorOverrideController(Animator.runtimeAnimatorController);
        }

        private void InitializeInputs()
        {
            _movementInput = _inputService.Get(ActionType.Movement);
            _inputService.Get(ActionType.Attack)
                .WithPerformedCallback(_ => Attack());
            _inputService.Get(ActionType.Sprint)
                .WithStartedCallback(_ => BeginSprint())
                .WithPerformedCallback(_ => EndSprint());
            _inputService.Get(ActionType.Block)
                .WithStartedCallback(_ => BlockAttackHandler.Block())
                .WithPerformedCallback(_ => BlockAttackHandler.Release());
        }

        protected override void Move()
        {
            // Read Input using new Input System.
            Vector3 inputMovement = _movementInput.Input.ReadValue<Vector3>();
            float longitudinalValue = inputMovement.z;
            float lateralValue = inputMovement.x;

            // Apply corresponding forces.
            Vector3 computedMovementVector = ComputeMovementVector(longitudinalValue, lateralValue);
            _characterController.Move(new Vector3(computedMovementVector.x, 0.0f, computedMovementVector.z));
        }

        private void BeginSprint()
        {
            AttributeUpdaterService.Enhance(MovementSpeedEnhancer, MovementSpeedDiminisher);
        }

        private void EndSprint()
        {
            AttributeUpdaterService.Diminish(MovementSpeedDiminisher, MovementSpeedEnhancer);
        }

        private Vector3 ComputeMovementVector(float longitudinalValue, float lateralValue)
        {
            Transform currentTransform = transform;
            return (lateralValue * currentTransform.right + longitudinalValue * currentTransform.forward).ToDeltaTime() * MovementSpeedAttribute.Value;
        }

        protected override void Attack()
        {
            ChainAttackHandler.Chain();
            AttributeUpdaterService.Diminish(AttackingMovementSpeedModifier);
        }

        public override void OnEnterAttackWindow()
        {
            base.OnEnterAttackWindow();
            AttributeUpdaterService.Diminish(WieldedWeapon.StaminaDiminisher);
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
            ChainAttackHandler.ResetChainPossibility();
            AttributeUpdaterService.Enhance(BaseStaminaEnhancer);
            AttributeUpdaterService.Enhance(AttackingMovementSpeedModifier);
        }
    }
}
