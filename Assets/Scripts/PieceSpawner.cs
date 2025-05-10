using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Piece[] _piecePrefabs;

    public void SpawnPiece()
    {
        Piece piece0 = Instantiate(_piecePrefabs[0], _spawnPoints[0].position, Quaternion.identity);
        Piece piece1 = Instantiate(_piecePrefabs[1], _spawnPoints[1].position, Quaternion.identity);
        Piece piece2 = Instantiate(_piecePrefabs[0], _spawnPoints[2].position, Quaternion.identity);
    }
}
