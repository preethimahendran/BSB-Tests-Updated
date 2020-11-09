namespace BattleShipGame.ApplicationService
{
    public interface IBattleBoard
    {
        int BoardId { get; set; }
        int BoardDimension { get; set; }

        IPlayer Player { get; set; }
        public IShip Ship { get; set; }
    }
}