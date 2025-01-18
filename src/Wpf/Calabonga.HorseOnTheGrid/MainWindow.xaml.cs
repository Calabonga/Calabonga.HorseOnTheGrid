using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Calabonga.HorseOntTheGrid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly int _rows = 10, _columns = 10;
        private Image[,] _gridImages;
        private bool _isGameRunning;
        private HorseGame _game;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Space)
            {
                return;
            }

            if (!_isGameRunning)
            {
                _isGameRunning = true;
            }

            RunGame();
        }

        private void RunGame()
        {
            _gridImages = SetupGrid();
            _game = new HorseGame(_rows, _columns);

            DrawGrid();

            PressKey.Visibility = Visibility.Hidden;
        }

        private void DrawGrid()
        {
            for (var x = 0; x < _rows; x++)
            {
                for (var y = 0; y < _columns; y++)
                {
                    var cell = _game.Grid[x, y];
                    _gridImages[x, y].Source = CellHelper.CellToImage[cell];
                    _gridImages[x, y].Tag = $"{x}:{y}";
                }
            }

            Score.Text = $"СЧЁТ: {_game.Score}";
        }

        private Image[,] SetupGrid()
        {
            if (HorseGrid.Children.Count > 0)
            {
                HorseGrid.Children.Clear();
            }

            var images = new Image[_rows, _columns];
            HorseGrid.Rows = _rows;
            HorseGrid.Columns = _columns;

            for (var x = 0; x < _rows; x++)
            {
                for (var y = 0; y < _columns; y++)
                {
                    var grid = new Grid();
                    var textBlock = new TextBlock
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Foreground = new SolidColorBrush(Colors.White),
                        Text = x == 0 & y == 0 ? "1" : "",
                        FontSize = 26,
                        IsHitTestVisible = false
                    };
                    var image = new Image { Source = Images.Empty };
                    images[x, y] = image;
                    grid.Children.Add(image);
                    grid.Children.Add(textBlock);
                    HorseGrid.Children.Add(grid);
                }
            }

            return images;
        }

        private void Window_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled && !_isGameRunning)
            {
                return;
            }

            var image = (Image)e.Source;
            var grid = (Grid)image.Parent;
            var textBlock = grid.Children.OfType<TextBlock>().First();
            var coordinates = image.Tag.ToString()!.Split(':');
            var x = int.Parse(coordinates[0]);
            var y = int.Parse(coordinates[1]);

            _game.MoveHorse(x, y, textBlock);

            if (_game.IsCurrentGameOver)
            {
                Score.Text = $"GAME OVER: {_game.Score}";
                _isGameRunning = false;
                PressKey.Visibility = Visibility.Visible;
            }

            DrawGrid();
        }

        private void Window_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is not Image)
            {
                e.Handled = true;
            }
        }
    }
}
