namespace Calabonga.HorseOnTheGrid.Mobile.ViewModels;

/// <summary>Как клетка выглядит на экране.</summary>
public enum CellState
{
    /// <summary>Клетка не посещена, и сейчас на неё пойти нельзя.</summary>
    Empty,

    /// <summary>Клетка доступна для следующего хода — подсвечивается.</summary>
    Available,

    /// <summary>Конь уже был здесь — клетка окрашена и показывает номер хода.</summary>
    Visited,

    /// <summary>Здесь стоит конь.</summary>
    Current
}
