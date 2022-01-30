using UnityEngine;

namespace Game.Stats
{
    /// <summary>
    /// Holds various stats of an entity.
    /// </summary>
    [CreateAssetMenu(menuName = "Stats.../EntityStats",
        fileName = "new EntityStats")]
    public class EntityStatsObject : ScriptableObject
    {
        [Tooltip("Maximum health of the entity.")]
        public float MaxHealth;
        public float Damage;
        public float MoveSpeed;
        public float OriginalDamage;
        public float OriginalMoveSpeed;

        public void OnEnable()
        {
            ResetStats();
        }

        private void ResetStats()
        {
            Damage = OriginalDamage;
            MoveSpeed = OriginalMoveSpeed;
        }
    }
}
