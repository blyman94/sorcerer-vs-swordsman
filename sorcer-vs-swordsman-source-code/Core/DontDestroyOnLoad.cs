using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
			DontDestroyOnLoad(gameObject);
        }
    }
}

