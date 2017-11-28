namespace BusTicketSystem.Client.Core
{
    using System;
    using BusTicketSystem.Client.Interfaces;

    public class Engine : IEngine
    {
        private readonly ICommandDispatcher dispatcher;
        private readonly IWriter writer;
        private readonly IReader reader;
        public Engine(ICommandDispatcher dispatcher, IWriter writer, IReader reader)
        {
            this.dispatcher = dispatcher;
            this.writer = writer;
            this.reader = reader;
        }
        public void Run()
        {
            try
            {
                var input = reader.ReadLine();
                var data = input.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries);
                var result = dispatcher.DispatchCommand(data);
                writer.WriteLine(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}