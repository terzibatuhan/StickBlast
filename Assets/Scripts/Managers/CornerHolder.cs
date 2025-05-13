using System.Collections.Generic;
using UnityEngine;

public class CornerHolder : MonoBehaviour
{
    public static CornerHolder Instance;

    private readonly Dictionary<(int x, int y), Corner> _corners = new();

    private void Awake()
    {
        Instance = this;

        foreach (Corner corner in FindObjectsByType<Corner>(FindObjectsSortMode.InstanceID))
            _corners[(corner.X, corner.Y)] = corner;
    }

    public Corner GetCorner(int x, int y)
    {
        return _corners.TryGetValue((x, y), out var corner) ? corner : null;
    }
}
