using BattleShipGame.ApplicationService;
using System.Collections.Generic;

namespace BattleShipGame.ApplicationService
{
    public interface IBattleShipFactory
    {
        IEnumerable<IBattleBoard> CreateBattleBoards(int noOfPlayers, int boardDimension);
        ShipCoordinate CreateShipCoordinate(char x, int y, int shipId);
    }
}