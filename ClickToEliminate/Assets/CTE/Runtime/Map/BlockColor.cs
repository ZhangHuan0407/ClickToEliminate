using UnityEngine;
using Tween;

namespace CTE
{
    public class BlockColor : MonoBehaviour, IBlock
    {
        /* field */
        public BlockType ColorType = BlockType.Color;
        public BlockType BlockType => ColorType;

        public int MapX { get; set; }
        public int MapY { get; set; }
        public bool WillDestroy { get; set; }
        Transform IBlock.transform => transform;

        /* ctor */
        public BlockColor()
        {
        }

        /* func */
        public void OnClickBlockColorButton()
        {
            if (!CTEGame.AllowClick)
            {
                Debug.Log($"{nameof(CTEGame.AllowClick)} => {CTEGame.AllowClick}");
                return;
            }

            LogicTweener logicTweener = new LogicTweener();
            logicTweener.SetLogic(CTEGame.GameCheck(logicTweener, this));
            CTEGame.GameAnimation.AddLast(logicTweener);
            CTEGame.TryRunOnce();
        }

#if UNITY_EDITOR
        private void Update()
        {
            Debug.DrawLine(transform.position, CTEGame.BlockWorldPosition[new Vector2Int(MapX, MapY)], Color.green);
        }
#endif

        /* IBlock */
        public void BreakCheck()
        {
            bool[,] haveVisit = new bool[CTEGame.MapWidth, CTEGame.MapHeight];
            int count = 0;
            BlockType blockType = GameData.BlockTypes[MapX, MapY];
            CheckType(MapX, MapY);
            if (count > 1)
                NearlyBreakOnce(blockType);

            void CheckType(int x, int y)
            {
                if (x < 0 || x >= CTEGame.MapWidth
                    || y < 0 || y >= CTEGame.MapHeight
                    || haveVisit[x, y])
                    return;

                haveVisit[x, y] = true;
                if (GameData.BlockTypes[x, y] == blockType)
                {
                    count++;
                    if (count > 1)
                        GameData.Blocks[x, y].NearlyBreakOnce(blockType);
                    CheckType(x - 1, y);
                    CheckType(x + 1, y);
                    CheckType(x, y - 1);
                    CheckType(x, y + 1);
                }
            }
        }
        public void NearlyBreakOnce(BlockType blockType)
        {
            WillDestroy = true;
        }
        public void PlayBreakAnimation()
        {
            // if Task, then fly
            // else
            transform.DoLocalScale(Vector3.zero, 0.5f)
                .DoIt()
                .OnComplete_Handle += () => Destroy(gameObject);
        }
    }
}