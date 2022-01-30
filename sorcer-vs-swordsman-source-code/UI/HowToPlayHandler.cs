using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HowToPlayHandler : MonoBehaviour
    {
        public CanvasGroup HowToPlayGroup;
        public CanvasGroup[] Subgroups;

        private int currentScreen = 0;

        private void Start()
        {
			HideHowToPlayGroup();
        }

        public void ShowHowToPlayGroup()
        {
            HowToPlayGroup.alpha = 1;
            HowToPlayGroup.interactable = true;
            HowToPlayGroup.blocksRaycasts = true;
            for (int i = 0; i < Subgroups.Length; i++)
            {
                HideSubGroup(i);
            }
            currentScreen = 0;
            ShowSubGroup(currentScreen);
        }

        public void HideHowToPlayGroup()
        {
            HowToPlayGroup.alpha = 0;
            HowToPlayGroup.interactable = false;
            HowToPlayGroup.blocksRaycasts = false;
        }

        public void NextGroup()
        {
            HideSubGroup(currentScreen);
            currentScreen += 1;
            ShowSubGroup(currentScreen);
        }

        public void PreviousGroup()
        {
            HideSubGroup(currentScreen);
            currentScreen -= 1;
            ShowSubGroup(currentScreen);
        }

        public void ShowSubGroup(int subGroupIndex)
        {
            Subgroups[subGroupIndex].alpha = 1;
            Subgroups[subGroupIndex].interactable = true;
            Subgroups[subGroupIndex].blocksRaycasts = true;
        }

        public void HideSubGroup(int subGroupIndex)
        {
            Subgroups[subGroupIndex].alpha = 0;
            Subgroups[subGroupIndex].interactable = false;
            Subgroups[subGroupIndex].blocksRaycasts = false;
        }
    }
}

