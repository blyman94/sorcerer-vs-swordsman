using UnityEngine;

namespace Game
{
    public class GameManagerHook : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            GameManager.Instance.LoadScene(sceneName);
        }

        public void ShowHowToPlay()
        {
            GameManager.Instance.ShowHowToPlay();
        }

        public void QuitGame()
        {
            GameManager.Instance.QuitGame();
        }
    }
}

