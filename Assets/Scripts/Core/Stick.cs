using UnityEngine;
using System.Collections;

public class Stick : MonoBehaviour
{
    private Edge _edgeToSnap;
    public Orientation Orientation;

    public Edge FindClosestEdge(Vector2 pos)
    {
        float minDist = float.MaxValue;
        Edge closest = null;

        foreach (var edge in EdgeHolder.Instance.GetAllEdges())
        {
            float dist = Vector2.Distance(edge.transform.position, pos);

            if (dist < 0.4f && dist < minDist && !edge.IsOccupied && Orientation == edge.Orientation)
            {
                minDist = dist;
                closest = edge;
            }
        }

        _edgeToSnap = closest;
        return closest;
    }

    public void SnapToClosestEdge()
    {
        if (_edgeToSnap == null) return;

        StartCoroutine(SmoothSnapWithScale(_edgeToSnap.transform.position));

        _edgeToSnap.Occupy();

        transform.SetParent(null);

        ExpManager.Instance.AddExp(3);
    }

    private IEnumerator SmoothSnapWithScale(Vector3 targetPos)
    {
        float duration = 0.2f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 startScale = transform.localScale;
        Vector3 overshootScale = startScale * 1.2f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            if (t < 0.5f)
            {
                float scaleT = t / 0.5f;
                transform.localScale = Vector3.Lerp(startScale, overshootScale, scaleT);
            }
            else
            {
                float scaleT = (t - 0.5f) / 0.5f;
                transform.localScale = Vector3.Lerp(overshootScale, startScale, scaleT);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    public void SetOrientation(Orientation targetOrientation)
    {
        if (Orientation != targetOrientation)
        {
            ChangeOrientations();
            transform.Rotate(0, 0, 90);
        }
    }

    public void ChangeOrientations()
    {
        Orientation = Orientation == Orientation.Horizontal
        ? Orientation.Vertical
        : Orientation.Horizontal;
    }
}
