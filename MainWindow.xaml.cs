using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]   //建立方塊像素數組
        {
            new BitmapImage(new Uri("UI/TileEmpty.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/TileCyan.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/TileBlue.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/TileOrange.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/TileYellow.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/TileGreen.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/TilePurple.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/TileRed.png",UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]   //建立方塊圖片數組
        {
            new BitmapImage(new Uri("UI/Block-Empty.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/Block-I.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/Block-J.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/Block-L.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/Block-O.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/Block-S.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/Block-T.png",UriKind.Relative)),
            new BitmapImage(new Uri("UI/Block-Z.png",UriKind.Relative))
        };

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }

        private GameState gameState = new GameState();
        private readonly Image[,] imageControls;
        private  readonly int maxDelay =1000;
        private readonly int minDelay =300;
        private readonly int delayDecrease = 10;

        private Image[,] SetupGameCanvas(GameGrid grid)  //設置畫布網格圖片
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;  //每個方格25像素
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)   //寬度250像素，高度500像素
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,   //設置網格大小
                        Height = cellSize
                    };
                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10); //畫出網格
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)    //繪製網格
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];  //獲取方格起始id
                    imageControls[r, c].Source = tileImages[id];
                    imageControls[r, c].Opacity = 1;
                }
            }
        }

        private void DrawBlock(Block block)   //繪製方塊在網格
        {
            foreach (Position p in block.TilePosition())
            {
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
                imageControls[p.Row, p.Column].Opacity = 1;
            }
        }

        private void DrawHoldBlock(Block heldBlock)   //畫上保存的方塊圖片
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];   //保存當前方塊
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];   //替換方塊
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);   //畫下一個方塊
            DrawHoldBlock(gameState.HeldBlock);
            DrawGhostBlock(gameState.CurrentBlock);
            ScoreText.Text = $"目前分數：{gameState.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    gameState.RotateBlockCW();
                    break;
                case Key.Z:    //逆時鐘旋轉
                    gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    gameState.HoldBlock();   //保存方塊
                    break;
                case Key.Space:
                    gameState.DropBlock();  //快速掉落方塊
                    break;
                default:
                    return;
            }
            Draw(gameState);
        }

        private async Task GameLoop()   //一直掉方塊
        {
            Draw(gameState);
            
            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(delay);   //建立在指定的毫秒數之後完成的工作
                gameState.MoveBlockDown();
                Draw(gameState);
            }
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text= $"最終你的分數是：{gameState.Score}";
        }

        private void DrawNextBlock(BlockQueue blockQueue)   //預覽下一個方塊
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)   //畫布載入就畫上網格
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;  //將暫停頁面隱藏
            await GameLoop();
        }

        private void DrawGhostBlock(Block block)  //畫幽靈方塊
        {
            int dropDistance = gameState.BlockDropDistance();
            //通過下降距離添加到當前方塊位置來找到方塊將要降落的方格
            foreach (Position p in block.TilePosition())  
            {
                imageControls[p.Row+dropDistance,p.Column].Opacity=0.25;   //設置不透明度
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }
    }
}
