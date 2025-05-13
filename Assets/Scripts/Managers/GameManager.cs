using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        PieceSpawner.Instance.SpawnPiece();
    }
}
