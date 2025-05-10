using System.Collections.Generic;
using UnityEngine;

public class EdgeHolder : MonoBehaviour
{
    public static EdgeHolder Instance { get; private set; }

    private readonly List<Edge> _allEdges = new();

    private void Awake()
    {
        Instance = this;

        _allEdges.AddRange(FindObjectsByType<Edge>(FindObjectsSortMode.InstanceID));

        foreach (var edge in _allEdges)
            edge.AssignPoints();
    }

    public List<Edge> GetAllEdges()
    {
        return _allEdges;
    }
}
