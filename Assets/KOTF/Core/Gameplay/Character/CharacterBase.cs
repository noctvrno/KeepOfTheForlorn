using KOTF.Core.Gameplay.Attribute;
using KOTF.Core.Gameplay.Equipment;
using KOTF.Core.Input;
using KOTF.Core.Services;
using KOTF.Core.Wrappers;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace KOTF.Core.Gameplay.Character
{
    public abstract class CharacterBase : KotfGameObject, IAggressive, IParryCapable
    {
        #region Serializable fields
        [field: SerializeField]
        public GatedAttribute<float> HealthAttribute { get; private set; }

        [field: SerializeField]
        public virtual float ParryWindowFrames { get; private set; }
        #endregion

        #region Object references
        public Weapon WieldedWeapon { get; private set; }
        protected ServiceProvider ServiceProvider { get; private set; }
        public CharacterAnimationHandler CharacterAnimationHandler { get; private set; }
        public BlockAttackHandler BlockAttackHandler { get; private set; }
        protected CharacterColliderService CharacterColliderService { get; private set; }
        protected AttributeUpdaterService AttributeUpdaterService { get; private set; }
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
            AttributeUpdaterService = ServiceProvider.Get<AttributeUpdaterService>();
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

            BlockAttackHandler = new BlockAttackHandler(AttributeUpdaterService, this);
        }

        protected virtual void Update()
        {
            Move();
        }

        protected abstract void Move();
        protected abstract void Attack();

        public void Parry()
        {
            CharacterAnimationHandler.TriggerAnimation(ActionType.Parry);
        }

        public void Parried()
        {
            CharacterAnimationHandler.TriggerAnimation(ActionType.Idle);
        }

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

        public void OnExitParryWindow()
        {
            BlockAttackHandler.ResetParryPossibility();
        }
    }
}
