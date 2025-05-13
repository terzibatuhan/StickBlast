using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public Cell[,] Cells {  get; private set; }

    private readonly int _width = 4;
    private readonly int _height = 4; 

    private void Awake()
    {
        Instance = this;

        Cells = new Cell[_width, _height];

        Cell[] allCells = FindObjectsByType<Cell>(FindObjectsSortMode.InstanceID);
        foreach (var cell in allCells)
        {
            if (cell.X < _width && cell.Y < _height)
            {
                Cells[cell.X, cell.Y] = cell;
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
            return Cells[x, y];

        return null;
    }

    public void ClearRow(int row)
    {
        for (int x = 0; x < _width; x++)
            Cells[x, row].ClearCell();

        ExpManager.Instance.AddExp(100f);
    }

    public void ClearColumn(int column)
    {
        for (int y = 0; y < _height; y++)
            Cells[column, y].ClearCell();

        ExpManager.Instance.AddExp(100f);
    }

    public void CheckForFullRowsAndColumns()
    {
        List<int> rowsToClear = new();
        List<int> columnsToClear = new();

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

        PaintOccupiedsAgain();
    }

    private void PaintOccupiedsAgain()
    {
        foreach (var cell in Cells)
            if (cell.IsOccupied)
                cell.PaintEdges();

        foreach (Edge edge in EdgeHolder.Instance.GetAllEdges())
            if (edge.IsOccupied)
                edge.PaintCorners();
    }

    private bool IsRowCompleted(int rowIndex)
    {
        for (int x = 0; x < _width; x++)
        {
            if (!Cells[x, rowIndex].IsOccupied)
                return false;
        }
        return true;
    }
    private bool IsColumnCompleted(int columnIndex)
    {
        for (int y = 0; y < _height; y++)
        {
            if (!Cells[columnIndex, y].IsOccupied)
                return false;
        }
        return true;
    }

    public void ClearBoard()
    {
        for (int i = 0; i < _width; i++)
            for (int j = 0; j < _height; j++)
                Cells[i, j].ClearCell();

        foreach (Edge edge in EdgeHolder.Instance.GetAllEdges())
            edge.ClearState();
    }
}
