using UnityEngine;

namespace Game.Combat.Reserves
{
    /// <summary>
    /// Health reserve. If it drops to or below 0, the entity will die.
    /// </summary>
    public class HealthReserve : MonoBehaviour, IReserve
    {
        public event CurrentChanged CurrentChanged;
        public event MaxChanged MaxChanged;
        public event ReserveEmpty Empty;

        public float Current
        {
            get
            {
                return currentHealth;
            }
            set
            {
                float previousHealth = currentHealth;
                currentHealth = value;
                CurrentChanged?.Invoke(currentHealth,
                    previousHealth > currentHealth);
                if (currentHealth == 0)
                {
                    Empty?.Invoke();
                }
            }
        }

        public float Max
        {
            get
            {
                return maxHealth;
            }
            set
            {
                maxHealth = value;
                Current = value;
                MaxChanged?.Invoke(maxHealth);
            }
        }

        /// <summary>
        /// Maximum value of the health reserve.
        /// </summary>
        private float maxHealth;

        /// <summary>
        /// Current value of the health reserve.
        /// </summary>
        private float currentHealth;

        private void OnDisable()
        {
            CurrentChanged = null;
            MaxChanged = null;
            Empty = null;
        }

        public void Modify(float amount)
        {
            if (currentHealth + amount > maxHealth)
            {
                Current = Max;
            }
            else if (currentHealth + amount <= 0)
            {
                Current = 0;
            }
            else
            {
                Current += amount;
            }
        }
    }
}

