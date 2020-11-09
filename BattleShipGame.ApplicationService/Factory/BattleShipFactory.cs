using System;
using System.Collections.Generic;
using BattleShipGame.ApplicationService;

namespace BattleShipGame.ApplicationService
{
    public class BattleShipFactory : IBattleShipFactory
    {
        /// <summary>
        /// Creates Board and assigns players to their board
        /// </summary>
        /// <param name="noOfPlayers"></param>
        /// <param name="boardDimension"></param>
        /// <returns>List of created boards</returns>
        public IEnumerable<IBattleBoard> CreateBattleBoards(int noOfPlayers, int boardDimension)
        {
            try
            {
                List<BattleBoard> battleBoards = new List<BattleBoard>();
                for (int i = 1; i <= noOfPlayers; i++)
                {
                    Player player = new Player();
                    Ship ship = new Ship();
                    
                    BattleBoard battleBoard = new BattleBoard()
                    {
                        BoardDimension = boardDimension,
                        Player = player,
                        Ship = ship
                    };
                    battleBoards.Add(battleBoard);
                }

                return battleBoards;
            }
            catch (Exception)
            {
                //TODO Handle this
                throw;
            }
        }
        /// <summary>
        /// Add Ship Coordinates to ship
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ship"></param>
        public ShipCoordinate CreateShipCoordinate(char x, int y, int shipId)
        {
           return new ShipCoordinate() { X = x, Y = y, ShipId = shipId };
        }
    }
}
