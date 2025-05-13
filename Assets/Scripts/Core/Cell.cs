using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public enum Direction
{
    Top,
    Bottom,
    Left,
    Right
}

public class Cell : MonoBehaviour
{
    public int X;
    public int Y;

    public Dictionary<Direction, Edge> ConnectedEdges = new();

    public bool IsOccupied {  get; private set; }

    private Image _image;
    private Color _defaultColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    private void Start()
    {
        AssignEdges();
    }

    public void Occupy()
    {
        if (IsOccupied)
            return;

        IsOccupied = true;
        _image.color = Color.white;

        ExpManager.Instance.AddExp(10f * ExpManager.Combo);
        ExpManager.Combo++;

        GridManager.Instance.CheckForFullRowsAndColumns();
    }

    public void PaintEdges()
    {
        foreach (var edge in ConnectedEdges)
            edge.Value.Occupy();
    }

    public void ClearCell()
    {
        IsOccupied = false;
        _image.color = _defaultColor;

        foreach (var edge in ConnectedEdges)
            edge.Value.ClearState();
    }

    public void CheckIfSurrounded()
    {
        foreach (var edge in ConnectedEdges)
            if (!edge.Value.IsOccupied)
                return;

        Occupy();
    }

    public void AssignEdges()
    {
        foreach (var edge in EdgeHolder.Instance.GetAllEdges())
        {
            if (edge.Orientation == Orientation.Horizontal)
            {
                if (edge.X == X && edge.Y == Y) // Top edge
                    ConnectedEdges.Add(Direction.Top, edge);
                else if (edge.X == X + 1 && edge.Y == Y) // Bottom edge
                    ConnectedEdges.Add(Direction.Bottom, edge);
            }
            else if (edge.Orientation == Orientation.Vertical)
            {
                if (edge.X == X && edge.Y == Y) // Left edge
                    ConnectedEdges.Add(Direction.Left, edge);
                else if (edge.X == X && edge.Y == Y + 1) // Right edge
                    ConnectedEdges.Add(Direction.Right, edge);
            }
        }

        foreach (var edge in ConnectedEdges)
            edge.Value.AssignCell(this);
    }

    public List<Direction> GetEmptyDirections()
    {
        var emptyDirs = new List<Direction>();

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            if (!ConnectedEdges.TryGetValue(dir, out Edge edge) || !edge.IsOccupied)
            {
                emptyDirs.Add(dir);
            }
        }

        return emptyDirs;
    }

    /*public int GetEmptySideCount()
    {
        int emptySideCount = 0;

        foreach (var edge in ConnectedEdges)
        {
            if (edge.Value.IsOccupied)
                emptySideCount++;
        }

        return emptySideCount;
    }*/

    public Edge GetEdge(Direction edge)
    {
        return ConnectedEdges[edge];
    }
}
