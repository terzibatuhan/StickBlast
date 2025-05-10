using UnityEngine;
using UnityEngine.UI;

public enum Orientation { Horizontal, Vertical }

public class Edge : MonoBehaviour
{
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
        _image.color = Color.yellow;
    }

    public void ResetColor()
    {
        _image.color = _defaultColor;
    }

    public void Occupy()
    {
        IsOccupied = true;
        _image.color = Color.gray;
    }
}