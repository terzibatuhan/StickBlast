using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum Orientation { Horizontal, Vertical }

public class Edge : MonoBehaviour
{
    public int X;
    public int Y;

    public Corner CornerA;
    public Corner CornerB;

    public List<Cell> ConnectedCells = new();

    public bool IsOccupied { get; private set; }

    public Orientation Orientation;
    private Image _image;
    private Color _defaultColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    public void Occupy()
    {
        IsOccupied = true;
        _image.color = Color.white;

        PaintCorners();

        foreach (Cell cell in ConnectedCells)
            cell.CheckIfSurrounded();
    }

    public void Highlight()
    {
        _image.color = Color.gray;

        CornerA.Highlight();
        CornerB.Highlight();
    }

    public void ClearPreview() // Removes only the highlight color if the cell is not occupied
    {
        _image.color = _defaultColor;

        CornerA.ClearPreview();
        CornerB.ClearPreview();
    }

    public void ClearState() // Resets the cell completely: clears the highlight and marks it as unoccupied
    {
        _image.color = _defaultColor;
        IsOccupied = false;

        CornerA.ClearState();
        CornerB.ClearState();
    }

    public void PaintCorners()
    {
        CornerA.Paint();
        CornerB.Paint();
    }

    public void AssignCell(Cell cell)
    {
        if (!ConnectedCells.Contains(cell))
            ConnectedCells.Add(cell);
    }

    public void AssignCorners()
    {
        CornerA = CornerHolder.Instance.GetCorner(X, Y);
        CornerB = (Orientation == Orientation.Horizontal)
            ? CornerHolder.Instance.GetCorner(X, Y + 1)
            : CornerHolder.Instance.GetCorner(X + 1, Y);
    }
}