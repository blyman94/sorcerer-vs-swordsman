using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Movement;
using Game.Combat;
using Game.Stats;

namespace Game.Entity
{
    public class Sorcerer : MonoBehaviour
    {
        public Flyer2D Flyer2D { get; set; }

        public Influencer Influencer;

        public GameObject FireVfx;

        public Animator animator;

        public Shooter Shooter;

        [Tooltip("Scriptable object to store player stats.")]
        public EntityStatsObject Stats;

        [Header("Audio")]
        public AudioSource SorcAudio;
        public AudioClip DeathClip1;
        public AudioClip DeathClip2;
        public AudioClip HitClip;

        public SorcererCombatTarget CombatTarget { get; set; }

        private bool isOnFire;
        private float damageTimer;

        private void OnEnable()
        {
            CombatTarget.DamageTaken += TriggerTakeDamage;
            CombatTarget.DamageTaken += PlayTakeDamageAudio;
            CombatTarget.Died += EndGame;
            CombatTarget.Died += TriggerDieAnim;
            CombatTarget.Died += PlayDeathAudio;

            Shooter.shotFired += TriggerAttackAnim;
            Shooter.megaShot += TriggerMegaShotAnim;
        }

        private void Start()
        {
            FireVfx.SetActive(false);
        }

        private void TriggerMegaShotAnim()
        {
            animator.SetTrigger("megaAttack_t");
        }

        private void TriggerAttackAnim()
        {
            animator.SetTrigger("attack_t");
        }

        private void TriggerTakeDamage()
        {
            animator.SetTrigger("hit_t");
        }

        private void PlayTakeDamageAudio()
        {
            SorcAudio.pitch = Random.Range(0.95f, 1.05f);
            SorcAudio.PlayOneShot(HitClip, 0.75f);
        }

        private void PlayDeathAudio()
        {
            Shooter.StopAllCoroutines();
            SorcAudio.PlayOneShot(DeathClip1, 1f);
            SorcAudio.PlayOneShot(DeathClip2, 1f);
        }

        private void TriggerDieAnim()
        {
            Shooter.StopAllCoroutines();
            animator.SetTrigger("die_t");
        }

        public EntityStatsObject SorcererStats
        {
            get
            {
                return Stats;
            }
        }

        public void Awake()
        {
            Flyer2D = GetComponent<Flyer2D>();
            Flyer2D.moveSpeed = Stats.MoveSpeed * 40;

            CombatTarget = GetComponent<SorcererCombatTarget>();
            CombatTarget.Stats = SorcererStats;

            Shooter.Stats = SorcererStats;
        }

        private void Update()
        {
            if (isOnFire)
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= 2.0f)
                {
                    damageTimer = 0;
                    CombatTarget.TakeDamage(2);
                }
            }
        }

        public void Ignite()
        {
            if (!isOnFire)
            {
                isOnFire = true;
                FireVfx.SetActive(true);
            }
        }

        public void Snuff()
        {
            if (isOnFire)
            {
                isOnFire = false;
                FireVfx.SetActive(false);
            }
        }


        public void UpdateMoveSpeed()
        {
            Flyer2D.moveSpeed = Stats.MoveSpeed * 40;
        }

        private void EndGame()
        {
            Shooter.StopAllCoroutines();
            GameManager.Instance.EndGame(true);
        }
    }
}

