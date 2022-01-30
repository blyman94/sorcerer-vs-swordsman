using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    /// <summary>
    /// Responsible for fading scenes in and out.
    /// </summary>
    public class SceneFader : MonoBehaviour
    {
        [Tooltip("Scene fader image.")]
        public Image FaderImage;

        /// <summary>
        /// Fades the current scene out to black.
        /// </summary>
        /// <param name="fadeOutTime">How long the fade should take.</param>
        public IEnumerator FadeOut(float fadeOutTime)
        {
            FaderImage.raycastTarget = true;
            float elapsedTime = 0.0f;
            while (elapsedTime < fadeOutTime)
            {
                float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeOutTime);
                FaderImage.color = new Color(0, 0, 0, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            FaderImage.color = new Color(0, 0, 0, 1);
        }

        /// <summary>
        /// Fades the current scene in from black.
        /// </summary>
        /// <param name="fadeInTime">How long the fade should take.</param>
        public IEnumerator FadeIn(float fadeInTime)
        {
            float elapsedTime = 0.0f;
            while (elapsedTime < fadeInTime)
            {
                float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeInTime);
                FaderImage.color = new Color(0, 0, 0, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            FaderImage.color = new Color(0, 0, 0, 0);
            FaderImage.raycastTarget = false;
        }
    }
}

