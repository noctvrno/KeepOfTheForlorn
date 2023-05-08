using KOTF.Core.Gameplay.Attribute;
using KOTF.Core.Gameplay.Equipment;
using KOTF.Core.Services;
using KOTF.Core.Wrappers;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public abstract class CharacterBase : KotfGameObject, IAggressive
    {
        #region Serializable fields
        [field: SerializeField]
        public GatedAttribute<float> HealthAttribute { get; private set; }
        #endregion

        #region Object references
        public Weapon WieldedWeapon { get; private set; }
        protected ServiceProvider ServiceProvider { get; private set; }
        protected CharacterAnimationHandler CharacterAnimationHandler { get; private set; }
        protected CharacterColliderService CharacterColliderService { get; private set; }
        public Animator Animator { get; private set; }
        public AnimatorController AnimatorController { get; private set; }
        #endregion

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {
            InitializeServices();
            InitializeFields();
        }

        protected virtual void InitializeServices()
        {
            ServiceProvider = ServiceProvider.GetInstance();
            CharacterColliderService = ServiceProvider.Get<CharacterColliderService>();
        }

        protected virtual void InitializeFields()
        {
            WieldedWeapon = GetComponentInChildren<Weapon>();

            Animator = GetComponent<Animator>();
            AnimatorController =
                AssetDatabase.LoadAssetAtPath<AnimatorController>(
                    AssetDatabase.GetAssetPath(Animator.runtimeAnimatorController));

            if (AnimatorController == null)
            {
                Debug.LogError($"Could not find an associated AnimatorController for {name}.");
                return;
            }

            CharacterAnimationHandler = new CharacterAnimationHandler(this);
            CharacterAnimationHandler.ValidateAnimator();
        }

        protected virtual void Update()
        {
            Move();
        }

        protected abstract void Move();

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

        public virtual void OnExitAttackAnimation()
        {

        }
    }
}
