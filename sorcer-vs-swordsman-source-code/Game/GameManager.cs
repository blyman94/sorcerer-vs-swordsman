using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.UI;
using TMPro;

namespace Game
{
    public enum GameState { Pregame, Running, Paused, Postgame, Default }
    public class GameManager : Singleton<GameManager>
    {
        [Header("UI Objects")]

        [Tooltip("Scene Fader to use between scenes.")]
        public SceneFader SceneFader;

        [Tooltip("Screen to show while game is loading.")]
        public LoadingScreen LoadingScreen;

        public CanvasGroup GameCountdownGroup;

        public TextMeshProUGUI CountdownText;

        public CanvasGroup PauseGroup;

        public HowToPlayHandler HowToPlayHandler;

        [Header("Transition Timing")]

        [Tooltip("Time to load scene")]
        public float LoadTime = 3.0f;

        [Tooltip("Time to fade out of a scene.")]
        public float FadeInTime = 1.5f;

        [Tooltip("Time to fade out of a scene.")]
        public float FadeOutTime = 1.5f;

        [Header("Music")]
        public AudioSource GameManagerMusicAudio;

        public AudioClip TitleScreenMusic;
        public AudioClip BattleSceneMusic;
        public AudioClip DefeatSceneMusic;
        public AudioClip VictorySceneMusic;


        [Header("SFX")]
        public AudioSource GameManagerSFXAudio;
        public AudioClip ReadyClip;
        public AudioClip ThreeClip;
        public AudioClip TwoClip;
        public AudioClip OneClip;
        public AudioClip BattleClip;

        /// <summary>
        /// Invoked by the GameManager when the state of the game changes.
        /// </summary>
        /// <param name="oldState">The state the GameManager has changed 
        /// from.</param>
        /// <param name="newState">The state the GameManager has changed to.</param>
        public delegate void GameStateChanged(GameState oldState, GameState newState);

        public event GameStateChanged gameStateChanged;

        /// <summary>
        /// Game State Property. Behaviours of all objects in the game change
        /// based on which state the game is in.
        /// </summary>
        public GameState State
        {
            get
            {
                return state;
            }
            set
            {
                gameStateChanged?.Invoke(state, value);
                state = value;
            }
        }

        /// <summary>
        /// Tracks the game's currnet state. Behaviours of all objects in the 
        /// game change based on which state the game is in.
        /// </summary>
        private GameState state;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.sceneUnloaded += OnSceneUnload;
        }

        private void Start()
        {
            GameManagerMusicAudio.loop = true;
            GameManagerMusicAudio.Play();
            GameCountdownGroup.alpha = 0;
            GameCountdownGroup.blocksRaycasts = false;
            GameCountdownGroup.interactable = false;
            State = GameState.Default;
            LoadingScreen.HideLoadingScreenGroup();
            LoadScene("Title");
        }

        public void ShowHowToPlay()
        {
            HowToPlayHandler.ShowHowToPlayGroup();
        }

        public void EndGame(bool isWin)
        {
            StartCoroutine(EndGameRoutine(isWin));
        }

        /// <summary>
        /// Starts the End Game routine.
        /// </summary>
        public IEnumerator EndGameRoutine(bool isWin)
        {
            State = GameState.Postgame;
            Time.timeScale = 0.35f;
            yield return new WaitForSeconds(1.2f);
            Time.timeScale = 1f;
            if (isWin)
            {
                yield return TransitionToScene("WinScene", true, true, false);
            }
            else
            {
                yield return TransitionToScene("LoseScene", true, true, false);
            }
        }

