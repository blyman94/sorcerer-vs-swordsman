using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Game.Entity;
using Game.Data;

namespace Game.Control
{
    public enum AIAction { AttackPlayer, MoveRandomly, ConnectBlocks, DisconnectBlocks, Default }
    public class SorcererController : MonoBehaviour
    {
        private Sorcerer sorcerer;
        private Seeker seeker;

        public Influencer Influencer;

        public Transform enemyGFX;

        [SerializeField] private float nextWayPointDistance = 1.5f;
        [SerializeField] private float pathUpdateRate = 5.0f;

        public float minX, minY, maxX, maxY;

        private Path path;
        private int currentWaypoint = 0;

        private Vector3 targetPos;
        private Vector3 projStartPos;

        private AIAction[] actions =
            new AIAction[1] { AIAction.MoveRandomly };

        private void Awake()
        {
            sorcerer = GetComponent<Sorcerer>();
            seeker = GetComponent<Seeker>();
        }

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.gameStateChanged += OnGameStateChange;
            }
        }

        private void Start()
        {
            projStartPos = sorcerer.Shooter.transform.localPosition;
        }

        private void OnGameStateChange(GameState oldState, GameState newState)
        {
            if ((oldState == GameState.Pregame && newState == GameState.Running) ||
                (oldState == GameState.Postgame && newState == GameState.Running))
            {
                targetPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
                InvokeRepeating("UpdatePath", 0, pathUpdateRate);
                sorcerer.Shooter.InvokeRepeating("FireProjectile", 
                    Random.Range(0.1f, 5.0f), sorcerer.Shooter.ShotCooldown);
                sorcerer.Influencer.InvokeRepeating("Influence", 10, 10);
            }
            else if (oldState == GameState.Running && newState == GameState.Postgame)
            {
                CancelInvoke();
                sorcerer.GetComponent<Rigidbody2D>().constraints = 
                    RigidbodyConstraints2D.FreezeAll;
                sorcerer.Shooter.CancelInvoke();
                sorcerer.Influencer.CancelInvoke();
            }
        }

        private void UpdatePath()
        {
            if (seeker.IsDone())
            {
                targetPos = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0);
                seeker.StartPath(transform.position, targetPos, OnPathComplete);
            }
        }
        private void Update()
        {
            if (PlayerPosition.Current.x < transform.position.x)
            {
                enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                enemyGFX.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        private void FixedUpdate()
        {
            if (path == null)
            {
                return;
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                return;
            }

            Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;

            if (Vector2.Distance(transform.position, targetPos) >= nextWayPointDistance)
            {
                Vector2 force = direction * Time.deltaTime;
                sorcerer.Flyer2D.AddForce(force);
            }

            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWayPointDistance)
            {
                currentWaypoint++;
            }
        }

        protected void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWaypoint = 0;
            }
        }
        protected void UpdatePath(Vector3 target)
        {
            if (seeker.IsDone())
            {
                seeker.StartPath(transform.position, target, OnPathComplete);
            }
        }
    }
}

