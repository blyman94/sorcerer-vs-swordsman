using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.UI;
using Game.Combat;

namespace Game
{
    /// <summary>
    /// Uses an object pool to store damage popup indicators. The pool can grow
    /// if the number of popups on screen eclipses the size of the pool.
    /// </summary>
    public class DamagePopupManager : Singleton<DamagePopupManager>
    {
        [Tooltip("GameObject prefab representing the damage popup.")]
        public GameObject DamagePopupPrefab;

        [Header("Object Pool Properties")]

        [Tooltip("Number of damage popups to instantiate in the damage popup " +
            "object pool.")]
        [SerializeField]
        private int InitialPoolSize = 10;

        [Tooltip("Transform representing the parent object of all damage " +
            "popup objects.")]
        [SerializeField]
        private Transform DamagePopupParent;

        /// <summary>
        /// Object pool for damage popups.
        /// </summary>
        private List<GameObject> damagePopupObjectPool;

        /// <summary>
        /// Current size of the object pool containing damage counters. When the
        /// pool grows this number increases.
        /// </summary>
        private int currentPoolSize;

        private void OnEnable()
        {
            SorcererCombatTarget.RequestDamagePopup += SpawnDamageCounter;
            PlayerCombatTarget.RequestDamagePopup += SpawnDamageCounter;
        }

        private void Start()
        {
            InitializeObjectPool();
        }

        /// <summary>
        /// Initializes the damage popup object pool.
        /// </summary>
        private void InitializeObjectPool()
        {
            damagePopupObjectPool = new List<GameObject>();

            for (int i = 0; i < InitialPoolSize; i++)
            {
                GameObject damagePopup = Instantiate(DamagePopupPrefab,
                    Vector3.zero, Quaternion.identity, DamagePopupParent);
                damagePopup.SetActive(false);
                damagePopupObjectPool.Add(damagePopup);
            }

            currentPoolSize = damagePopupObjectPool.Count;
        }

        /// <summary>
        /// Returns a damage popup GameObject from the object pool. If one is 
        /// not available, a new one is instantiated and the pool grows.
        /// </summary>
        /// <returns></returns>
        private GameObject GetObjectFromPool()
        {
            // Return the first inactive damage popup object.
            for (int i = 0; i < currentPoolSize; i++)
            {
                if (!damagePopupObjectPool[i].activeInHierarchy)
                {
                    return damagePopupObjectPool[i];
                }
            }

            // If no inactive damage popup is available, grow the pool and
            // return the new damage popup.
            GameObject damagePopup = Instantiate(DamagePopupPrefab,
                Vector3.zero, Quaternion.identity, DamagePopupParent);
            damagePopup.SetActive(false);
            damagePopupObjectPool.Add(damagePopup);

            currentPoolSize = damagePopupObjectPool.Count;

            return damagePopup;
        }

        /// <summary>
        /// Method called to spawn a damage counter at a given origin, 
        /// displaying the amount of damage taken.
        /// </summary>
        /// <param name="origin">Position where the damage counter should
        /// spawn.</param>
        /// <param name="damage">The amount of damage the counter should 
        /// display.</param>
        /// <param name="critical">Whether or not the damage taken was from a 
        /// critical hit. Critical hits are displayed in a different font and
        /// color.</param>
        public void SpawnDamageCounter(Vector3 origin, int damage, bool player)
        {
            GameObject damagePopupObject = GetObjectFromPool();
            DamageIndicator damageIndicator =
                damagePopupObject.GetComponent<DamageIndicator>();
            damageIndicator.StartPos = origin;
            if (player)
            {
                damageIndicator.FontColor = Color.cyan;
            }
            else
            {
                damageIndicator.FontColor = Color.yellow;
            }
            damageIndicator.SetDamageText(damage);
            damagePopupObject.SetActive(true);
        }
    }
}

