using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CTE
{
    public class BlockFloor : MonoBehaviour, IBlock
    {
        /* field */
        public BlockType BlockType => BlockType.Factory;
        public int MapX { get; set; }
        public int MapY { get; set; }
        public bool WillDestroy { get => false; set => _ = value; }
        Transform IBlock.transform => transform;

        /* ctor */
        public BlockFloor()
        {
        }

        /* func */

        /* IBlock */
        public void BreakCheck()
        {
        }
        public void NearlyBreakOnce()
        {
        }
        public void NearlyBreakOnce(BlockType blockType)
        {
        }
        public void PlayBreakAnimation()
        {
            throw new System.NotImplementedException();
        }
    }
}