using System.Collections;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Interface specifying properties of a sensor. Sensors are used to
    /// determine behaviour based on positioning relative to other objects
    /// (i.e., a sensor to determine if the subject is on the ground).
    /// </summary>
    public interface ISensor2D
    {
        /// <summary>
        /// Determines activeness of the sensor when other colliders pass 
        /// through it. Assumed to be marked as a trigger.
        /// </summary>
        Collider2D Sensor { get; }

        /// <summary>
        /// Sensor state.
        /// </summary>
        bool Active { get; }

        bool Disabled { get; }

        /// <summary>
        /// Responds to another collider entering this sensor's collider.
        /// </summary>
        /// <param name="other">The collider of the entering GameObject.</param>
        void OnTriggerEnter2D(Collider2D other);

        /// <summary>
        /// Responds to another collider exiting this sensor's collider.
        /// </summary>
        /// <param name="other">The collider of the entering GameObject.</param>
        void OnTriggerExit2D(Collider2D other);

        /// <summary>
        /// Disables the sensor for a given amount of time using a coroutine.
        /// </summary>
        /// <param name="duration">Duration of the sensor disable.</param>
        void Disable(float duration);

        /// <summary>
        /// Coroutine to disable sensor.
        /// </summary>
        /// <param name="duration">Duration of sensor disable.</param>
        IEnumerator DisableRoutine(float duration);
    }
}
