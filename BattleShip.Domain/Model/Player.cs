using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipGame.ApplicationService
{
    public class Player : IPlayer
    {

        private static int _nextPlayerId;
        public Player()
        {
            PlayerId = ++_nextPlayerId;
        }
        
        public int PlayerId { get; set; }
    }
}
