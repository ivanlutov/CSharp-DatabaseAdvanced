namespace BusTicketSystem.Client.Core
{
    using System;
    using BusTicketSystem.Client.Interfaces;

    public class ConsoleReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}