namespace BusTicketSystem.Client.Core
{
    using System;
    using BusTicketSystem.Client.Interfaces;
    public class ConsoleWriter : IWriter
    {
        public void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}