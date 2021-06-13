namespace CTE
{
    public class BlockNone : IBlock
    {
        /* field */
        public BlockType BlockType => BlockType.None;
        public int MapX { get; set; }
        public int MapY { get; set; }
        public bool WillDestroy { get => false; set => _ = value; }

        /* ctor */
        public BlockNone()
        {
        }

        /* IBlock */

        /* func */
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