        /// <summary>
        /// Loads a new scene with transitions.
        /// </summary>
        /// <param name="sceneName">Scene to be loaded.</param>
        public void LoadScene(string sceneName)
        {
            if (State == GameState.Paused)
            {
                TogglePauseState();
                State = GameState.Pregame;
            }
            string currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName == "Boot" && sceneName == "Title")
            {
                StartCoroutine(TransitionToScene(sceneName, false, true, false));
            }
            else
            {
                StartCoroutine(TransitionToScene(sceneName, true, true, true));
            }

        }

        private void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "Title")
            {
                State = GameState.Pregame;
            }
            if (scene.name == "Main")
            {
                StartCoroutine(StartGameRoutine());
            }
        }

        private IEnumerator StartGameRoutine()
        {
            yield return new WaitForSeconds(FadeOutTime + LoadTime + 2.0f);
            CountdownText.text = "Ready!";
            GameManagerSFXAudio.PlayOneShot(ReadyClip, 1.5f);
            GameCountdownGroup.alpha = 1;
            yield return new WaitForSeconds(1.0f);
            CountdownText.text = "3...";
            GameManagerSFXAudio.PlayOneShot(ThreeClip, 1.5f);
            yield return new WaitForSeconds(1.0f);
            CountdownText.text = "2...";
            GameManagerSFXAudio.PlayOneShot(TwoClip, 1.5f);
            yield return new WaitForSeconds(1.0f);
            CountdownText.text = "1...";
            GameManagerSFXAudio.PlayOneShot(OneClip, 1.5f);
            yield return new WaitForSeconds(1.0f);
            CountdownText.text = "Battle!";
            GameManagerSFXAudio.PlayOneShot(BattleClip, 1.5f);
            State = GameState.Running;
            yield return new WaitForSeconds(0.5f);
            GameCountdownGroup.alpha = 0;
        }

        private void HidePauseGroup()
        {
            PauseGroup.alpha = 0;
            PauseGroup.interactable = false;
            PauseGroup.blocksRaycasts = false;
        }

        private void ShowPauseGroup()
        {
            PauseGroup.alpha = 1;
            PauseGroup.interactable = true;
            PauseGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Pauses/Unpauses the game based on current game state.
        /// </summary>
        public void TogglePauseState()
        {
            if (State == GameState.Paused)
            {
                HidePauseGroup();
                Time.timeScale = 1.0f;
                State = GameState.Running;
            }
            else
            {
                if (State != GameState.Running)
                {
                    // Can only pause if game is running.
                    return;
                }
                else
                {
                    ShowPauseGroup();
                    Time.timeScale = 0.0f;
                    State = GameState.Paused;
                }
            }
        }

        private void OnSceneUnload(Scene scene)
        {
            if (scene.name == "Main")
            {
                gameStateChanged = null;
            }
        }

        /// <summary>
        /// Shuts down the application. Called from a UI button.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }

        /// <summary>
        /// Shows LoadingScreen for the given LoadTime.
        /// </summary>
        private IEnumerator ShowLoadingScreen()
        {
            LoadingScreen.ShowLoadingScreenGroup();
            yield return SceneFader.FadeIn(FadeInTime);
            yield return new WaitForSeconds(LoadTime);
            yield return SceneFader.FadeOut(FadeOutTime);
            LoadingScreen.HideLoadingScreenGroup();
        }

        /// <summary>
        /// Fades out the current scene, loads the new scene, then fades the 
        /// new scene in.
        /// </summary>
        /// <param name="sceneName">Name of scene to transition to.</param>
        private IEnumerator TransitionToScene(string sceneName, bool fadeOut,
            bool fadeIn, bool showLoadingScreen)
        {
            if (fadeOut)
            {
                StartCoroutine(FadeOutAudio());
                yield return SceneFader.FadeOut(FadeOutTime);
            }
            SceneManager.LoadScene(sceneName);
            if (showLoadingScreen)
            {
                yield return ShowLoadingScreen();
            }
            switch (sceneName)
            {
                case "Title":
                    GameManagerMusicAudio.clip = TitleScreenMusic;
                    break;
                case "Main":
                    GameManagerMusicAudio.clip = BattleSceneMusic;
                    break;
                case "WinScene":
                    GameManagerMusicAudio.clip = VictorySceneMusic;
                    break;
                case "LoseScene":
                    GameManagerMusicAudio.clip = DefeatSceneMusic;
                    break;
            }
            if (fadeIn)
            {
                GameManagerMusicAudio.Play();
                StartCoroutine(FadeInAudio());
                yield return SceneFader.FadeIn(FadeInTime);
            }
        }

        public IEnumerator FadeOutAudio()
        {
            float startVol = GameManagerMusicAudio.volume;

            float elapsedTime = 0.0f;

            while (elapsedTime < FadeOutTime)
            {
                GameManagerMusicAudio.volume =
                    Mathf.Lerp(startVol, 0.0f, elapsedTime / FadeOutTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        public IEnumerator FadeInAudio()
        {
            float endVol = 1;

            GameManagerMusicAudio.volume = 0;

            float elapsedTime = 0.0f;

            while (elapsedTime < FadeInTime)
            {
                GameManagerMusicAudio.volume =
                    Mathf.Lerp(0.0f, endVol, elapsedTime / FadeInTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }
}
