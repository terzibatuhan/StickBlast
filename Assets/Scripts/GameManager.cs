using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PieceSpawner _pieceSpawner;

    private void Start()
    {
        _pieceSpawner.SpawnPiece();
    }
}
