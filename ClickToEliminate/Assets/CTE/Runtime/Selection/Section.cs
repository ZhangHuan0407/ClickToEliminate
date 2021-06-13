using System;
using System.Collections.Generic;
using UnityEngine;

namespace CTE
{
    public class Section : MonoBehaviour
    {
        /* field */
        public int SectionIndex;

        public Level[] Levels;

        /* func */
        public void Refresh()
        {
            SectionData sectionData = GameData.SectionConfig[SectionIndex];
            for (int index = 0; index < Levels.Length; index++)
            {
                Level level = Levels[index];
                if (index < sectionData.LevelCount)
                    level.LevelIndex = sectionData.LevelIndex[index];
                else
                    level.LevelIndex = -1;
                level.Refresh();
            }
        }
        public void SetInteractable(bool state)
        {
            foreach (Level level in Levels)
                level.LevelButton.interactable = state;
        }
    }
}