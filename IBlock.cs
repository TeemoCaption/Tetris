﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class IBlock:Block
    {
        //四種旋轉狀態格子位置
        private readonly Position[][] tiles=new Position[][]  
        {
            new Position[]{ new(1,0),new(1,1),new(1,2),new(1,3)},
            new Position[]{ new(0,2),new(1,2),new(2,2),new(3,2)},
            new Position[]{ new(2,0),new(2,1),new(2,2),new(2,3)},
            new Position[]{ new(0,1),new(1,1),new(2,1),new(3,1)}
        };

        public override int Id => 1;
        //起始偏移量(-1,3)，將使方塊生成在頂行的中間
        protected override Position StartOffset => new Position(-1,3);
        protected override Position[][] Tiles => tiles;
    }
}
