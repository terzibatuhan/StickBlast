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
        _ = Instantiate(_piecePrefabs[Random.Range(0, _piecePrefabs.Length)], _spawnPoints[0].position, Quaternion.identity);
        _ = Instantiate(_piecePrefabs[Random.Range(0, _piecePrefabs.Length)], _spawnPoints[1].position, Quaternion.identity);
        _ = Instantiate(_piecePrefabs[Random.Range(0, _piecePrefabs.Length)], _spawnPoints[2].position, Quaternion.identity);

        _remainingPieces = 3;
    }

    public void ChangePieceCount(int number)
    {
        _remainingPieces += number;

        if (_remainingPieces == 0)
            SpawnPiece();
    }
}
