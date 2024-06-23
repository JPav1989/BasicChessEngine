using BasicChessEngine.Domain.Enums;
using BasicChessEngine.Domain.Interfaces;
using BasicChessEngine.Domain.Pieces;
using FluentAssertions;
using NSubstitute;

namespace BasicChessEngine.Domain.Tests;

public class PawnTests
{
    private Pawn _pawn;
    private IBoardManager _boardManager;

    [SetUp]
    public void Setup()
    {
        _boardManager = Substitute.For<IBoardManager>();
        _boardManager.PositionOccupied(Arg.Any<string>()).ReturnsForAnyArgs(false);
        _pawn = new Pawn(_boardManager)
        {
            Active = true,
            Colour = Colour.White,
            CurrentBoardPosition = "A2"
        };
    }

    [TestCase("A3")]
    [TestCase("A4")]
    public void WhenMovePieceIsCalledWhenMadeMoveIsFalse_ThenThePawnCanMoveUpToTwoSpacesForwardPositive(string movePosition)
    {
        var result = _pawn.MovePiece(movePosition);

        result.Should().BeTrue();
    }
    
    [TestCase("A5")]
    [TestCase("A1")]
    [TestCase("B1")]
    [TestCase("B3")]
    public void WhenMovePieceIsCalledWhenMadeMoveIsFalse_ThenThePawnCanMoveUpToTwoSpacesForwardNegative(string movePosition)
    {
        var result = _pawn.MovePiece(movePosition);

        result.Should().BeFalse();
    }
    
    [TestCase("A3")]
    public void WhenMovePieceIsCalledWhenMadeMoveIsTrue_ThenThePawnCanMoveUpOnlyOneSpaceForwardPositive(string movePosition)
    {
        _pawn.MadeMove = true;
        var result = _pawn.MovePiece(movePosition);

        result.Should().BeTrue();
    }
    
    [TestCase("A4")]
    [TestCase("A1")]
    [TestCase("B1")]
    [TestCase("B3")]
    public void WhenMovePieceIsCalledWhenMadeMoveIsTrue_ThenThePawnCanMoveUpOnlyOneSpaceForwardNegative(string movePosition)
    {
        _pawn.MadeMove = true;
        var result = _pawn.MovePiece(movePosition);

        result.Should().BeFalse();
    }

    [TestCase("B3")]
    public void WhenMovePieceIsCalled_ThePawnCanMoveOneSpaceDiagonallyForwardIfAnotherPieceOccupiesTheSpace(string movePosition)
    {
        _boardManager.PositionOccupied(Arg.Any<string>()).ReturnsForAnyArgs(true);
        
        var result = _pawn.MovePiece(movePosition);
        
        result.Should().BeTrue();
    }
}