using System;
using System.Windows.Input;

namespace TeamBuilder.App
{
    public interface ICommandDispatcher
    {
        ICommand Dispatch(IServiceProvider provider, string commandName);
    }
}