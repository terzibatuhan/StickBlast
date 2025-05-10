using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    private bool _isPainted = false;

    public void PaintCell()
    {
        _isPainted = true;
        GetComponent<Image>().color = Color.green;
    }

    public void ClearCell()
    {
        _isPainted = false;
        GetComponent<Image>().color = Color.white;
    }

    private void OnMouseDown()
    {
        PaintCell();
    }
}
