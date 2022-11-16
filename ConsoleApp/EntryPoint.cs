using System;

namespace ConsoleApp
{
    public class EntryPoint
    {
        private readonly RobotClient _robotClient;

        public EntryPoint(RobotClient robotClient)
        {
            _robotClient = robotClient;
        }

        public void MainRun()
        {
            Console.WriteLine("Please enter your robot commands (e.g. PLACE 1,2,EAST etc) or enter EXIT:");
            var input = Console.ReadLine();

            while (input != null && !input.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                _robotClient.RunCommand(input);
                input = Console.ReadLine();
            }
        }
    }
}
