using System;

namespace BattleshipsGame
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Available Commands\n{\n  register(email=example@;password=example;confirmpassword=example)\n  login(email=example@;password=example)\n  creategame\n  availableplayers\n  jointogame(playeremail=example)\n  play(positionx=x;positiony=y)\n  exit\n}\n");

            var engine = new Engine();
            engine.Run();
        }
    }
}
