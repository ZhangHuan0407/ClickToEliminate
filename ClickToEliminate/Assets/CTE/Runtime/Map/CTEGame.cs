using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tween;
using UnityEngine.SceneManagement;

namespace CTE
{
    public class CTEGame : MonoBehaviour
    {
        /* field */
        public static LinkedList<Tweener> GameAnimation;
        
        /* func */
        public static void StartNewGame(int levelIndex)
        {
            MapData map = GameData.Map = GameData.MapConfig[levelIndex];
            GameData.Blocks = new IBlock[map.Blocks.GetLength(0), map.Blocks.GetLength(1)];
            GameData.BlockTypes = new BlockType[map.Blocks.GetLength(0), map.Blocks.GetLength(1)];
            GameAnimation = new LinkedList<Tweener>();
        }
        public static void WinGame()
        {
            int levelIndex = GameData.Map.LevelIndex;
            GameData.PlayerRecord.LevelDone[levelIndex] = true;
        }
        public static void ExitGame()
        {
            GameAnimation.Clear();
            GameData.SaveOut();
            SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
        }


    }
}