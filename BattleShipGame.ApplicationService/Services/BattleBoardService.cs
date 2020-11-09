using BattleShipGame.ApplicationService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShipGame.ApplicationService
{
    public class BattleBoardService : IBattleBoardService
    {
        private readonly IBattleShipFactory _battleShipFactory;

        public BattleBoardService(IBattleShipFactory battleShipFactory)
        {
            _battleShipFactory = battleShipFactory;
        }

        public int MyProperty { get; set; }

        public List<IBattleBoard> _battleBoardCollection = new List<IBattleBoard>();

        public int BoardDimension { get; set; }

        /// <summary>
        /// Validates Game Board dimension against game limits
        /// </summary>
        /// <param name="boarddimension">User Input</param>
        /// <param name="gameBoardSizeLowerLimit">set in UI layer</param>
        /// <param name="gameBoardSizeUpperLimit">set in UI layer</param>
        /// <returns></returns>
        public Boolean IsBoardSizeValid(int boarddimension, int gameBoardSizeLowerLimit, int gameBoardSizeUpperLimit)
        {
            var isBoardSizeValid = boarddimension >= gameBoardSizeLowerLimit && boarddimension <= gameBoardSizeUpperLimit;
            return isBoardSizeValid;
        }

        /// <summary>
        /// Creates battle boards from factory 
        /// </summary>
        /// <param name="boardDimension">User Input</param>
        /// <param name="noOfPlayers">must be set from the UI layer</param>
        public bool CreateBoard( int boardDimension, int noOfPlayers)
        {
            _battleBoardCollection = _battleShipFactory.CreateBattleBoards(noOfPlayers, boardDimension).ToList();
            return _battleBoardCollection.Count() != 0;
        }

        /// <summary>
        /// Set current boards ship for service
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="boardId"></param>
        public IShip GetCurrentPlayerShipOnBattleBoard(int playerId)
        {   
            return _battleBoardCollection.Where(x=>x.Player.PlayerId == playerId).Select(x => x.Ship).FirstOrDefault();
        }

        /// <summary>
        /// Set Ship on Battle Board 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public bool SetShipOnBattleBoard(char x, int y, int shipLength, string shipDirection, IShip currentShip)
        {
            var isShipSetupSuccessfull = false;
            try
            {
                isShipSetupSuccessfull = SetupShipDirectionAndLength(shipLength, shipDirection, currentShip, isShipSetupSuccessfull);

                if (currentShip.ShipLength != 0 && isShipSetupSuccessfull)
                {
                    var yCoordinate = y;
                    var xCoordinate = x;
                    CreateNewShipCoordinate(x, y, currentShip);

                    while (currentShip.ShipCoordinates.Count() < currentShip.ShipLength)
                    {
                        //vertical
                        if (currentShip.ShipDirection == ShipDirection.U || currentShip.ShipDirection == ShipDirection.D)
                        {
                            if (currentShip.ShipDirection == ShipDirection.U)
                            {
                                CreateNewShipCoordinate(x, ++yCoordinate, currentShip);
                            }
                            else
                            {
                                CreateNewShipCoordinate(x, --yCoordinate, currentShip);
                            }
                        }
                        //horizontal
                        else
                        {
                            if (currentShip.ShipDirection == ShipDirection.R)
                            {
                                xCoordinate = xCoordinate.NextAlphabet();
                                CreateNewShipCoordinate(xCoordinate, y, currentShip);
                            }
                            else
                            {
                                xCoordinate = xCoordinate.PreviousAlphabet();
                                CreateNewShipCoordinate(xCoordinate, y, currentShip);
                            }
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                //TODO: Handle this - Add log
                isShipSetupSuccessfull = false;
                
            }
            return isShipSetupSuccessfull;
        }

        private void CreateNewShipCoordinate(char x, int y, IShip currentShip)
        {
            IShipCoordinate shipCoordinate = _battleShipFactory.CreateShipCoordinate(x, y, currentShip.ShipId);

            currentShip.ShipCoordinates.Add(shipCoordinate);
        }

        private static bool SetupShipDirectionAndLength(int shipLength, string shipDirection, IShip currentShip, bool isShipSetupSuccessfull)
        {
            currentShip.ShipLength = shipLength;

            if (Enum.TryParse(shipDirection, out ShipDirection shipDirectionTemp))
            {
                currentShip.ShipDirection = shipDirectionTemp;
                isShipSetupSuccessfull = true;
            }
           
            return isShipSetupSuccessfull;
        }

        public bool ValidateShipDirection(string userInput)
        {
            var isShipDirectionValid = Enum.IsDefined(typeof(ShipDirection), userInput);
            return isShipDirectionValid;
        }
        public bool ValidateShipLocationOnBoard(string shipDirection, char xCoordinate, int yCoordinate)
        {
            var isShipLocationValid = false;
            var boardOrigin = 1;

            try
            {
                var isShipCoordinateValid = yCoordinate <= BoardDimension && xCoordinate.ConvertCharToNumber() <= BoardDimension;

                if (isShipCoordinateValid)
                {
                    if (shipDirection == ShipDirection.R.ToString())
                    {
                        isShipLocationValid = xCoordinate.ConvertCharToNumber() + 2 <= BoardDimension;
                    }
                    else if (shipDirection == ShipDirection.L.ToString())
                    {
                        isShipLocationValid = xCoordinate.ConvertCharToNumber() - 2 >= boardOrigin;
                    }
                    else if (shipDirection == ShipDirection.U.ToString())
                    {
                        isShipLocationValid = yCoordinate + 2 <= BoardDimension;
                    }
                    else if (shipDirection == ShipDirection.D.ToString())
                    {
                        isShipLocationValid = yCoordinate - 2 >= boardOrigin;
                    }
                }
                else isShipLocationValid = isShipCoordinateValid;
            }
            catch (Exception)
            {
                isShipLocationValid = false;
                //TODO: Handle this better with logging
            }

            return isShipLocationValid;
        }

        public bool ValidateMissileCoordinates(char x, int y)
        {
            var isMissileCoordinateValid = false;
            try
            {
                var xCoordinateNumber = x.ConvertCharToNumber();
                isMissileCoordinateValid = xCoordinateNumber <= BoardDimension && y <= BoardDimension;
            }
            catch (Exception)
            {
                return isMissileCoordinateValid = false;
            }

            return isMissileCoordinateValid;
        }

        public bool IsShipHitByMissile(char x, int y, IShip currentShip)
        {
            var isShipHitByMissile = false;
            try
            {
                var ShipHitByMissile = currentShip.ShipCoordinates.SingleOrDefault(shipCoordinate => shipCoordinate.X == x && shipCoordinate.Y == y);
                if (ShipHitByMissile != null)
                {
                    currentShip.ShipCoordinates.Remove(ShipHitByMissile);
                    isShipHitByMissile = true;
                }
                return isShipHitByMissile;
            }
            catch (Exception)
            {
                //TODO: Handle this better with logging
                isShipHitByMissile = false;
            }

            return isShipHitByMissile;
        }

        public bool IsShipSunk(IShip currentShip)
        {
            return currentShip.ShipCoordinates.Count() == 0;
        }


    }
}
