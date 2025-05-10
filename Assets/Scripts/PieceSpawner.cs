using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public static PieceSpawner Instance;

    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Piece[] _piecePrefabs;

    int _remainingPieces;

    private void Awake()
    {
        Instance = this;   
    }

    public void SpawnPiece()
    {
        Piece piece0 = Instantiate(_piecePrefabs[0], _spawnPoints[0].position, Quaternion.identity);
        Piece piece1 = Instantiate(_piecePrefabs[1], _spawnPoints[1].position, Quaternion.identity);
        Piece piece2 = Instantiate(_piecePrefabs[0], _spawnPoints[2].position, Quaternion.identity);

        _remainingPieces = 3;
    }

    public void ChangePieceCount(int number)
    {
        _remainingPieces += number;

        if (_remainingPieces == 0)
            SpawnPiece();
    }
}
