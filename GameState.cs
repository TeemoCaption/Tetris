using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameState
    {
        private Block? currentBlock { get; set; }

        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                //當更新目前方塊時，調用重置方塊方法來設置正確的開始位置和旋轉狀態
                currentBlock.Reset();

                for (int i = 0; i < 2; i++)   //讓方塊在一開始的時候就在可見行(也就是第一行)
                {
                    currentBlock.Move(1, 0);
                    if (!BlockFits())
                    {
                        currentBlock.Move(-1, 0);
                    }
                }
            }
        } 
        public GameGrid GameGrid { get; }   //遊戲網格屬性
        public BlockQueue BlockQueue { get; }  //方塊列
        public bool GameOver { get; private set; }  //是否遊戲結束

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);   //初始化遊戲網格(22行10列的網格)
            BlockQueue = new BlockQueue();   //初始化方塊列，用來獲取下一個產生的隨機方塊
            CurrentBlock = BlockQueue.GetAndUpdate();  //返回下一個方塊並再次更新下一個方塊的屬性
        }

        private bool BlockFits()   //檢查當前方塊是否在正確的位置
        {
            foreach (Position p in CurrentBlock.TilePosition())  //循環遍歷當前方塊位置
            {
                if (!GameGrid.IsEmpty(p.Row, p.Column)) //檢查是否超出「刪除網格」範圍或重疊另一個圖塊
                {
                    return false;
                }
            }
            return true;
        }

        public void RotateBlockCW()  //順時針旋轉當前方塊
        {
            CurrentBlock.RotateCW();
            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();   //若處於無法旋轉的位置，則將他轉回來  
            }
        }

        public void RotateBlockCCW()  //逆時針旋轉當前方塊
        {
            CurrentBlock.RotateCCW();
            if (!BlockFits())
            {
                CurrentBlock.RotateCW();   //若處於無法旋轉的位置，則將他轉回來  
            }
        }

        public void MoveBlockLeft()   //方塊向左移動
        {
            CurrentBlock.Move(0, -1);
            if (!BlockFits())   //如果移動到非法位置，則向右倒退移動
            {
                CurrentBlock.Move(0, 1);
            }
        }

        public void MoveBlockRight()  //方塊向右移動
        {
            CurrentBlock.Move(0, 1);
            if (!BlockFits())   //如果移動到非法位置，則向左倒退移動
            {
                CurrentBlock.Move(0, -1);
            }
        }

        private bool IsGameOver()
        {
            //如果頂部的兩行隱藏行不為空(即填滿)的話，遊戲失敗
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()   //當目前的方塊無法向下移動時(即有一行滿了)
        {
            foreach (Position p in CurrentBlock.TilePosition())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }
            GameGrid.ClearFullRows();  //清除所有已滿的行

            if (IsGameOver())  //檢查是否遊戲結束
            {
                GameOver = true;   //遊戲結束
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();  //更新當前方塊
            }
        }

        public void MoveBlockDown()  //方塊向下移動
        {
            CurrentBlock.Move(1, 0);
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);  //如果移動到非法位置，則向上倒退移動
                PlaceBlock();   //調用此方法，以避免方塊無法向下移動
            }
        }
    }
}
