using System;
using System.Collections.Generic;
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
        public void OnClickBlockColorButton() { }

        /* IBlock */
        public void BreakCheck()
        {

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