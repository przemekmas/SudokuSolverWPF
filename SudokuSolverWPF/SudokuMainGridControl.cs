using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SudokuSolverWPF
{
    [TemplatePart(Name = "PART_SudokuMainGrid", Type = typeof(UniformGrid))]
    public class SudokuMainGridControl : Control
    {
        private UniformGrid _uniformGrid;

        public static readonly DependencyProperty ColumnsProperty =
           DependencyProperty.Register(nameof(Columns), typeof(int), typeof(SudokuMainGridControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty RowsProperty =
           DependencyProperty.Register(nameof(Rows), typeof(int), typeof(SudokuMainGridControl), new UIPropertyMetadata(null));
        
        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        static SudokuMainGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SudokuMainGridControl), new FrameworkPropertyMetadata(typeof(SudokuMainGridControl)));
        }

        public override void OnApplyTemplate()
        {
            _uniformGrid = (UniformGrid)Template.FindName("PART_SudokuMainGrid", this);
            _uniformGrid.Columns = Columns;
            _uniformGrid.Rows = Rows;

            for (var cell = 0; cell < Rows * Columns; cell++)
            {
                _uniformGrid.Children.Add(new SudokuSubGridControl(3, 3));
            }
        }

        public List<List<int>> GetCurrentConfiguration()
        {
            var localGrid = new List<List<int>>();

            foreach (var grid in _uniformGrid.Children)
            {
                if (grid is SudokuSubGridControl subGrid)
                {
                    var localSubGrid = new List<int>();

                    foreach (var cell in subGrid.Cells)
                    {
                        if (int.TryParse(cell.Text, out int result))
                        {
                            localSubGrid.Add(result);
                        }
                        else
                        {
                            localSubGrid.Add(0);
                        }
                    }
                    localGrid.Add(localSubGrid);
                }
            }
            return localGrid;
        }
        
        public void HiglightCells(List<List<int>> clues)
        {
            var subGridIndex = 0;
            foreach (var grid in _uniformGrid.Children)
            {
                var gridConfiguration = clues[subGridIndex];

                if (grid is SudokuSubGridControl subGrid)
                {
                    var cellsList = subGrid.Cells;

                    for (int cell = 0; cell < cellsList.Count; cell++)
                    {
                        if (gridConfiguration[cell] > 0)
                        {
                            cellsList[cell].TextForeground = new SolidColorBrush(Color.FromRgb(215, 10, 10));
                            cellsList[cell].TextBoxBackground = new SolidColorBrush(Color.FromRgb(186, 186, 186));
                        }
                    }
                    subGridIndex++;
                }
            }
        }

        public void LoadConfiguration(List<List<int>> gridConfiguration)
        {
            var subGridIndex = 0;
            foreach (var grid in _uniformGrid.Children)
            {
                var localGridConfiguration = gridConfiguration[subGridIndex];

                if (grid is SudokuSubGridControl subGrid)
                {
                    var cellsList = subGrid.Cells;

                    for (int cell = 0; cell < cellsList.Count; cell++)
                    {
                        if (localGridConfiguration[cell] > 0)
                        {
                            cellsList[cell].Text = localGridConfiguration[cell].ToString();
                        }
                        else
                        {
                            cellsList[cell].Text = string.Empty;
                        }
                    }
                    subGridIndex++;
                }
            }
        }

        public void ClearConfiguration()
        {
            foreach (var grid in _uniformGrid.Children.Cast<SudokuSubGridControl>())
            {
                foreach (var cell in grid.Cells)
                {
                    cell.Text = string.Empty;
                    cell.TextForeground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    cell.TextBoxBackground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                }
            }
        }
    }
}
