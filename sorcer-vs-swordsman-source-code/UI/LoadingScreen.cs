using UnityEngine;
using TMPro;

namespace Game.UI
{
    /// <summary>
    /// Loading screens are shown while new scenes are loading and display
    /// random tips.
    /// </summary>
    public class LoadingScreen : MonoBehaviour
    {
        [Tooltip("Canvas group representing the loading screen.")]
        public CanvasGroup LoadingScreenGroup;

        [Tooltip("Text to display tips as the game is loading.")]
        public TextMeshProUGUI TipText;

        [SerializeField]
        [Tooltip("Text strings representing tips to be shown randomly " +
            "during load screens.")]
        private string[] tips;

        /// <summary>
        /// Shows the Loading Screen.
        /// </summary>
        public void ShowLoadingScreenGroup()
        {
            UpdateTip();
            LoadingScreenGroup.alpha = 1f;
            LoadingScreenGroup.interactable = true;
            LoadingScreenGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Hides the Loading Screen.
        /// </summary>
        public void HideLoadingScreenGroup()
        {
            LoadingScreenGroup.alpha = 0f;
            LoadingScreenGroup.interactable = false;
            LoadingScreenGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// Updates the tip text of the loading screen with a random tip from 
        /// the tips array.
        /// </summary>
        private void UpdateTip()
        {
            TipText.text = "Tip: " + tips[Random.Range(0, tips.Length)];
        }

    }
}

