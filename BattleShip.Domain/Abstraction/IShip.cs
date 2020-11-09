using System.Collections.Generic;

namespace BattleShipGame.ApplicationService
{
    public interface IShip
    {
      
        ShipDirection ShipDirection { get; set; }
        int ShipId { get; set; }
        List<IShipCoordinate> ShipCoordinates { get; set; }
        int ShipLength { get; set; }
        

        
       

    }
}