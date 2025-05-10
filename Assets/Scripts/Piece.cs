using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour
{
    private Vector3 _offset;
    private bool _isDragging;
    private Vector3 _originalPosition;

    [SerializeField] private Stick[] _sticks;

    private void Start()
    {
        _originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        _offset = transform.position - GetMouseWorldPosition();
        _isDragging = true;
    }

    void OnMouseDrag()
    {
        foreach (var edge in EdgeHolder.Instance.GetAllEdges())
            edge.ResetColor();

        if (_isDragging)
        {
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            transform.position = mouseWorldPos + _offset;
        }

        foreach (Stick stick in _sticks)
        {
            Vector2 pos = stick.transform.position;
            Edge edge = stick.FindClosestEdge(pos);

            if (edge != null && !edge.IsOccupied)
                edge.Highlight();
        }
    }

    void OnMouseUp()
    {
        _isDragging = false;

        bool allSnapped = true;

        foreach (var stick in _sticks)
        {
            var edge = stick.FindClosestEdge(stick.transform.position);

            if (edge != null)
            {
                stick.SnapToClosestEdge();
            }
            else
            {
                allSnapped = false;
            }
        }

        if (allSnapped)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(MoveBackToOriginalPosition());
        }
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
}
