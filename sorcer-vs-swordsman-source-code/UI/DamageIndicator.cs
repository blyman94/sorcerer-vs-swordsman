using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class DamageIndicator : MonoBehaviour
    {
        [Tooltip("TMPro text component of the damage indicator object.")]
        public TextMeshProUGUI damageText;

        [Header("Fonts and Font Colors")]

        [Tooltip("Font asset to use for regular hits.")]
        public TMP_FontAsset Font;

        public Color FontColor {get; set;}

        [Header("Damage Popup Behaviour")]

        [Tooltip("How long the popup lasts for.")]
        public float Lifetime = 1.2f;

        [Tooltip("How far into the life time does the popup start to fade.")]
        public float FadeStartTime = 0.6f;

        [Tooltip("Minimum distance the popup can travel away from source " +
            "during its lifetime.")]
        public float MinDistance = 1.0f;

        [Tooltip("Maximum distance the popup can travel away from source " +
            "during its lifetime.")]
        public float MaxDistance = 2.0f;

        /// <summary>
        /// Starting position of the damage popup.
        /// </summary>
        public Vector2 StartPos { get; set; }

        /// <summary>
        /// Target position of the damage popup.
        /// </summary>
        private Vector2 targetPos;

        /// <summary>
        /// Timer to track how long the damage popup has been active for.
        /// </summary>
        private float timer;

        private void OnEnable()
        {
            transform.position = StartPos;
            timer = 0.0f;

            Vector2 direction = Random.insideUnitCircle;
            StartPos = transform.position;
            float distance = Random.Range(MinDistance, MaxDistance);
            targetPos = StartPos + (direction * distance);

            transform.localScale = Vector3.zero;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer > Lifetime)
            {
                gameObject.SetActive(false);
            }
            else if (timer > FadeStartTime)
            {
                damageText.color =
                    Color.Lerp(damageText.color, Color.clear,
                    (timer - FadeStartTime) / (Lifetime - FadeStartTime));
            }

            transform.localPosition =
                Vector3.Lerp(StartPos, targetPos, Mathf.Sin(timer / Lifetime));
            transform.localScale =
                Vector3.Lerp(Vector3.zero, Vector3.one,
                Mathf.Sin(timer / Lifetime));
        }

        /// <summary>
        /// Set the text displayed to the damage taken.
        /// </summary>
        /// <param name="damage">Damage number to be displayed.</param>
        public void SetDamageText(int damage)
        {
            damageText.color = FontColor;
            damageText.font = Font;
            damageText.text = damage.ToString();
        }
    }
}
