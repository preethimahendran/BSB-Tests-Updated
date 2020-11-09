namespace BattleShipGame.ApplicationService
{
    public class BattleBoard : IBattleBoard
    {
        private static int _nextBoardId;

        public BattleBoard()
        {
            BoardId = ++_nextBoardId;
        }

        public int BoardId { get; set; }
        public IPlayer Player { get; set; }
        public IShip Ship { get; set; }
        public int BoardDimension { get; set; }
        public int BoardWidth { get; set; }
    }
}
