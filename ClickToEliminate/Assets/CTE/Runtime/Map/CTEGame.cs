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

        /// <summary>
        /// 砖块工厂对应的预制体资源
        /// </summary>
        public GameObject BlockFactory;

        /// <summary>
        /// 默认砖块工厂对应的场景引用
        /// </summary>
        public BlockFactory CommonFactory { get; private set; }
        [SerializeField]
        private GameObject DebugGo;

        public static int MapWidth { get; private set; }
        public static int MapHeight { get; private set; }
        /// <summary>
        /// 当前允许用户点击
        /// </summary>
        public static bool AllowClick { get; set; }
        public static Dictionary<Vector2Int, Vector2> BlockWorldPosition { get; private set; }

        /* ctor */
        private void Start()
        {
            CommonFactory = Instantiate(BlockFactory, transform).GetComponent<BlockFactory>();
            CommonFactory.transform.position = new Vector3(-100f, 0f, 0f);

            MapWidth = GameData.Map.Blocks.GetLength(1);
            MapHeight = GameData.Map.Blocks.GetLength(0);

            AllowClick = true;

            BlockWorldPosition = new Dictionary<Vector2Int, Vector2>();
            for (int indexY = 0; indexY < MapHeight; indexY++)
                for (int indexX = 0; indexX < MapWidth; indexX++)
                {
                    (DebugGo.transform as RectTransform).anchoredPosition = GameData.Map.GetBlockAnchoredPosition(indexX, indexY);
                    BlockWorldPosition.Add(new Vector2Int(indexX, indexY), DebugGo.transform.position);
                }

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
                    // Color => Green || Red...
                    GameData.BlockTypes[indexX, indexY] = block.BlockType;
                    block.MapX = indexX;
                    block.MapY = indexY;
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
            GameData.Blocks = null;
            BlockWorldPosition = null;
            GameData.SaveOut();
            SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
        }

        public static void TryRunOnce()
        {
            if (GameAnimation.Count > 0
                && GameAnimation.First.Value.State == TweenerState.Finish)
                GameAnimation.RemoveFirst();

            if (GameAnimation.Count > 0)
                GameAnimation.First.Value.DoIt();
        }
        /// <summary>
        /// 检测牌面上标记为摧毁的砖块，触发并将动画指令入栈
        /// </summary>
        public static IEnumerator<Tweener> GameCheck(LogicTweener tweener, IBlock pointerBlock)
        {
            if (tweener is null)
                throw new ArgumentNullException(nameof(tweener));
            if (pointerBlock is null)
                throw new ArgumentNullException(nameof(pointerBlock));

            AllowClick = false;
            pointerBlock.BreakCheck();

            for (int indexY = 0; indexY < MapHeight; indexY++)
                for (int indexX = 0; indexX < MapWidth; indexX++)
                {
                    IBlock block = GameData.Blocks[indexX, indexY];
                    if (block.WillDestroy)
                    {
                        block.PlayBreakAnimation();
                        yield return TimeTween.DoTime(0.2f);
                    }
                }

            yield return TimeTween.DoTime(0.5f);

            // block down animation

            GameAnimation.RemoveFirst();
            if (GameAnimation.Count > 0)
                GameAnimation.First.Value.DoIt();

            AllowClick = true;
            yield break;
        }
    }
}