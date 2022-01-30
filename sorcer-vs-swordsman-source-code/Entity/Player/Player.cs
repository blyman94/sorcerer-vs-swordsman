using Game.Combat;
using Game.Data;
using Game.Movement;
using Game.Stats;
using Game.Entity;
using UnityEngine;

namespace Game.Entity
{
    /// <summary>
    /// The player entity to be controlled by an instance of the
    /// PlayerController class.
    /// </summary>
    [RequireComponent(typeof(Dasher2D))]
    [RequireComponent(typeof(Mover2D))]
    [RequireComponent(typeof(Jumper2D))]
    [RequireComponent(typeof(WallJumper2D))]
    public class Player : MonoBehaviour
    {
        [Tooltip("Scriptable object to store player stats.")]
        public EntityStatsObject Stats;

        [Header("Audio")]
        public AudioSource PlayerAudio;

        public AudioClip HurtClip;
        public AudioClip DeathClip;
        public AudioClip JumpClip;

        public AudioClip ImpactClip;

        public EntityStatsObject PlayerStats
        {
            get
            {
                return Stats;
            }
        }

        public bool openConnection = false;

        private bool isOnFire = false;

        public Block currentBlock;

        public Rigidbody2D Rigidbody2D { get; set; }

        public PlayerCombatTarget CombatTarget { get; set; }

        public MeleeFighter MeleeFighter { get; set; }

        public Croucher2D Croucher2D { get; set; }

        public Dasher2D Dasher2D { get; set; }

        public Jumper2D Jumper2D { get; set; }

        public Mover2D Mover2D { get; set; }

        public WallJumper2D WallJumper2D { get; set; }

        public BoxCollider2D bc2d { get; set; }

        public GameObject FireVfx;

        /// <summary>
        /// Stores the previous position of the player transform. Before
        /// updating the PlayerPosition data object, this script first checks
        /// to make sure the position has actually changed. This decision was
        /// made to increase efficiency.
        /// </summary>
        private Vector3 lastPosition;

        float damageTimer = 0;

        private void Awake()
        {
            GetPlayerComponents();
            UpdatePosition();
        }

        private void OnEnable()
        {
            CombatTarget.DisableMove += Mover2D.SetDisableMovementTimer;
            CombatTarget.DisableMove += Jumper2D.SetDisableMovementTimer;
            CombatTarget.Died += EndGame;

            CombatTarget.DamageTaken += PlayHurtAudio;
            CombatTarget.Died += PlayDeathAudio;
            Jumper2D.jumpStarted += PlayJumpAudio;

            MeleeFighter.DisableMove += Mover2D.SetDisableMovementTimer;
            MeleeFighter.DisableMove += Jumper2D.SetDisableMovementTimer;
        }

        private void Start()
        {
            FireVfx.SetActive(false);
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
            if (transform.position != lastPosition)
            {
                UpdatePosition();
            }
        }

        private void PlayHurtAudio()
        {
            if (CombatTarget.Health.Current > 0)
            {
                PlayerAudio.pitch = Random.Range(1.1f, 1.5f);
                PlayerAudio.PlayOneShot(HurtClip, 0.85f);
            }
        }

        public void PlayImpactAudio()
        {
            PlayerAudio.PlayOneShot(ImpactClip, 0.25f);
        }

        private void PlayDeathAudio()
        {
            PlayerAudio.pitch = Random.Range(1.1f, 1.35f);
            PlayerAudio.Stop();
            PlayerAudio.PlayOneShot(DeathClip, 0.75f);
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

        private void PlayJumpAudio()
        {
            bool playAudio = Random.value > 0.5f;
            if (playAudio)
            {
                PlayerAudio.pitch = Random.Range(1.1f, 1.35f);
                PlayerAudio.PlayOneShot(JumpClip, 0.75f);
            }
        }

        public void UpdateMoveSpeed()
        {
            Mover2D.MaxSpeed = Stats.MoveSpeed;
        }

        public void InteractWithBlock(Block block)
        {
            if ((transform.position - block.transform.position).sqrMagnitude < 3.0f)
            {
                if (!openConnection)
                {
                    StartBlockConnection(block);
                }
                else
                {
                    EndBlockConnection(block);
                }
            }
            else
            {
                Debug.Log("Block not close enough!");
            }
        }

        public void StartBlockConnection(Block block)
        {
            bool connectionSuccessful = block.StartConnection(false);
            if (connectionSuccessful)
            {
                openConnection = true;
                currentBlock = block;
            }
        }

        public void EndBlockConnection(Block block)
        {
            bool connectionSuccessful = block.CloseConnection(currentBlock);
            if (connectionSuccessful)
            {
                openConnection = false;
                currentBlock = null;
            }
        }

        private void EndGame()
        {
            transform.tag = "Untagged";
            Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
            GameManager.Instance.EndGame(false);
        }

        /// <summary>
        /// Uses GameObject.GetComponent to gather references to each of the 
        /// player's components.
        /// </summary>
        private void GetPlayerComponents()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();

            CombatTarget = GetComponent<PlayerCombatTarget>();
            CombatTarget.Stats = PlayerStats;

            MeleeFighter = GetComponent<MeleeFighter>();
            MeleeFighter.Stats = PlayerStats;

            Croucher2D = GetComponent<Croucher2D>();
            Dasher2D = GetComponent<Dasher2D>();
            Jumper2D = GetComponent<Jumper2D>();
            Mover2D = GetComponent<Mover2D>();
            WallJumper2D = GetComponent<WallJumper2D>();

            bc2d = GetComponent<BoxCollider2D>();
        }

        /// <summary>
        /// Updates the player position
        /// </summary>
        private void UpdatePosition()
        {
            lastPosition = transform.position;
            PlayerPosition.Current = (Vector2)transform.position;
        }
    }
}
