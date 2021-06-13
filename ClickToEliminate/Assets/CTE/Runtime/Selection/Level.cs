using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CTE
{
    public class Level : MonoBehaviour
    {
        /* field */
        public int LevelIndex = -1;
        public Text LevelText;
        public Image Done;
        public Button LevelButton;

        /* func */
        public void Refresh()
        {
            bool haveLevel = LevelIndex >= 0;
            gameObject.SetActive(haveLevel);
            if (haveLevel)
            {
                LevelText.text = $"{LevelIndex + 1}";
                Done.gameObject.SetActive(GameData.PlayerRecord.IsDone(LevelIndex));
            }
        }

        public void OnClickLevelButton()
        {
            if (LevelIndex < 0)
                return;
            GetComponentInParent<SectionView>().SelectLevel(LevelIndex);
        }
    }
}