using Game.Combat;
using Game.Movement;
using Game.Stats;
using UnityEngine;

namespace Game.Entity
{
    /// <summary>
    /// Interface to determine what is considered a player entity.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// EntityStatsObject of the player. All players have a set of stats
        /// that influence many aspects of gameplay.
        /// </summary>
        EntityStatsObject PlayerStats { get; }

        /// <summary>
        /// Player's Rigidbody2D component used for motion.
        /// </summary>
        /// <value></value>
        Rigidbody2D Rigidbody2D { get; }

        /// <summary>
        /// MeleeFighter component of the player. All players can deal damage
        /// via melee attacks.
        /// </summary>
        MeleeFighter MeleeFighter { get; }

        /// <summary>
        /// CombatTarget component of the player. All players can recieve damage
        /// and die.
        /// </summary>
        PlayerCombatTarget CombatTarget { get; }

        /// <summary>
        /// Croucher2D component of the player. All players can crouch to
        /// reduce hit chance.
        /// </summary>
        /// <value></value>
        Croucher2D Croucher2D { get; }

        /// <summary>
        /// Dasher2D component of the player. All players can dash.
        /// </summary>
        Dasher2D Dasher2D { get; }

        /// <summary>
        /// Jumper2D component of the player. All players can jump.
        /// </summary>
        Jumper2D Jumper2D { get; }

        /// <summary>
        /// Mover2D component of the player. All players can move horizontally.
        /// </summary>
        Mover2D Mover2D { get; }

        /// <summary>
        /// WallJumper2D component of the player. All players can wall jump.
        /// </summary>
        WallJumper2D WallJumper2D { get; }

        void StartBlockConnection(Block block);
    }
}
