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
    }

    private void Start()
    {
        foreach (var edge in _allEdges)
            edge.AssignCorners();
    }

    public List<Edge> GetAllEdges()
    {
        return _allEdges;
    }
}
