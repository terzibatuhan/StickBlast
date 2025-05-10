using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    public bool IsOccupied { get; private set; }
    private Image _image;
    private Color _defaultColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
        IsOccupied = false;
    }

    public void Paint()
    {
        if (IsOccupied) 
            return;

        IsOccupied = true;
        _image.color = Color.white;
    }

    public void Highlight()
    {
        _image.color = Color.white;
    }

    public void ResetColor()
    {
        if (IsOccupied)
            return;

        _image.color = _defaultColor;
    }
}
