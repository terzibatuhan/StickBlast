using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    private Cell[,] _cells;

    private readonly int _width = 4;
    private readonly int _height = 4; 

    private void Awake()
    {
        Instance = this;

        _cells = new Cell[_width, _height];

        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsSortMode.InstanceID);
        foreach (var cell in allCells)
        {
            if (cell.X < _width && cell.Y < _height)
            {
                _cells[cell.X, cell.Y] = cell;
            }
            else
            {
                Debug.LogWarning($"Cell at ({cell.X}, {cell.Y}) is out of grid bounds!");
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (x >= 0 && x < _width && y >= 0 && y < _height)
            return _cells[x, y];

        return null;
    }

    public void ClearRow(int row)
    {
        for (int x = 0; x < _width; x++)
        {
            _cells[x, row].ClearCell();
        }
    }

    public void ClearColumn(int column)
    {
        for (int y = 0; y < _height; y++)
        {
            _cells[column, y].ClearCell();
        }
    }

    public void CheckForFullRowsAndColumns()
    {
        List<int> rowsToClear = new List<int>();
        List<int> columnsToClear = new List<int>();

        for (int i = 0; i < _width; i++)
        {
            if (IsRowCompleted(i))
            {
                rowsToClear.Add(i);
            }
        }

        for (int i = 0; i < _height; i++)
        {
            if (IsColumnCompleted(i))
            {
                columnsToClear.Add(i);
            }
        }

        foreach (var rowIndex in rowsToClear)
        {
            ClearRow(rowIndex);
        }

        foreach (var columnIndex in columnsToClear)
        {
            ClearColumn(columnIndex);
        }
    }

    private bool IsRowCompleted(int rowIndex)
    {
        for (int x = 0; x < _width; x++)
        {
            if (!_cells[x, rowIndex].IsOccupied)
                return false;
        }
        return true;
    }
    private bool IsColumnCompleted(int columnIndex)
    {
        for (int y = 0; y < _height; y++)
        {
            if (!_cells[columnIndex, y].IsOccupied)
                return false;
        }
        return true;
    }
}
