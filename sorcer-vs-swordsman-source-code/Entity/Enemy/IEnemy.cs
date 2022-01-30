using Game.Combat;
using Game.Stats;

namespace Game.Entity
{
    public interface IEnemy
    {
        /// <summary>
        /// EntityStatsObject of the enemy. All enemies have a set of stats
        /// that influence many aspects of behaviour.
        /// </summary>
        EntityStatsObject Stats { get; }
    }
}
