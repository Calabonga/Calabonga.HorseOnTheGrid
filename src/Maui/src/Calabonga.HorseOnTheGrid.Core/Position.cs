namespace Calabonga.HorseOnTheGrid.Core;

/// <summary>
/// Позиция клетки на поле. Индексы начинаются с нуля:
/// Column — номер столбца слева направо, Row — номер строки сверху вниз.
/// Игроку клетки показываются с номерами от 1 (см. <see cref="ToString"/>).
/// </summary>
public readonly record struct Position(int Column, int Row)
{
    /// <summary>Возвращает позицию, смещённую на заданное число столбцов и строк.</summary>
    public Position Offset(int columns, int rows) => new(Column + columns, Row + rows);

    /// <summary>Номер клетки для человека: левая верхняя клетка — x:1, y:1.</summary>
    public override string ToString() => $"x:{Column + 1}, y:{Row + 1}";
}
