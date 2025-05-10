using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    public int X;
    public int Y;

    [SerializeField] private List<Edge> _connectedEdges = new();

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

    public void PaintCell()
    {
        IsOccupied = true;
        _image.color = Color.white;

        GridManager.Instance.CheckForFullRowsAndColumns();
    }

    public void ClearCell()
    {
        IsOccupied = false;
        _image.color = _defaultColor;

        foreach (Edge edge in _connectedEdges)
            edge.ClearState();
    }

    public void CheckIfSurrounded()
    {
        foreach (Edge edge in _connectedEdges)
            if (!edge.IsOccupied)
                return;

        PaintCell();
    }

    public void AssignEdges()
    {
        foreach (var edge in EdgeHolder.Instance.GetAllEdges())
        {
            if (edge.Orientation == Orientation.Horizontal)
            {
                if (edge.X == X && edge.Y == Y) // Top edge
                    _connectedEdges.Add(edge);
                else if (edge.X == X + 1 && edge.Y == Y) // Bottom edge
                    _connectedEdges.Add(edge);
            }
            else if (edge.Orientation == Orientation.Vertical)
            {
                if (edge.X == X && edge.Y == Y) // Left edge
                    _connectedEdges.Add(edge);
                else if (edge.X == X && edge.Y == Y + 1) // Right edge
                    _connectedEdges.Add(edge);
            }
        }

        foreach (Edge edge in _connectedEdges)
            edge.AssignCell(this);
    }
}
