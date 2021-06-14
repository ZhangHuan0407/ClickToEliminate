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
        public IBlock CreateBlock(BlockType blockType)
        {
            if (blockType == BlockType.Color)
                switch (UnityEngine.Random.Range(0, 3))
                {
                    case 0:
                        blockType = BlockType.Red;
                        break;
                    case 1:
                        blockType = BlockType.Green;
                        break;
                    case 2:
                        blockType = BlockType.Yellow;
                        break;
                    case 3:
                        blockType = BlockType.Blue;
                        break;
                }

            switch (blockType)
            {
                case BlockType.Red:
                    return Instantiate(BlockRed).GetComponent<IBlock>();
                case BlockType.Green:
                    return Instantiate(BlockGreen).GetComponent<IBlock>();
                case BlockType.Yellow:
                    return Instantiate(BlockYellow).GetComponent<IBlock>();
                case BlockType.Blue:
                    return Instantiate(BlockBlue).GetComponent<IBlock>();
                //case BlockType.RocketVertical:
                //    break;
                //case BlockType.RocketHorizontal:
                //    break;
                case BlockType.None:
                    return new BlockNone();
                case BlockType.Floor:
                    return Instantiate(BlockFloor).GetComponent<IBlock>();
                // case BlockType.Color:
                // case BlockType.Factory:
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

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