using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using KOTF.Utils.Extensions;
using KOTF.Core.Input;
using KOTF.Core.Services;
using KOTF.Utils.StringConstants;
using UnityEditorInternal;
using AnimatorController = UnityEditor.Animations.AnimatorController;
using Object = UnityEngine.Object;

namespace KOTF.Core.Gameplay.Character
{
    public class MainPlayerCharacter : MonoBehaviour, ICharacter
    {
        [Header("Movement")]
        [SerializeField] private float _movementSpeed = 10.0f;
        [SerializeField] private AnimationClip _testAnimation;
        private InputHandler _movementInput = null;
        private InputHandler _attackInput = null;
        private Animator _animator = null;

        private ServiceProvider _serviceProvider;
        private EquipmentService _equipmentService;

        private void Awake()
        {
            new Initialization.Initializer().Initialize();

            _movementInput = InputFactory.GetInput(InputActionType.Movement);
            _attackInput = InputFactory.GetInput(InputActionType.Attack);
            if (_movementInput == null)
                Debug.LogError($"{_movementInput} is null which is not permitted!");

            _serviceProvider = ServiceProvider.GetInstance();
            _equipmentService = _serviceProvider.Get<EquipmentService>();

            _equipmentService.AttachObjectTo(WeaponPrefabNames.LONGSWORD, gameObject);
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();

            // Update the Animator to make sure that all references and properties are correct.
            _animator.runtimeAnimatorController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        }

        private void Update()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            Move();
            Attack();
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

        public void Attack()
        {
            _animator.SetBool(AnimationConstants.ATTACK, Convert.ToBoolean(_attackInput.Input.ReadValue<float>()));
        }
    }
}
