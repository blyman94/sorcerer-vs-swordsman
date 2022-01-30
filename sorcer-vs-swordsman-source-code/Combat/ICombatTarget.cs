using Game.Combat.Reserves;
using Game.Combat.Utilities;
using Game.Stats;

namespace Game.Combat
{
    /// <summary>
    /// Interface to describe a combat target, which can take damage and die.
    /// </summary>
    public interface ICombatTarget
    {
        /// <summary>
        /// CombatTarget's stats. Passed to CombatTarget from its entity.
        /// </summary>
        /// <value></value>
        EntityStatsObject Stats { get; set; }

        /// <summary>
        /// CombatTargets's health reserve.
        /// </summary>
        IReserve Health { get; }

        event DamageTaken DamageTaken;

        event Died Died;

        /// <summary>
        /// CombatTarget's response to taking damage.
        /// </summary>
        /// <param name="howMuchDamage">How much damage the CombatTarget
        /// will take.</param>
        void TakeDamage(float howMuchDamage);

        /// <summary>
        /// CombatTarget's response to death trigger.
        /// </summary>
        void Die();
    }
}

