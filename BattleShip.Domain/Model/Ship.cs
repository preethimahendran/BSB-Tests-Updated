using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleShipGame.ApplicationService
{
    public class Ship : IShip
    {
        private static int _nextShipId;

        #region Constructor

        public Ship()
        {
            ShipId = ++_nextShipId;
            ShipCoordinates = new List<IShipCoordinate>();
        }
        #endregion

        #region Properties

        public int ShipId { get; set; }
        public ShipDirection ShipDirection { get; set; }

        public int ShipLength { get; set; }
        public List<IShipCoordinate> ShipCoordinates { get; set; }

        #endregion

    } 
}
