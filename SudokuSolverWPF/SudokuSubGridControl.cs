using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SudokuSolverWPF
{
    [TemplatePart(Name = "PART_SudokuSubGrid", Type = typeof(UniformGrid))]
    public class SudokuSubGridControl : Control
    {
        private UniformGrid _uniformGrid;

        public static readonly DependencyProperty ColumnsProperty =
           DependencyProperty.Register(nameof(Columns), typeof(int), typeof(SudokuSubGridControl), new UIPropertyMetadata(null));
        
        public static readonly DependencyProperty RowProperty =
           DependencyProperty.Register(nameof(Rows), typeof(int), typeof(SudokuSubGridControl), new UIPropertyMetadata(null));

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        public int Rows
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }

        public List<SudokuCellControl> Cells
        {
            get { return _uniformGrid.Children.Cast<SudokuCellControl>().ToList(); }
        }

        static SudokuSubGridControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SudokuSubGridControl), new FrameworkPropertyMetadata(typeof(SudokuSubGridControl)));
        }

        public SudokuSubGridControl(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
        }

        public override void OnApplyTemplate()
        {
            _uniformGrid = (UniformGrid)Template.FindName("PART_SudokuSubGrid", this);
            _uniformGrid.Columns = Columns;
            _uniformGrid.Rows = Rows;

            for (int i = 0; i < Columns * Rows; i++)
            {
                _uniformGrid.Children.Add(new SudokuCellControl());
            }
        }
    }
}
