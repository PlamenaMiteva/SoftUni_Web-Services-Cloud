using System;
using System.Collections.Generic;

namespace BattleshipsGame
{
    class CommandParser
    {
        private const char CommandNameSeparator = '(';
        private const char CommandParameterSeparator = ';';
        private const char CommandValueSeparator = '=';

        private CommandTypes type;
        private IDictionary<string, string> parameters = new Dictionary<string, string>();

        public CommandParser(string input)
        {
            this.TranslateInput(input);
        }

        public CommandTypes Type
        {
            get
            {
                return this.type;
            }

            private set
            {
                this.type = value;
            }
        }

        public IDictionary<string, string> Parameters
        {
            get
            {
                return this.parameters;
            }

            private set
            {
                if (value == null)
                {
                    Console.WriteLine("The command parameters are required.");
                    this.IsValid = false;
                }

                this.parameters = value;
            }
        }

        public bool IsValid { get; set; } = true;

        public static CommandParser Parse(string input)
        {
            return new CommandParser(input);
        }

        private void TranslateInput(string input)
        {
            try
            {
                int parametersBeginning = input.IndexOf(CommandNameSeparator);
                int typeEndIndex = parametersBeginning > 0 ? parametersBeginning : input.Length;
                this.Type =
                    (CommandTypes)
                        Enum.Parse(typeof (CommandTypes), input.Substring(0, typeEndIndex).ToLower(), true);

                if (parametersBeginning > 0)
                {
                    var parametersKeysAndValues = input.Substring(parametersBeginning + 1,
                        input.Length - parametersBeginning - 2)
                        .Split(new[] {CommandParameterSeparator}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var parameter in parametersKeysAndValues)
                    {
                        var split = parameter.Split(new[] {CommandValueSeparator}, StringSplitOptions.RemoveEmptyEntries);
                        this.Parameters.Add(split[0].ToLower(), split[1]);
                    }
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid command");
                this.IsValid = false;
            }
        }
    }
}
