using KOTF.Core.Gameplay.Equipment;
using KOTF.Core.Services;
using KOTF.Core.Wrappers;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public abstract class CharacterBase : KotfGameObject
    {
        #region Serializable fields
        [Header("Stats")]
        [SerializeField] private int _healthPoints = 1000;
        public int HealthPoints
        {
            get => _healthPoints;
            set => _healthPoints = value;
        }
        #endregion

        #region Objected references
        public Weapon WieldedWeapon { get; private set; }
        protected ServiceProvider ServiceProvider { get; private set; }
        protected AnimationService AnimationService { get; private set; }
        protected CharacterColliderService CharacterColliderService { get; private set; }
        public Animator Animator { get; private set; }
        public AnimatorController AnimatorController { get; private set; }
        #endregion

        protected virtual void Awake()
        {
            ServiceProvider = ServiceProvider.GetInstance();
            AnimationService = ServiceProvider.Get<AnimationService>();
            CharacterColliderService = ServiceProvider.Get<CharacterColliderService>();
        }

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
            AnimatorController =
                AssetDatabase.LoadAssetAtPath<AnimatorController>(
                    AssetDatabase.GetAssetPath(Animator.runtimeAnimatorController));

            if (AnimatorController == null)
            {
                Debug.LogError($"Could not find an associated AnimatorController for {name}.");
                return;
            }

            WieldedWeapon = GetComponentInChildren<Weapon>();
            WieldedWeapon.Owner = this;
        }

        protected virtual void Update()
        {
            Move();
            Attack();
        }

        protected virtual void FixedUpdate()
        {

        }

        public abstract void Move();
        public abstract void Attack();

        public virtual void Die()
        {
            Debug.Log($"{name} has died.");
            Destroy(gameObject);
        }

        public virtual void OnEnterAttackWindow()
        {
            WieldedWeapon.Collider.enabled = true;
        }

        public virtual void OnExitAttackWindow()
        {
            WieldedWeapon.Collider.enabled = false;
            CharacterColliderService.Reset();
        }
    }
}
