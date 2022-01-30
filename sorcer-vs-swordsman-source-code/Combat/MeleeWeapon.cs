using Game.Combat.Utilities;
using Game.Entity;
using Game.Stats;
using Game.Core;
using System.Collections;
using UnityEngine;

namespace Game.Combat
{
    public class MeleeWeapon : MonoBehaviour
    {
        // TODO: Find a slightly more elegant way to determine which direction
        // the attack should go in.
        public SpriteRenderer SpriteRenderer;

        [Header("Audio")]
        public AudioSource WeaponAudio;
        public AudioClip SwordSwooshClip;
        public AudioClip AttackSucessClip;

        public event AttackSuccessful AttackSuccessful;

        public LayerMask WhatIsEnemy;

        public GroundSensor2D GroundSensor2D;

        public Rigidbody2D rb2D;

        [SerializeField]
        private EntityStatsObject entityStats;

        [Header("Weapon Attack Points")]

        [SerializeField]
        private Transform AttackPointPrimary;

        [SerializeField]
        private Transform AttackPointSecondary;

        [SerializeField]
        private Transform AttackPointUp;

        [SerializeField]
        private Transform AttackPointSlam;

        [Header("Weapon Attack Geometry")]

        [SerializeField]
        private Vector2 AttackCapsuleSizePrimary;

        [SerializeField]
        private Vector2 AttackCapsuleSizeSecondary;

        [SerializeField]
        private Vector2 AttackCapsuleSizeUp;

        [SerializeField]
        private Vector2 AttackBoxSizeSlam;

        [SerializeField]
        private float SlamAttackPushForce;

        [HideInInspector]
        public int currentAttack;

        private void OnDisable()
        {
            AttackSuccessful = null;
        }

        public void PrimaryStrike()
        {
            Strike(AttackPointPrimary, AttackCapsuleSizePrimary,
                CapsuleDirection2D.Horizontal);
        }

        public void SecondaryStrike()
        {
            Strike(AttackPointSecondary, AttackCapsuleSizeSecondary,
                CapsuleDirection2D.Horizontal);
        }

        public void UpwardStrike()
        {
            Strike(AttackPointUp, AttackCapsuleSizeUp,
                CapsuleDirection2D.Vertical);
        }

        public void StartSlamStrike()
        {
            StartCoroutine(SlamStrikeRoutine());
        }

        public IEnumerator SlamStrikeRoutine()
        {
            while (true)
            {
                Collider2D[] hitEnemies =
                    Physics2D.OverlapBoxAll(AttackPointSlam.position,
                    AttackBoxSizeSlam, 0.0f, WhatIsEnemy);
                if (hitEnemies.Length > 0)
                {
                    AttackSuccessful?.Invoke(false);
                }
                foreach (Collider2D enemy in hitEnemies)
                {
                    ICombatTarget target = enemy.GetComponent<ICombatTarget>();
                    if (target != null)
                    {
                        target.TakeDamage(entityStats.Damage);
                        Rigidbody2D targetRb = enemy.GetComponent<Rigidbody2D>();
                        Vector2 pushDir = (enemy.transform.position - transform.position).normalized * new Vector2(3.0f, 1.0f);
                        targetRb.velocity = pushDir * SlamAttackPushForce;
                    }
                    LineRenderer connection = enemy.GetComponent<LineRenderer>();
                    if (connection != null)
                    {
                        connection.GetComponentInParent<Block>().SeverConnection(connection);
                    }
                }
                yield return null;
            }
        }

        public void EndSlamStrike()
        {
            StopAllCoroutines();
        }

        public void PlaySwordSwooshAudio()
        {
            WeaponAudio.pitch = Random.Range(0.95f, 1.05f);
            WeaponAudio.PlayOneShot(SwordSwooshClip, 1.0f);
        }

        public void PlaySuccessAttackAudio()
        {
            WeaponAudio.pitch = Random.Range(0.95f, 1.05f);
            WeaponAudio.PlayOneShot(AttackSucessClip, 1.0f);
        }

        private void Strike(Transform attackPoint, Vector2 capsuleSize,
            CapsuleDirection2D capsuleDirection)
        {

            int attackDir = SpriteRenderer.flipX ? -1 : 1;

            Vector2 adjustedPoint =
                transform.TransformPoint(new Vector2(attackPoint.localPosition.x * attackDir,
                attackPoint.localPosition.y));
            PlaySwordSwooshAudio();
            Collider2D[] hitEnemies = Physics2D.OverlapCapsuleAll(adjustedPoint,
                capsuleSize, capsuleDirection,
                0.0f, WhatIsEnemy);
            if (hitEnemies.Length > 0)
            {
                if (!GroundSensor2D.Active)
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x, 0.0f);
                    rb2D.velocity = new Vector2(rb2D.velocity.x, 20.0f);
                }
                AttackSuccessful?.Invoke(false);
            }
            foreach (Collider2D enemy in hitEnemies)
            {
                ICombatTarget target = enemy.GetComponent<ICombatTarget>();
                if (target != null)
                {
                    target.TakeDamage(entityStats.Damage);
                    PlaySuccessAttackAudio();
                    continue;
                }
                LineRenderer connection = enemy.GetComponent<LineRenderer>();
                if (connection != null)
                {
                    connection.GetComponentInParent<Block>().SeverConnection(connection);
                }
            }
        }
    }
}
