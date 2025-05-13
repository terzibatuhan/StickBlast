using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
    private bool _isDragging;
    private Vector3 _originalPosition;
    private Vector3 _originalScale;

    public Vector3 Offset;

    public Direction[] Directions;

    public int Width;
    public int Height;

    [SerializeField] private Stick[] _sticks;

    public Stick[] Sticks => _sticks;

    private void Start()
    {
        _originalPosition = transform.position;
        _originalScale = transform.localScale;
    }

    void OnMouseDown()
    {
        transform.localScale = Vector3.one;
        _isDragging = true;
    }

    void OnMouseDrag()
    {
        foreach (var edge in EdgeHolder.Instance.GetAllEdges())
            if (!edge.IsOccupied)
                edge.ClearPreview();

        if (_isDragging)
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            transform.position = mouseWorldPos + Offset;
        }

        var edges = GetPlaceableEdges();
        if (edges == null)
            return;

        foreach (var edge in edges)
            edge.Highlight();
    }

    void OnMouseUp()
    {
        _isDragging = false;

        var edges = GetPlaceableEdges();
        if (edges == null)
        {
            transform.localScale = _originalScale;
            StartCoroutine(MoveBackToOriginalPosition());
            return;
        }

        foreach (var stick in _sticks)
            stick.SnapToClosestEdge();

        PieceSpawner.Instance.ChangePieceCount(-1);

        BoardAnalyzer.Instance.Analyze();

        Destroy(gameObject);
    }

    private List<Edge> GetPlaceableEdges()
    {
        List<Edge> edgesToPlace = new();

        foreach (Stick stick in _sticks)
        {
            Vector2 pos = stick.transform.position;
            Edge edge = stick.FindClosestEdge(pos);

            if (edge != null && !edge.IsOccupied)
                edgesToPlace.Add(edge);
        }

        if (edgesToPlace.Count != _sticks.Length)
            return null;

        return edgesToPlace;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }

    private IEnumerator MoveBackToOriginalPosition()
    {
        float elapsed = 0f;
        float duration = 0.2f;
        Vector3 start = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, _originalPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = _originalPosition;
    }

    public void ChangeOrientations()
    {
        foreach (Stick stick in _sticks)
            stick.ChangeOrientations();
    }

    public void RotateClockwise()
    {
        for (int i = 0; i < Directions.Length; i++)
        {
            Directions[i] = RotateDirectionClockwise(Directions[i]);
        }
    }

    private Direction RotateDirectionClockwise(Direction dir)
    {
        return dir switch
        {
            Direction.Top => Direction.Left,
            Direction.Right => Direction.Top,
            Direction.Bottom => Direction.Right,
            Direction.Left => Direction.Bottom,
            _ => dir
        };
    }
}
