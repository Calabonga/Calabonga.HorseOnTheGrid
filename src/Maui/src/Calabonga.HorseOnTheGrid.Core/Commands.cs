namespace Calabonga.HorseOnTheGrid.Core;

/// <summary>
/// Паттерн «Команда» (Command): действие, которое умеет выполниться и отмениться.
/// </summary>
public interface IGameCommand
{
    void Execute();

    void Undo();
}

/// <summary>Ход коня на клетку <c>target</c>. Запоминает, откуда был сделан ход, чтобы вернуть коня назад.</summary>
internal sealed class MoveCommand(Board board, Position target) : IGameCommand
{
    private Position? _previous;

    public void Execute()
    {
        _previous = board.Current;
        board.Visit(target);
    }

    public void Undo() => board.Unvisit(target, _previous);
}

/// <summary>История выполненных команд: последняя выполненная отменяется первой.</summary>
public sealed class CommandHistory
{
    private readonly Stack<IGameCommand> _done = new();

    public bool CanUndo => _done.Count > 0;

    public int Count => _done.Count;

    public void Execute(IGameCommand command)
    {
        command.Execute();
        _done.Push(command);
    }

    public void Undo()
    {
        if (!CanUndo)
        {
            throw new InvalidOperationException("Нет ходов для отмены.");
        }

        _done.Pop().Undo();
    }

    public void Clear() => _done.Clear();
}
