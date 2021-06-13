using System;
using System.Collections.Generic;
using UnityEngine;

namespace CTE
{
    public class BlockFactory : MonoBehaviour, IBlock
    {
        /* field */
        public BlockType BlockType => BlockType.Factory;
        public int MapX { get; set; }
        public int MapY { get; set; }
        public bool WillDestroy { get => false; set => _ = value; }

        public GameObject BlockFloor;
        public GameObject BlockRed;
        public GameObject BlockGreen;
        public GameObject BlockYellow;
        public GameObject BlockBlue;

        /* ctor */
        public BlockFactory()
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
            throw new NotImplementedException();
        }
    }
}