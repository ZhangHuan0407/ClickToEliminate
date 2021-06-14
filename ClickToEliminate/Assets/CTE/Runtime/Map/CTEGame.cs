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

        public GameObject BlockFactory;

        public BlockFactory CommonFactory { get; private set; }
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }

        /* ctor */
        private void Start()
        {
            CommonFactory = Instantiate(BlockFactory, transform).GetComponent<BlockFactory>();
            CommonFactory.transform.position = new Vector3(-100f, 0f, 0f);

            MapWidth = GameData.Map.Blocks.GetLength(1);
            MapHeight = GameData.Map.Blocks.GetLength(0);

            // 初始牌面
            for (int indexY = 0; indexY < MapHeight; indexY++)
                for (int indexX = 0; indexX < MapWidth; indexX++)
                {
                    // 配置表视觉坐标转换为场景坐标
                    BlockType blockType = GameData.Map.Blocks[MapHeight - indexY - 1, indexX];
                    GameData.BlockTypes[indexX, indexY] = blockType;
                    IBlock block;
                    if (blockType == BlockType.Factory)
                        block = GameData.Blocks[indexX, indexY] = Instantiate(BlockFactory).GetComponent<IBlock>();
                    else
                        block = GameData.Blocks[indexX, indexY] = CommonFactory.CreateBlock(blockType);
                    if (block.transform is RectTransform rectTransform)
                        rectTransform.anchoredPosition = GameData.Map.GetBlockAnchoredPosition(indexX, indexY);
                    if (block.transform != null)
                        block.transform.SetParent(transform, false);
                }
        }

        /* func */
        public static void StartNewGame(int levelIndex)
        {
            GameAnimation = new LinkedList<Tweener>();
            MapData map = GameData.Map = GameData.MapConfig[levelIndex];
            GameData.Blocks = new IBlock[map.Blocks.GetLength(1), map.Blocks.GetLength(0)];
            GameData.BlockTypes = new BlockType[map.Blocks.GetLength(1), map.Blocks.GetLength(0)];
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