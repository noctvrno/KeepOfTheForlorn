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
        protected ServiceProvider ServiceProvider { get; private set; }
        protected AnimationService AnimationService { get; private set; }
        protected Animator Animator { get; private set; }
        public Weapon WieldedWeapon { get; set; }
        #endregion

        protected virtual void Awake()
        {
            ServiceProvider = ServiceProvider.GetInstance();
            AnimationService = ServiceProvider.Get<AnimationService>();
        }

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            Move();
            Attack();
        }

        protected void TriggerAttackAnimation(bool value)
        {
            AnimationService.TriggerAttackAnimation(Animator, value);
        }

        public abstract void Move();
        public abstract void Attack();

        public virtual void Die()
        {
            Debug.Log($"{gameObject.name} has died.");
            Destroy(gameObject);
        }

        public virtual void OnEnterAttackWindow()
        {

        }

        public virtual void OnExitAttackWindow()
        {

        }

    }
}
