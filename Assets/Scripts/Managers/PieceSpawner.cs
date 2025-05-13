using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public static PieceSpawner Instance;

    [SerializeField] private Piece[] _piecePrefabs;
    [SerializeField] private Transform[] _spawnPoints;

    public List<Piece> CurrentPieces = new();

    int _remainingPieces;

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnPiece()
    {
        CurrentPieces.Clear();

        List<Edge> missingEdges = new();

        for (int i = 0; i < 3; i++)
        {
            Edge edge = BoardAnalyzer.Instance.GetMissingPiece();

            if (edge != null && !missingEdges.Contains(edge))
            {
                missingEdges.Add(edge);

                var piece = Instantiate(_piecePrefabs[0], _spawnPoints[i].position, Quaternion.identity);

                Stick stick = piece.GetComponentInChildren<Stick>();

                stick.SetOrientation(edge.Orientation);

                CurrentPieces.Add(piece);
            }

            else
            {
                int pieceID = Random.Range(0, _piecePrefabs.Length);
                var piece = Instantiate(_piecePrefabs[pieceID], _spawnPoints[i].position, Quaternion.identity);

                int[] possibleAngles = new int[] { 0, 90, 180, 270 };
                int randomAngle = possibleAngles[Random.Range(0, possibleAngles.Length)];
                piece.transform.rotation = Quaternion.Euler(0, 0, randomAngle);

                // Adjusts piece orientation if rotated by 90 or 270 degrees (i.e., changes axis)
                if (randomAngle == 90 || randomAngle == 270)
                {
                    piece.ChangeOrientations();
                    int rotations = randomAngle / 90;  // This will give 1 for 90° and 3 for 270°

                    for (int j = 0; j < rotations; j++)
                    {
                        piece.RotateClockwise();
                    }
                }

                CurrentPieces.Add(piece);
            }
        }

        _remainingPieces = 3;
    }

    public void ChangePieceCount(int number)
    {
        _remainingPieces += number;

        if (_remainingPieces == 0)
            SpawnPiece();
    }
}
