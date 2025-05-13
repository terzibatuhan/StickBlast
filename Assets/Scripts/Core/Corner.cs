using UnityEngine;
using UnityEngine.UI;

public class Corner : MonoBehaviour
{
    public int X;
    public int Y;

    public bool IsOccupied { get; private set; }

    private Image _image;
    private Color _defaultColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
        IsOccupied = false;

        name = $"Corner_{X}x{Y}";
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

    public void ClearPreview() // Removes only the highlight color if the cell is not occupied
    {
        if (IsOccupied)
            return;

        _image.color = _defaultColor;
    }

    public void ClearState() // Resets the cell completely: clears the highlight and marks it as unoccupied
    {
        IsOccupied = false;
        _image.color = _defaultColor;
    }
}
