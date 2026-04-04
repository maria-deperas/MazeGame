using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Commands.Exploration
{
    // interfejs dla handlerów wejścia
    public interface IInputHandler
    {
        IInputHandler SetNext(IInputHandler handler); // podpina kolejny element łańcucha, zwraca ten element
        IInputHandler GetNext();
        bool Handle(ConsoleKey key); // prónuje obsłużyć klawisz, zwraca true jeśli obsłużył, false jeśli nie
        List<string> GetGuiInstructions();
    }

    // abstrakcyjna klasa bazowa, która implementuje chain of responsibility
    public abstract class BaseInputHandler : IInputHandler
    {
        // przechowuje informację o następnym handlerze w łańcuchu
        private IInputHandler _nextHandler;

        // metoda do ustawiania następnego handlera
        public IInputHandler SetNext(IInputHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }
        public IInputHandler GetNext() => _nextHandler;

        // metoda do obsługi klawisza, jeśli nie obsługuje go ten handler, przekazuje do następnego
        public virtual bool Handle(ConsoleKey key)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(key);
            }
            return false;
        }
        public virtual List<string> GetGuiInstructions() => new List<string>();
    }
}
