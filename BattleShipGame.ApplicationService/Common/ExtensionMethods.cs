using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShipGame.ApplicationService
{
    public static class ExtensionMethods
    {
        public static char NextAlphabet(this char input)
        {
            return input == 'z' ? 'a' : (char)((int)input + 1);
        }
        public static char PreviousAlphabet(this char input)
        {
            return input == 'a' ? 'z' : (char)((int)input - 1);
        }
        public static char ConvertNumberToChar(this int input)
        {
            return ((char)(64 + input));
        }
        public static int ConvertCharToNumber(this char input)
        {
            return (Char.ToUpper(input) - 64);
        }
    }
}

