using System.Collections;
using UnityEngine;

namespace Game.Core
{
    /// <summary>
    /// Determines groundedness with a collider (trigger) along the bottom edge 
    /// of a GameObject.
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class GroundSensor2D : MonoBehaviour, ISensor2D
    {
        public delegate void SensorStateChanged(bool newState);
        public SensorStateChanged sensorStateChanged;

        public Collider2D Sensor
        {
            get
            {
                return (Collider2D)boxCollider2D;
            }
        }

        public bool Disabled { get; set; }

        public bool Active
        {
            get
            {
                return active;
            }

            set
            {
                active = value;
                sensorStateChanged?.Invoke(active);
            }
        }

        /// <summary>
        /// Sensor state.
        /// </summary>
        private bool active;

        /// <summary>
        /// Collider that determines groundedness/activeness.
        /// </summary>
        private BoxCollider2D boxCollider2D;

        public void Disable(float duration)
        {
            Disabled = true;
            StopAllCoroutines();
            StartCoroutine(DisableRoutine(duration));
        }

        public IEnumerator DisableRoutine(float duration)
        { 
            yield return new WaitForSeconds(duration);
            Disabled = false;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Platform") || other.CompareTag("Block"))
            {
                Active = true;
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Platform") || other.CompareTag("Block"))
            {
                Active = false;
            }
        }
    }
}
