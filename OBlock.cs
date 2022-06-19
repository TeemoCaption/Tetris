using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class OBlock:Block   //每個旋轉狀態下的固定(中心)位置
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[]{ new(0,0),new(0,1),new(1,0),new(1,1)}
        };
        public override int Id => 4;
        protected override Position StartOffset => new Position(0, 4);
        protected override Position[][] Tiles => tiles;
    }
}
