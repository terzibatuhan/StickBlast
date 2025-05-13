using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardAnalyzer : MonoBehaviour
{
    public static BoardAnalyzer Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Analyze()
    {
        bool atLeastOnePieceFits = false;

        foreach (Piece piece in PieceSpawner.Instance.CurrentPieces)
        {
            if (piece.Sticks.Length == 1)
            {
                var stick = piece.Sticks[0];
                var orientation = stick.Orientation;

                foreach (var edge in EdgeHolder.Instance.GetAllEdges())
                {
                    if (!edge.IsOccupied && edge.Orientation == orientation)
                    {
                        atLeastOnePieceFits = true;
                        break;
                    }
                }

                if (atLeastOnePieceFits)
                    break;
            }

            // TODO: Implement support for long I pieces that span multiple cells
            if (piece.Width > 1 || piece.Height > 1)
            {
                continue;
            }

            else
            {
                foreach (Cell cell in GridManager.Instance.Cells)
                {
                    var emptyDirs = cell.GetEmptyDirections();

                    if (piece.Directions.All(d => emptyDirs.Contains(d)))
                    {
                        atLeastOnePieceFits = true;
                        break;
                    }
                }

                if (atLeastOnePieceFits)
                    break;
            }
        }

        if (!atLeastOnePieceFits)
        {
            Debug.Log("Game Over");
        }
    }

    public Edge GetMissingPiece()
    {
        var suitableCells = GridManager.Instance.Cells
        .Cast<Cell>()
        .Where(c => c != null && c.GetEmptyDirections().Count == 1)
        .ToList();

        if (suitableCells.Count == 0)
            return null;

        var randomCell = suitableCells[UnityEngine.Random.Range(0, suitableCells.Count)];
        var dir = randomCell.GetEmptyDirections()[0];

        return randomCell.GetEdge(dir);
    }
}
