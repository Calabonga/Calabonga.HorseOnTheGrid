namespace Calabonga.HorseOnTheGrid.Core;

/// <summary>
/// Правила хода шахматного коня: буквой «Г», две клетки в одном направлении и одна в перпендикулярном.
/// </summary>
public static class KnightMoves
{
    /// <summary>Восемь возможных смещений коня по часовой стрелке, начиная с хода «вверх-вправо».</summary>
    private static readonly (int Columns, int Rows)[] Offsets =
    [
        (1, -2), (2, -1), (2, 1), (1, 2),
        (-1, 2), (-2, 1), (-2, -1), (-1, -2)
    ];

    /// <summary>Все клетки поля размером <paramref name="size"/>, куда конь может пойти из <paramref name="from"/>.</summary>
    public static IEnumerable<Position> From(Position from, int size)
    {
        foreach (var (columns, rows) in Offsets)
        {
            var target = from.Offset(columns, rows);
            if (target.Column >= 0 && target.Column < size && target.Row >= 0 && target.Row < size)
            {
                yield return target;
            }
        }
    }

    /// <summary>
    /// Проверяет, что из <paramref name="from"/> в <paramref name="to"/> можно попасть ходом коня.
    /// Приём: модули смещений по осям равны 1 и 2 (в любом порядке) тогда и только тогда,
    /// когда их произведение равно 2.
    /// </summary>
    public static bool IsKnightMove(Position from, Position to)
    {
        var columns = Math.Abs(from.Column - to.Column);
        var rows = Math.Abs(from.Row - to.Row);
        return columns * rows == 2;
    }
}
