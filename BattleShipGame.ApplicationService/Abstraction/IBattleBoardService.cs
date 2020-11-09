using BattleShipGame.ApplicationService;
using System.Collections;
using System.Collections.Generic;

namespace BattleShipGame.ApplicationService
{
    public interface IBattleBoardService
    {
        int BoardDimension { get; set; }
        bool CreateBoard(int boardDimension, int noOfPlayers);
        bool IsBoardSizeValid(int boardDimension, int gameBoardSizeLowerLimit, int gameBoardSizeUpperLimit);
        IShip GetCurrentPlayerShipOnBattleBoard(int playerId);

        bool SetShipOnBattleBoard(char x, int y, int shipLength, string shipDirection, IShip currentShip);
        bool IsShipHitByMissile(char x, int y, IShip currentShip);
        bool IsShipSunk(IShip currentShip);

        bool ValidateMissileCoordinates(char x, int y);

        bool ValidateShipLocationOnBoard(string shipDirection, char xCoordinate, int yCoordinate);

        bool ValidateShipDirection(string userInput);
    }
}