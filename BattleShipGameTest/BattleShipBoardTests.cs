using BattleShipGame.ApplicationService;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace BattleShipGame.Test
{
    [TestClass]
    public class BattleShipBoardTests
    {  
        private Mock<IBattleShipFactory> _battleShipFactoryMock;
        private Mock<IShip> _shipMock;
        private Mock<IShipCoordinate> _shipCoordinatesMock;
        private int _shipLength;
        private BattleBoardService _battleBoardService;
        private Mock<IPlayer> _playerMock;


        [TestInitialize]
        public void TestSetup()
        {
            _battleShipFactoryMock = new Mock<IBattleShipFactory>();

            _battleBoardService = new BattleBoardService(_battleShipFactoryMock.Object);

            _playerMock = new Mock<IPlayer>();
            _shipMock = new Mock<IShip>();

            _shipLength = 3;
        }

       
        [TestMethod]
        [TestCategory("Ship")]
        public void ValidateShipLocationOnBoard_ValidShipLocation_ReturnsTrue()
        {
            _battleBoardService.BoardDimension = 5;

            var result = _battleBoardService.ValidateShipLocationOnBoard("R", 'C', 2);
            Assert.AreEqual(true,result);
        }

        [TestMethod]
        [TestCategory("Ship")]
        public void ValidateShipLocationOnBoard_InvalidShipLocation_ReturnsFalse()
        {
            _battleBoardService.BoardDimension = 5;

            var result = _battleBoardService.ValidateShipLocationOnBoard("R",'E', 2);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        [TestCategory("Missile")]
        public void ShipHitByMissile_MissileTargetInShipLocation_ReturnsTrue()
        {   
            MockShip(_shipLength);

            var result = _battleBoardService.IsShipHitByMissile('A', 1, _shipMock.Object);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [TestCategory("Missile")]
        public void ShipHitByMissile_MissileTargetOutOfShipLocation_ReturnsFalse()
        {   
            MockShip(_shipLength);

            var result = _battleBoardService.IsShipHitByMissile('D', 4, _shipMock.Object);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        [TestCategory("Missile")]
        public void IsShipSunk_ShipWithOneCoordinateNotHit_ReturnsFalse()
        {
            MockShip(_shipLength);

            var result = _battleBoardService.IsShipSunk(_shipMock.Object);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        [TestCategory("Missile")]
        public void IsShipSunk_ShipWithNoCoordinatesLeftToBeHit_ReturnsTrue()
        {
            MockShip(0);

            var result = _battleBoardService.IsShipSunk(_shipMock.Object);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [TestCategory("MissileTargetLocation")]
        public void ValidateMissileTargetCoordinate_yCoordinateOutsideBoardDimension_ReturnsFalse()
        {
            _battleBoardService.BoardDimension = 3;
            var xcoordinate = 'D';
            var ycoordinate = 2;

            var result = _battleBoardService.ValidateMissileCoordinates(xcoordinate, ycoordinate);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        [TestCategory("MissileTargetLocation")]
        public void ValidateMissileTargetCoordinate_xCoordinateOutsideBoardDimension_ReturnsFalse()
        {
            _battleBoardService.BoardDimension = 3;
            var xcoordinate = 'A';
            var ycoordinate = 4;

            var result = _battleBoardService.ValidateMissileCoordinates(xcoordinate, ycoordinate);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        [TestCategory("MissileTargetLocation")]
        public void ValidateMissileTargetCoordinate_CoordinateInsideBoardDimension_ReturnsTrue()
        {
            _battleBoardService.BoardDimension = 3;
            var xcoordinate = 'A';
            var ycoordinate = 2;

            var result = _battleBoardService.ValidateMissileCoordinates(xcoordinate, ycoordinate);

            Assert.AreEqual(true, result);

        }

        [TestMethod]
        [TestCategory("BattleBoard")]
        public void CreateBoard_2player3by3Board_ReturnsBoardCollection()
        {
            int noOfPlayers = 2;
            int boardDimension = 3;

            _battleShipFactoryMock.Setup(x => x.CreateBattleBoards(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => new List<BattleBoard> { 
                    new BattleBoard() { BoardDimension = boardDimension, Player = _playerMock.Object, Ship = _shipMock.Object },
                    new BattleBoard() { BoardDimension = boardDimension, Player = _playerMock.Object, Ship = _shipMock.Object }
                });
               
            var result = _battleBoardService.CreateBoard(boardDimension, noOfPlayers);


            //Making multiple asserts in one test method for demo purpose - This should be avoided 

            _battleShipFactoryMock.Verify(x => x.CreateBattleBoards(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            CollectionAssert.AllItemsAreNotNull(_battleBoardService._battleBoardCollection);

            CollectionAssert.AllItemsAreInstancesOfType(_battleBoardService._battleBoardCollection,typeof(IBattleBoard));

            CollectionAssert.AllItemsAreUnique(_battleBoardService._battleBoardCollection);

            Assert.AreEqual(true,result);
        }

        [TestMethod]
        [TestCategory("BattleBoard")]
        public void CreateBoard_2player3by3Board_ReturnsEmptyCollection()
        {
            int noOfPlayers = 2;
            int boardDimension = 3;

            _battleShipFactoryMock.Setup(x => x.CreateBattleBoards(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => new List<BattleBoard>());

            var result = _battleBoardService.CreateBoard(boardDimension, noOfPlayers);


            _battleShipFactoryMock.Verify(x => x.CreateBattleBoards(It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            Assert.IsTrue(_battleBoardService._battleBoardCollection.Count == 0);
        }

        [TestMethod]
        [TestCategory("Ship")]
        public void GetCurrentPlayerShipOnBattleBoard_playerNotSetupInBattleBoard_ReturnsNull()
        {
            SetupBattleBoard();

            int playerId = 2;

            var result = _battleBoardService.GetCurrentPlayerShipOnBattleBoard(playerId);

            //Making multiple asserts in one test method for demo purpose - This should be avoided 

            Assert.IsInstanceOfType(result, typeof(IShip));

            Assert.IsNotNull(result);

            Assert.IsNotInstanceOfType(result, typeof(IBattleBoard));

        }

        [TestMethod]
        [TestCategory("Ship")]
        public void GetCurrentPlayerShipOnBattleBoard_playerId_ReturnsShipObjectForthePlayerPassed()
        {
            SetupBattleBoard();

            int playerId = 4;

            var result = _battleBoardService.GetCurrentPlayerShipOnBattleBoard(playerId);

            //Making multiple asserts in one test method for demo purpose - This should be avoided 

            Assert.IsNull(result);

            Assert.IsNotInstanceOfType(result, typeof(IShip));
        }


        [TestCleanup]
        public void TestCleanUp()
        {
            _battleBoardService = null;
            _battleShipFactoryMock = null;
            if (_shipMock != null) { _shipMock = null; }
            if (_playerMock != null) { _shipMock = null; }
            if (_playerMock != null) { _shipMock = null; }
        }

        private void MockShip(int noOfShipCoordinates)
        {
            _shipMock = new Mock<IShip>();
            _shipMock.Setup(x => x.ShipId).Returns(1);
            _shipMock.Setup(x => x.ShipLength).Returns(_shipLength);
            _shipMock.Setup(x => x.ShipDirection).Returns(ShipDirection.U);
            var shipCoordinates = new List<IShipCoordinate>();

            for (int i = 1; i <= noOfShipCoordinates; i++)
            {
                _shipCoordinatesMock = new Mock<IShipCoordinate>();
                _shipCoordinatesMock.Setup(x => x.ShipId).Returns(_shipMock.Object.ShipId);
                _shipCoordinatesMock.Setup(x => x.X).Returns('A');
                _shipCoordinatesMock.Setup(x => x.Y).Returns(i);
                shipCoordinates.Add(_shipCoordinatesMock.Object);
            }
            _shipMock.Setup(x => x.ShipCoordinates).Returns(shipCoordinates);
        }

        private void SetupBattleBoard()
        {
            int noOfPlayers = 2;
            int boardDimension = 3;
            List<BattleBoard> battleBoardCollectionMock = new List<BattleBoard>();

            for (int i = 1; i <= noOfPlayers; i++)
            {
                _playerMock = new Mock<IPlayer>();
                _playerMock.Setup(x => x.PlayerId).Returns(i);

                _shipMock = new Mock<IShip>();

                battleBoardCollectionMock.Add(new BattleBoard() { BoardDimension = boardDimension, Player = _playerMock.Object, Ship = _shipMock.Object });
            }

            _battleShipFactoryMock.Setup(x => x.CreateBattleBoards(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(() => battleBoardCollectionMock);

            _battleBoardService.CreateBoard(boardDimension, noOfPlayers);
        }

    }
}

