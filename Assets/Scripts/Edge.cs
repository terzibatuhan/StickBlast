using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum Orientation { Horizontal, Vertical }

public class Edge : MonoBehaviour
{
    public int X;
    public int Y;

    private Point _pointA;
    private Point _pointB;

    [SerializeField] private List<Cell> _connectedCells = new();

    public bool IsOccupied { get; private set; }

    public Orientation Orientation;
    private Image _image;
    private Color _defaultColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    public void Highlight()
    {
        _image.color = Color.gray;

        _pointA.Highlight();
        _pointB.Highlight();
    }

    public void ClearPreview() // Removes only the highlight color if the cell is not occupied
    {
        _image.color = _defaultColor;

        _pointA.ClearPreview();
        _pointB.ClearPreview();
    }

    public void ClearState() // Resets the cell completely: clears the highlight and marks it as unoccupied
    {
        _image.color = _defaultColor;
        IsOccupied = false;

        _pointA.ClearState();
        _pointB.ClearState();
    }

    public void Occupy()
    {
        IsOccupied = true;
        _image.color = Color.white;

        PaintCorners();

        foreach (Cell cell in _connectedCells)
            cell.CheckIfSurrounded();
    }

    public void PaintCorners()
    {
        _pointA.Paint();
        _pointB.Paint();
    }

    public void AssignCell(Cell cell)
    {
        if (!_connectedCells.Contains(cell))
            _connectedCells.Add(cell);
    }

    public void AssignPoints()
    {
        // example: "Edge_H_2x3"
        var parts = name.Split('_', 'x');
        int x = int.Parse(parts[1]);
        int y = int.Parse(parts[2]);
        bool isHorizontal = parts[3] == "H";

        var p1 = $"Point_{x}x{y}";
        var p2 = isHorizontal ? $"Point_{x}x{y + 1}" : $"Point_{x + 1}x{y}";

        _pointA = GameObject.Find(p1).GetComponent<Point>();
        _pointB = GameObject.Find(p2).GetComponent<Point>();
    }
}