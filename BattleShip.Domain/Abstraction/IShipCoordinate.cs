namespace BattleShipGame.ApplicationService
{
    public interface IShipCoordinate
    {
        int ShipId { get; set; }
        char X { get; set; }
        int Y { get; set; }
    }
}