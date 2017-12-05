namespace TeamBuilder.App.Core
{
    using System;
    using Commands;
    using System.Linq;

    public class Engine
    {
        private readonly IServiceProvider provider;

        public Engine(IServiceProvider provider)
        {
            this.provider = provider;
        }
        public void Run()
        {
            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    var args = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var commandName = args[0];
                    args = args.Skip(1).ToArray();
                    var command = CommandDispatcher.Dispatch(provider, commandName);
                    var result = command.Execute(args);
                    Console.WriteLine(result);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}