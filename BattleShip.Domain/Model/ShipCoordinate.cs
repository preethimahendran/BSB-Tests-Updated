namespace BattleShipGame.ApplicationService
{
    /// <summary>
    /// Ship's Child
    /// </summary>
    public class ShipCoordinate : IShipCoordinate
    {
        public int ShipId { get; set; }
        public char X { get; set; }
        public int Y { get; set; }
    }
}
