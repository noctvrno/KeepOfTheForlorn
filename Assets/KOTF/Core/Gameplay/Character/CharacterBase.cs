using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KOTF.Core.Gameplay.Equipment;
using KOTF.Core.Services;
using KOTF.Core.Wrappers;
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

        protected void TriggerAttackAnimation(bool value)
        {
            AnimationService.TriggerAttackAnimation(Animator, value);
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
