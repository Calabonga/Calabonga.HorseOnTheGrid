using Calabonga.HorseOnTheGridMvvm.Game.Entities;
using Calabonga.HorseOnTheGridMvvm.Game.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Calabonga.HorseOnTheGridMvvm.UserControls;

public class GridCell : Control
{
    public int Row { get; set; }

    public int Column { get; set; }

    #region DependencyProperty CellType

    public static readonly DependencyProperty CellTypeProperty = DependencyProperty.Register(
        nameof(CellType), typeof(CellType), typeof(GridCell), new PropertyMetadata(default(CellType),
            OnCellTypePropertyChanged));

    public CellType CellType
    {
        get => (CellType)GetValue(CellTypeProperty);
        set => SetValue(CellTypeProperty, value);
    }

    private static void OnCellTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var gridCell = (GridCell)d;
        var value = (CellType)e.NewValue;
        gridCell.ImageSource = ImageHelper.CellToImage[value];
    }

    #endregion

    #region DepdencyProperty ImageSource

    public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
        nameof(ImageSource), typeof(ImageSource), typeof(GridCell), new PropertyMetadata(default(ImageSource)));

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }

    #endregion

    #region DepdencyProperty Title

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title), typeof(string), typeof(GridCell), new PropertyMetadata(default(string)));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    #endregion

    #region DepdencyProperty SelectCommand

    public static readonly DependencyProperty SelectCommandProperty = DependencyProperty.Register(
        nameof(SelectCommand), typeof(ICommand), typeof(GridCell), new PropertyMetadata(default(ICommand)));

    public ICommand SelectCommand
    {
        get => (ICommand)GetValue(SelectCommandProperty);
        set => SetValue(SelectCommandProperty, value);
    }

    #endregion

}
