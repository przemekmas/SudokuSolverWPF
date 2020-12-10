using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace SudokuSolverWPF
{
    /// <summary>
    /// Interaction logic for SudokuWindow.xaml
    /// </summary>
    public partial class SudokuWindow : Window
    {
        private readonly int _rows = 9;
        private readonly int _columns = 9;
        private readonly int _subGridRows = 3;
        private readonly int _subGridColumns = 3;
        private readonly int _subGridCount = 9;
        private readonly SudokuOperations _sudokuOperations = SudokuOperations.Instance;

        private readonly List<List<int>> _initialGridConfiguration = new List<List<int>>
        {
            new List<int>() { 5,3,0,6,0,0,0,9,8 },
            new List<int>() { 0,7,0,1,9,5,0,0,0 },
            new List<int>() { 0,0,0,0,0,0,0,6,0 },
            new List<int>() { 8,0,0,4,0,0,7,0,0 },
            new List<int>() { 0,6,0,8,0,3,0,2,0 },
            new List<int>() { 0,0,3,0,0,1,0,0,6 },
            new List<int>() { 0,6,0,0,0,0,0,0,0 },
            new List<int>() { 0,0,0,4,1,9,0,8,0 },
            new List<int>() { 2,8,0,0,0,5,0,7,9 }
        };

        public SudokuWindow()
        {
            InitializeComponent();
            MainSudokuGrid.Loaded += OnSudokuGridLoadFinished;
        }

        private void OnSudokuGridLoadFinished(object sender, RoutedEventArgs e)
        {
            MainSudokuGrid.LoadConfiguration(_initialGridConfiguration);
        }

        private void OnClearSudokuGrid(object sender, RoutedEventArgs e)
        {
            MainSudokuGrid.ClearConfiguration();
            SolveButton.IsEnabled = true;
        }

        private async void OnSolveClick(object sender, RoutedEventArgs e)
        {
            SolveButton.IsEnabled = false;
            await Task.Run(() =>
            {
                var currentGridConfiguration = new List<List<int>>();

                Dispatcher.Invoke(() => { currentGridConfiguration = MainSudokuGrid.GetCurrentConfiguration(); });
                
                var convertedGridConfiguration = _sudokuOperations.ConvertListTo2DGrid(currentGridConfiguration, _rows, _columns,
                    _subGridColumns);
                var solvedGridArray = _sudokuOperations.Solve(convertedGridConfiguration);
                var solvedGridConfiguration = _sudokuOperations.Convert2DGridToList(solvedGridArray, _columns, _subGridColumns,
                    _subGridRows, _subGridCount);

                Dispatcher.Invoke(() =>
                {
                    MainSudokuGrid.HiglightCells(currentGridConfiguration);
                    MainSudokuGrid.LoadConfiguration(solvedGridConfiguration);
                });
            });
        }
    }
}
