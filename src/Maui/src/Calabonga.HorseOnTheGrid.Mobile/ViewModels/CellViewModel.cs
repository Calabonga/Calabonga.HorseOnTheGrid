using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Calabonga.HorseOnTheGrid.Core;

namespace Calabonga.HorseOnTheGrid.Mobile.ViewModels;

/// <summary>Одна клетка поля на экране.</summary>
public partial class CellViewModel : ObservableObject
{
    private readonly Action<CellViewModel> _onSelected;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(OrderText))]
    private int _order;

    [ObservableProperty]
    private CellState _state;

    public CellViewModel(Position position, Action<CellViewModel> onSelected)
    {
        Position = position;
        _onSelected = onSelected;
    }

    public Position Position { get; }

    /// <summary>Столбец в сетке Grid (с нуля).</summary>
    public int Column => Position.Column;

    /// <summary>Строка в сетке Grid (с нуля).</summary>
    public int Row => Position.Row;

    /// <summary>Номер хода на посещённой клетке; пустая строка для непосещённой.</summary>
    public string OrderText => Order > 0 ? Order.ToString() : string.Empty;

    /// <summary>Нажатие на клетку. Решение, допустим ли ход, принимает модель представления игры.</summary>
    [RelayCommand]
    private void Select() => _onSelected(this);
}
