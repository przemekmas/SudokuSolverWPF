using System.Collections.Generic;
using System.Linq;

namespace SudokuSolverWPF
{
    public class SudokuOperations
    {
        private static int _subGridSize = 3;
        private static SudokuOperations _instance;

        public static SudokuOperations Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SudokuOperations();
                }
                return _instance;
            }
        }

        public List<List<int>> Convert2DGridToList(int[,] grid, int columns, int subGridColumns, int subGridRows, int subGridCount)
        {
            var localList = new List<List<int>>();
            var currentRowIndex = 0;
            var currentColIndex = 0;

            for (int subGrid = 0; subGrid < subGridCount; subGrid++)
            {
                var subGridList = new List<int>();

                if (currentColIndex == columns)
                {
                    currentColIndex = 0;
                }

                if (subGrid != 0 && subGrid % subGridRows == 0)
                {
                    currentRowIndex += subGridRows;
                }
                var rowIndex = currentRowIndex;
                var colIndex = currentColIndex;

                for (int row = 0; row < subGridRows; row++)
                {
                    for (int col = 0; col < subGridColumns; col++)
                    {
                        subGridList.Add(grid[rowIndex, colIndex]);

                        colIndex++;
                    }
                    colIndex = currentColIndex;
                    rowIndex++;
                }

                localList.Add(subGridList);
                currentColIndex += subGridColumns;
            }

            return localList;
        }

        public int[,] ConvertListTo2DGrid(IEnumerable<IEnumerable<int>> grid, int rows, int columns, int horizontalCellCountInSubgrid)
        {
            var originalGrid = grid.ToList();
            var localGrid = new int[rows, columns];
            var subGridIndex = 0;
            var currentColumnIndex = 0;
            var currentRowIndex = 0;

            foreach (var subGrid in originalGrid)
            {
                var index = 0;

                if (subGridIndex != 0 && subGridIndex % horizontalCellCountInSubgrid == 0)
                {
                    currentRowIndex += horizontalCellCountInSubgrid;
                    if (currentColumnIndex == columns)
                    {
                        currentColumnIndex = 0;
                    }
                }

                var colIndex = currentColumnIndex;
                var rowIndex = currentRowIndex;

                foreach (var item in subGrid)
                {
                    if (index != 0 && index % horizontalCellCountInSubgrid == 0)
                    {
                        rowIndex++;
                        colIndex = currentColumnIndex;
                    }

                    localGrid[rowIndex, colIndex] = item;
                    index++;
                    colIndex++;
                }

                currentColumnIndex += horizontalCellCountInSubgrid;
                subGridIndex++;
            }

            return localGrid;
        }

        public int[,] Solve(int[,] grid)
        {
            var localGrid = (int[,])grid.Clone();
            var backTrack = false;

            for (var row = 0; row <= 8;)
            {
                for (var col = 0; col <= 8;)
                {
                    var ignoreCell = grid[row, col] > 0;
                    var currentCellValue = localGrid[row, col];

                    if (ignoreCell && backTrack)
                    {
                        GoBackACell(ref row, ref col);
                        continue;
                    }

                    if (!ignoreCell)
                    {
                        var newCellValue = AssignNextBestValueForCell(currentCellValue, localGrid, row, col);

                        if (newCellValue == 0)
                        {
                            localGrid[row, col] = 0;
                            backTrack = true;
                            GoBackACell(ref row, ref col);
                            continue;
                        }
                        else
                        {
                            backTrack = false;
                            localGrid[row, col] = newCellValue;
                        }
                    }

                    col++;
                }

                row++;
            }

            return localGrid;
        }

        private int AssignNextBestValueForCell(int currentCellValue, int[,] currentGrid, int row, int col)
        {
            var nextCellValue = 0;

            if (currentCellValue > 0)
            {
                currentGrid[row, col] = 0;
            }

            var valuesInSubGrid = GetSubGridValues(currentGrid, row, col);
            var numbersInRow = GetNumbersOnRow(row, currentGrid);
            var numbersInColumn = GetNumbersOnColumn(col, currentGrid);
            var possibleValues = Enumerable.Range(1, 9).Except(numbersInRow).Except(numbersInColumn).Except(valuesInSubGrid).Distinct().OrderBy(x => x).ToList();

            if (possibleValues.Count() > 0)
            {
                if (currentCellValue == 0)
                {
                    nextCellValue = possibleValues.FirstOrDefault();
                }
                else
                {
                    var currentValIndex = possibleValues.IndexOf(currentCellValue);

                    if (possibleValues.Count > 0 && possibleValues.Count - 1 >= currentValIndex + 1)
                    {
                        nextCellValue = possibleValues[currentValIndex + 1];
                    }
                }
            }

            return nextCellValue;
        }

        private void GoBackACell(ref int row, ref int col)
        {
            if (col - 1 == -1)
            {
                row--;
                col = 8;
            }
            else
            {
                col--;
            }
        }

        private List<int> GetSubGridValues(int[,] grid, int currentRow, int currentCol)
        {
            var subGrids = new List<List<int>>();

            var currentSubGrid = (currentRow / _subGridSize) * _subGridSize + (currentCol / _subGridSize);

            var gridSizeRowPos = 0;
            var gridSizeColPos = 0;

            for (var g = 0; g < _subGridSize * _subGridSize; g++)
            {
                var valuesInSubGrid = new List<int>();

                for (var row = gridSizeRowPos; row < _subGridSize + gridSizeRowPos; row++)
                {
                    for (var col = gridSizeColPos; col < _subGridSize + gridSizeColPos; col++)
                    {
                        valuesInSubGrid.Add(grid[row, col]);
                    }
                }
                subGrids.Add(valuesInSubGrid);

                if (gridSizeColPos + _subGridSize >= _subGridSize * _subGridSize)
                {
                    gridSizeRowPos += _subGridSize;
                    gridSizeColPos = 0;
                }
                else
                {
                    gridSizeColPos += _subGridSize;
                }
            }

            return subGrids.ElementAt(currentSubGrid);
        }

        private List<int> GetNumbersOnRow(int row, int[,] grid)
        {
            var results = new List<int>();
            for (var r = 0; r <= 8; r++)
            {
                results.Add(grid[row, r]);
            }
            return results;
        }

        private List<int> GetNumbersOnColumn(int col, int[,] grid)
        {
            var results = new List<int>();
            for (var c = 0; c <= 8; c++)
            {
                results.Add(grid[c, col]);
            }
            return results;
        }
    }
}
