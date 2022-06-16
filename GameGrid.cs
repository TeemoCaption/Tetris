using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid;  //建立一個二維陣列，一維是行二維是列
        public int Rows { get; } //行(橫的)
        public int Columns { get; } //列(直的)
        public int this[int r, int c]   //建立索引器，能輕鬆訪問行列數組
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool IsInside(int r, int c)   //檢查給定的行列數值是否在網格內
        {
            //當不在網格內，該行必須大於等於0並小於列的行數，但小於列
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        public bool IsEmpty(int r, int c)  //檢查給定單元格是否為空
        {
            //必須在網格內並且該條目的值必須為0
            return IsInside(r, c) && grid[r, c] == 0;
        }

        public bool IsRowFull(int r)   //檢查一行是否已滿
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r,c] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsRowEmpty(int r)   //檢查一行是否為空
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearRow(int r)   //當有完整的行時，清除該行
        {
            for(int c=0; c< Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        private void MoveRowDown(int r,int numRows)  //當前行被清除則將行向下移動
        {
            for(int c = 0;c<Columns;c++)
            {
                grid[r+numRows,c] = grid[r,c];
                grid[r, c] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared = 0;  //從底部開始清除行

            for(int r = Rows - 1; r >= 0; r--)  //由底部向頂部移動
            {
                if (IsRowFull(r))  //檢查當前行是否已滿
                {
                    ClearRow(r);  //如果是則清除 
                    cleared++;
                }
                else if (cleared>0)   //當有清除任何行時，就將當前行向下移動
                {
                    MoveRowDown(r,cleared);
                }   
            }
            return cleared;  //返回清除的行數
        }
    }
}
