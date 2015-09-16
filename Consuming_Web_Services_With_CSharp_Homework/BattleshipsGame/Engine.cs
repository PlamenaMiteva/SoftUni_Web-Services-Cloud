using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BattleshipsGame
{
    class Engine
    {
        private Requester requester;
        private bool hasStarted = true;
        private Task task;

        public Engine()
        {
            this.requester = new Requester();
            this.task = Task.FromResult(false);
        }

        public void Run()
        {
            while (this.hasStarted)
            {
                var input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("The command name is required.");
                    continue;
                }

                var comand = CcmmandParser.Parse(input);
                if (comand.IsValid)
                {
                    this.ProcessCommand(comand);                    
                }
            }
        }

        private void ProcessCommand(CcmmandParser comand)
        {
            try
            {
                if (this.task.IsCompleted)
                {
                    switch (comand.Type)
                    {
                        case CommandTypes.Exit:
                            this.hasStarted = false;
                            break;
                        case CommandTypes.Register:
                            this.task = this.requester.RegisterAsync(comand.Parameters["email"],
                                comand.Parameters["password"],
                                comand.Parameters["confirmpassword"]);
                            break;
                        case CommandTypes.Login:
                            this.task = this.requester.LoginAsync(comand.Parameters["email"], comand.Parameters["password"]);
                            break;
                        case CommandTypes.CreateGame:
                            this.task = this.requester.CreateGameAsync();
                            break;
                        case CommandTypes.AvailablePlayers:
                            this.task = this.requester.PrintAvailablePlayersAsync();
                            break;
                        case CommandTypes.JoinToGame:
                            this.task = this.requester.JoinToGameAsync(comand.Parameters["playeremail"]);
                            break;
                        case CommandTypes.Play:
                            this.task = this.requester.PlayAsync(comand.Parameters["positionx"],
                                comand.Parameters["positiony"]);
                            break;
                        default:
                            Console.WriteLine("invalid command type");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Please try again after second");
                }
            }
            catch (KeyNotFoundException)
            {
                Console.WriteLine("Invalid paraameter name");
            }
        }
    }
}
