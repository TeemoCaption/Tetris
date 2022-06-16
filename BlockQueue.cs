using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BlockQueue
    {
        //負責在遊戲中選擇將包含的下一個方塊，共有七種方塊
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock(),
        }; 
        private readonly Random random = new Random();
        public Block NextBlock { get; private set; }   //下一個方塊的屬性
        
        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock()   //隨機產生下一個方塊
        {
            return blocks[random.Next(blocks.Length)];
        }

        public Block GetAndUpdate()  //返回下一個方塊並再次更新下一個方塊的屬性
        {
            Block block = NextBlock;
            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);  //不斷隨機產生下一個方塊

            return block;
        }
    }
}
