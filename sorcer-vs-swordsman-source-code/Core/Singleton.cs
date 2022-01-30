using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// A base class for Singletons. All inheriting classes will follow the 
    /// Singleton pattern.
    /// </summary>
    /// <typeparam name="T">Type of singleton class (i.e. GameManager)</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary>
        /// Singular reference to the singleton.
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Tracks whether the singleton has been instantiated.
        /// </summary>
        public static bool IsInitialized => Instance != null;

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("[Singleton.cs] Trying to instantiate a second" +
                        "instance of a singleton class.");
            }
            else
            {
                Instance = (T)this;
            }
        }

        protected void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }

}

