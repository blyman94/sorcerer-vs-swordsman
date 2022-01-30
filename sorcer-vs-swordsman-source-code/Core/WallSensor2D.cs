using System.Collections;
using UnityEngine;

namespace Game.Core
{
    public class WallSensor2D : MonoBehaviour, ISensor2D
    {
        public delegate void SensorStateChanged(bool newState);
        public SensorStateChanged sensorStateChanged;

        [Tooltip("True if left wall sensor, false if right wall sensor")]
        [SerializeField]
        private bool isSensorOnLeft;

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
                if (Disabled)
                {
                    return false;
                }
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
        /// Collider that determines activeness of the wall sensor.
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
            if (other.CompareTag("Platform"))
            {
                Active = true;
            }
        }

        public void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Platform"))
            {
                Active = false;
            }
        }
    }
}
