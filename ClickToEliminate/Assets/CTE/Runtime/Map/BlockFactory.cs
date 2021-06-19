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
        Transform IBlock.transform => transform;

        [SerializeField]
        private GameObject m_BlockFloor;
        [SerializeField]
        private GameObject m_BlockRed;
        [SerializeField]
        private GameObject m_BlockGreen;
        [SerializeField]
        private GameObject m_BlockYellow;
        [SerializeField]
        private GameObject m_BlockBlue;

        /* ctor */
        private void Reset()
        {
            m_BlockFloor = null;
            m_BlockRed = null;
            m_BlockGreen = null;
            m_BlockYellow = null;
            m_BlockBlue = null;
        }
        public BlockFactory()
        {
        }

        /* func */
        public IBlock CreateBlock(BlockType blockType)
        {
            if (blockType == BlockType.Color)
                switch (UnityEngine.Random.Range(0, 4))
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
                    return Instantiate(m_BlockRed).GetComponent<IBlock>();
                case BlockType.Green:
                    return Instantiate(m_BlockGreen).GetComponent<IBlock>();
                case BlockType.Yellow:
                    return Instantiate(m_BlockYellow).GetComponent<IBlock>();
                case BlockType.Blue:
                    return Instantiate(m_BlockBlue).GetComponent<IBlock>();
                //case BlockType.RocketVertical:
                //    break;
                //case BlockType.RocketHorizontal:
                //    break;
                case BlockType.None:
                    return new BlockNone();
                case BlockType.Floor:
                    return Instantiate(m_BlockFloor).GetComponent<IBlock>();
                // case BlockType.Color:
                // case BlockType.Factory:
                default:
                    throw new NotImplementedException();
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