using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Block   //抽象類
    {
        //紀錄四個旋轉狀態中的方塊位置
        protected abstract Position[][] Tiles { get; }  
        //紀錄一個方塊的起始偏移量，決定方塊在網格中產生的位置
        protected abstract Position StartOffset { get; }
        public abstract int Id { get; }  //區分不同類型的方塊

        private int rotationState;   //當前旋轉狀態
        private Position offset;   //當前偏移量

        public Block()
        {
            //設定一開始的偏移量
            offset = new Position(StartOffset.Row,StartOffset.Column);   
        }

        //目前旋轉和偏移的方塊所占用的網格位置，IEnumerable是介面
        public IEnumerable<Position> TilePosition() { 
            //循環當前旋轉狀態下方塊位置並添加偏移行和列
            foreach(Position p in Tiles[rotationState])    
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        public void RotateCW()  //順時鐘旋轉90度
        {
            rotationState = (rotationState +1) % Tiles.Length;
        }   

        public void RotateCCW()   //逆時鐘旋轉
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length -1;
            }
            else
            {
                rotationState--;
            }
        }


        public void Move(int rows,int column)
        {
            offset.Row += rows;
            offset.Column += column;
        }

        public void Reset()   //重置旋轉
        {
            rotationState=0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
