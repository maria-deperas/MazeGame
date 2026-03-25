using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGame.Input
{
    // interfejs dla handlerów wejścia
    public interface IInputHandler
    {
        IInputHandler SetNext(IInputHandler handler);
        bool Handle(ConsoleKey key);
    }

    // abstrakcyjna klasa bazowa, która implementuje chain of responsibility
    public abstract class BaseInputHandler : IInputHandler
    {
        // referencja do następnego handlera w łańcuchu
        private IInputHandler _nextHandler;

        // metoda do ustawiania następnego handlera
        public IInputHandler SetNext(IInputHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        // metoda do obsługi klawisza, jeśli nie obsługuje go ten handler, przekazuje do następnego
        public virtual bool Handle(ConsoleKey key)
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle(key);
            }
            return false;
        }
    }
}
