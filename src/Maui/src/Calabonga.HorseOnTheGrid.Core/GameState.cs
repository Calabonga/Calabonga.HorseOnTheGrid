namespace Calabonga.HorseOnTheGrid.Core;

/// <summary>Состояние партии.</summary>
public enum GameState
{
    /// <summary>Партия идёт: есть хотя бы один доступный ход.</summary>
    InProgress,

    /// <summary>Победа: посещены все клетки поля.</summary>
    Won,

    /// <summary>Тупик: клетки ещё остались, но ходов из текущей позиции нет.</summary>
    Stuck
}
