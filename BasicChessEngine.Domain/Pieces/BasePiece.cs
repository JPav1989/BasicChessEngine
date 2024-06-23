using BasicChessEngine.Domain.Enums;
using BasicChessEngine.Domain.Interfaces;

namespace BasicChessEngine.Domain.Pieces;

public abstract class BasePiece
{
    protected readonly IBoardManager _boardManager;

    public BasePiece(IBoardManager boardManager)
    {
        _boardManager = boardManager;
    }
    /// <summary>
    /// The colour of the piece, be it Black or White
    /// </summary>
    public Colour Colour { get; set; }
    
    /// <summary>
    /// The active state of the piece, if taken be the opponent, it would be false.
    /// </summary>
    public bool Active { get; set; }
    
    /// <summary>
    /// The current position on the board, for example A8.
    /// </summary>
    public string CurrentBoardPosition { get; set; }
    
    /// <summary>
    /// Abstract method that defines moves the piece and returns true if the move was legal.
    /// </summary>
    /// <param name="position"></param>
    /// <returns>a bool based in if the move was successfully completed.</returns>
    public abstract bool MovePiece(string position);

}

public class Pawn : BasePiece
{
    public bool MadeMove { get; set; }

    private List<string> PotentialLegalMoves()
    {
        List<string> potentialMoves = new();
        var current = CurrentBoardPosition.ToCharArray();
        
        if (Colour == Colour.White)
        {
            int rowPosition = int.Parse(current[1].ToString());
            if (!MadeMove)
            {
                int[] potentialRowPositions = new int[2];
                potentialRowPositions[0] = ++rowPosition;
                potentialRowPositions[1] = ++rowPosition;

                potentialMoves.AddRange(potentialRowPositions.Select(row => $"{current[0]}{row.ToString()}"));
            }
            else
            {
                rowPosition++;
                potentialMoves.Add($"{current[0]}{rowPosition.ToString()}");
            }

            CheckTakeMoveWhite(potentialMoves, current);
        }

        return potentialMoves;
    }

    private void CheckTakeMoveWhite(List<string> potentialMoves, char[] currentPosition)
    {
        switch (currentPosition[0])
            {
                case 'A':
                {
                    CheckAdjacentColumnWhite(potentialMoves,"B", currentPosition);
                    break;
                }
                case 'B':
                {
                    CheckNextAndPreviousColumnsWhite(potentialMoves, "A", "C", currentPosition);
                    break;
                }
                case 'C':
                {
                    CheckNextAndPreviousColumnsWhite(potentialMoves, "B", "D", currentPosition);
                    break;
                }
                case 'D':
                {
                    CheckNextAndPreviousColumnsWhite(potentialMoves, "C", "E", currentPosition);
                    break;
                }
                case 'E':
                {
                    CheckNextAndPreviousColumnsWhite(potentialMoves, "D", "F", currentPosition);
                    break;
                }
                case 'F':
                {
                    CheckNextAndPreviousColumnsWhite(potentialMoves, "E", "G", currentPosition);
                    break;
                }
                case 'G':
                {
                    CheckNextAndPreviousColumnsWhite(potentialMoves, "F", "H", currentPosition);
                    break;
                }
                case 'H':
                {
                    CheckAdjacentColumnWhite(potentialMoves,"G", currentPosition);
                    break;
                }
            }
    }

    private void CheckNextAndPreviousColumnsWhite(List<string> potentialMoves, string prevColumn, string nextColumn, char[] currentPosition)
    {
        int tempRowPosition = int.Parse(currentPosition[1].ToString());
        if (_boardManager.PositionOccupied($"{prevColumn}{++tempRowPosition}"))
        {
            potentialMoves.Add($"{prevColumn}{tempRowPosition}");
        }
                
        tempRowPosition = int.Parse(currentPosition[1].ToString());
        if (_boardManager.PositionOccupied($"{nextColumn}{++tempRowPosition}"))
        {
            potentialMoves.Add($"{nextColumn}{tempRowPosition}");
        }
    }

    private void CheckAdjacentColumnWhite(List<string> potentialMoves, string adjacentColumn, char[] currentPosition)
    {
        int tempRowPosition = int.Parse(currentPosition[1].ToString());
        if (_boardManager.PositionOccupied($"{adjacentColumn}{++tempRowPosition}"))
        {
            potentialMoves.Add($"{adjacentColumn}{tempRowPosition}");
        }
    }
    public override bool MovePiece(string position)
    {
        return PotentialLegalMoves().Contains(position);
    }

    public Pawn(IBoardManager boardManager) : base(boardManager)
    {
    }
}