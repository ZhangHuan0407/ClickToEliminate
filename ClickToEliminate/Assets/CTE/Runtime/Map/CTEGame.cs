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
        /// <summary>
        /// 地图位置
        /// </summary>
        public static Transform MapTransform { get; private set; }

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

            MapTransform = transform;

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
            MapTransform = null;
            GameData.SaveOut();
            SceneManager.LoadScene("SelectionScene", LoadSceneMode.Single);
        }

        public static void TryRunOnce(IBlock block)
        {
            LogicTweener logicTweener = new LogicTweener();
            logicTweener.SetLogic(GameCheck(logicTweener, block));
            var node = GameAnimation.AddLast(logicTweener);

            LogicTweener createAndShiftBlockTweener = new LogicTweener();
            createAndShiftBlockTweener.SetLogic(CreateAndShiftBlock(createAndShiftBlockTweener));
            node = GameAnimation.AddAfter(node, createAndShiftBlockTweener);

            LogicTweener unlockTweener = new LogicTweener();
            unlockTweener.SetLogic(Unlock(unlockTweener));
            GameAnimation.AddAfter(node, unlockTweener);

            AllowClick = false;
            GameAnimation.First.Value.DoIt();
        }
        /// <summary>
        /// 检测牌面上标记为摧毁的砖块，触发并将动画指令入栈
        /// </summary>
        public static IEnumerator<Tweener> GameCheck(LogicTweener logicTweener, IBlock pointerBlock)
        {
            if (logicTweener is null)
                throw new ArgumentNullException(nameof(logicTweener));
            if (pointerBlock is null)
                throw new ArgumentNullException(nameof(pointerBlock));

            pointerBlock.BreakCheck();

            List<Vector2Int> breakPositions = new List<Vector2Int>();
            for (int indexY = 0; indexY < MapHeight; indexY++)
                for (int indexX = 0; indexX < MapWidth; indexX++)
                {
                    IBlock block = GameData.Blocks[indexX, indexY];
                    if (block.WillDestroy)
                    {
                        block.PlayBreakAnimation();
                        breakPositions.Add(new Vector2Int(indexX, indexY));
                        yield return TimeTween.DoTime(0.05f);
                    }
                }

            yield return TimeTween.DoTime(0.3f);

            foreach (Vector2Int breakPosition in breakPositions)
            {
                GameData.Blocks[breakPosition.x, breakPosition.y] = null;
                GameData.BlockTypes[breakPosition.x, breakPosition.y] = BlockType.None;
            }

            GameAnimation.RemoveFirst();
            if (GameAnimation.Count > 0)
                GameAnimation.First.Value.DoIt();

            yield break;
        }

        /// <summary>
        /// 扫描空砖块位置，创建并位移上侧的砖块。随后重复一次当前行为，直至满场。
        /// </summary>
        public static IEnumerator<Tweener> CreateAndShiftBlock(LogicTweener logicTweener)
        {
            if (logicTweener is null)
                throw new ArgumentNullException(nameof(logicTweener));

            bool waitAndDelayInvoke = false;
            for (int indexX = 0; indexX < MapWidth; indexX++)
            {
                for (int indexY = 0; indexY < MapHeight; indexY++)
                {
                    if (GameData.BlockTypes[indexX, indexY] != BlockType.None)
                        continue;

                    for (int searchY = indexY + 1; searchY < MapHeight; searchY++)
                    {
                        switch (GameData.BlockTypes[indexX, searchY])
                        {
                            case BlockType.None:
                                continue;
                            case BlockType.Factory:
                                BlockFactory blockFactory = GameData.Blocks[indexX, searchY] as BlockFactory;
                                IBlock block = blockFactory.CreateBlock(BlockType.Color);
                                block.transform.SetParent(MapTransform, false);
                                block.transform.position = blockFactory.transform.position;
                                block.MapX = indexX;
                                block.MapY = indexY;
                                GameData.Blocks[indexX, indexY] = block;
                                GameData.BlockTypes[indexX, indexY] = block.BlockType;
                                Vector2 targetPosition = BlockWorldPosition[new Vector2Int(indexX, indexY)];
                                block.transform.DoPosition(
                                    targetPosition,
                                    (targetPosition - (Vector2)block.transform.position).magnitude * 0.3f)
                                    .DoIt();
                                waitAndDelayInvoke = true;
                                goto AfterColumnGetBlock;
                            case BlockType.Floor:
                                throw new Exception("I don't know how to handle it.");
                            //case BlockType.RocketVertical:
                            //case BlockType.RocketHorizontal:
                            //case BlockType.Color:
                            //case BlockType.Red:
                            //case BlockType.Green:
                            //case BlockType.Yellow:
                            //case BlockType.Blue:
                            default:
                                IBlock block2 = GameData.Blocks[indexX, indexY] = GameData.Blocks[indexX, searchY];
                                GameData.BlockTypes[indexX, indexY] = GameData.BlockTypes[indexX, searchY];
                                GameData.Blocks[indexX, searchY] = null;
                                GameData.BlockTypes[indexX, searchY] = BlockType.None;
                                Vector2 targetPosition2 = BlockWorldPosition[new Vector2Int(indexX, indexY)];
                                block2.MapX = indexX;
                                block2.MapY = indexY;
                                block2.transform.DoPosition(
                                    targetPosition2,
                                    (targetPosition2 - (Vector2)block2.transform.position).magnitude * 0.3f)
                                    .DoIt();
                                waitAndDelayInvoke = true;
                                goto AfterColumnGetBlock;
                        }
                    }
                }
            AfterColumnGetBlock:
                ;
            }

            if (waitAndDelayInvoke)
            {
                yield return TimeTween.DoTime(0.2f);
                LogicTweener createAndShiftBlockTweener = new LogicTweener();
                createAndShiftBlockTweener.SetLogic(CreateAndShiftBlock(createAndShiftBlockTweener));
                GameAnimation.AddAfter(GameAnimation.First, createAndShiftBlockTweener);
            }

            GameAnimation.RemoveFirst();
            if (GameAnimation.Count > 0)
                GameAnimation.First.Value.DoIt();

            yield break;
        }

        /// <summary>
        /// 界面解锁
        /// </summary>
        public static IEnumerator<Tweener> Unlock(LogicTweener logicTweener)
        {
            if (logicTweener is null)
                throw new ArgumentNullException(nameof(logicTweener));

            AllowClick = true;

            GameAnimation.RemoveFirst();
            if (GameAnimation.Count > 0)
                GameAnimation.First.Value.DoIt();

            yield break;
        }
    }
}