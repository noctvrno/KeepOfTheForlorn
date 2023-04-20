﻿using System;
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
        [field: SerializeField]
        public GatedAttribute<float> MovementSpeedAttribute { get; private set; }

        [field: SerializeField]
        public AnalogAttributeModifier MovementSpeedEnhancer { get; private set; }

        [field: SerializeField]
        public AnalogAttributeModifier MovementSpeedDiminisher { get; private set; }

        [field: SerializeField]
        public GatedAttribute<float> StaminaAttribute { get; private set; }

        [field: SerializeField]
        public AnalogAttributeModifier BaseStaminaEnhancer { get; private set; }
        #endregion

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

            ScriptableObject.CreateInstance<HudObjectHandler>().Initialize(); // TODO: Perhaps this would be better as a service.

            ChainAttackHandler = new ChainAttackHandler(CharacterAnimationHandler);

            _characterController = GetComponent<CharacterController>();

            BaseStaminaEnhancer.Initialize(StaminaAttribute, StaminaAttribute.MaximumValue);
            MovementSpeedEnhancer.Initialize(MovementSpeedAttribute, MovementSpeedAttribute.MaximumValue);
            MovementSpeedDiminisher.Initialize(MovementSpeedAttribute, MovementSpeedAttribute.MinimumValue);

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
            if (Convert.ToBoolean(_sprintInput.Input.ReadValue<float>()))
                _attributeUpdaterService.Enhance(MovementSpeedEnhancer);
            else
                _attributeUpdaterService.Diminish(MovementSpeedDiminisher);

            Transform currentTransform = transform;
            return (lateralValue * currentTransform.right + longitudinalValue * currentTransform.forward).ToDeltaTime() * MovementSpeedAttribute.Value;
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
