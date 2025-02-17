﻿using Calabonga.HorseOnTheGridMvvm.Core;
using Calabonga.HorseOnTheGridMvvm.Game;
using Calabonga.HorseOnTheGridMvvm.UserControls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Calabonga.HorseOnTheGridMvvm.ViewModels;

/// <summary>
/// ViewModel for MainWindow
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private HorseGame? _game;

    public MainWindowViewModel()
    {
        Title = "HORSE ON THE GRID (MVVM)";
    }

    #region properties

    #region property CanShowBoard

    public bool CanShowBoard => !IsGameNotRunning || _game?.Score > 0;

    #endregion

    #region property IsGameNotRunning

    public bool IsGameNotRunning => !IsGameRunning;

    #endregion

    #region property IsGameRunning

    /// <summary>
    /// Property IsGameRunning
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsGameNotRunning))]
    [NotifyPropertyChangedFor(nameof(CanShowBoard))]
    private bool _isGameRunning;

    #endregion

    #region property Cells

    /// <summary>
    /// Property Cells
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<GridCell> _cells = [];

    #endregion

    #region property Rows

    /// <summary>
    /// Property Rows
    /// </summary>
    [ObservableProperty]
    private int _rows = 10;

    #endregion

    #region property Columns

    /// <summary>
    /// Property Columns
    /// </summary>
    [ObservableProperty]
    private int _columns = 10;

    #endregion

    #region property Score

    /// <summary>
    /// Property Score
    /// </summary>
    [ObservableProperty] private string _score = "SCORE: 1";

    #endregion

    #endregion

    #region commands

    #region command StartGameCommand

    [RelayCommand]
    private void StartGame()
    {
        if (!IsGameRunning)
        {
            IsGameRunning = true;
        }

        RunGame();

    }
    #endregion

    #region MoveHorseCommand

    [RelayCommand]
    private void MoveHorse(object args)
    {
        if (!IsGameRunning)
        {
            return;
        }

        var grid = (GridCell)args;
        _game?.MoveHorse(grid);

        if (_game?.GameOver == true)
        {
            Score = $"GAME OVER: {_game.Score}";
            IsGameRunning = false;
        }

        Draw();
    }

    #endregion

    #endregion

    private void RunGame()
    {
        SetupGrid();

        Draw();
    }

    private void Draw()
    {
        Score = $"SCORE: {_game?.Score}";
    }

    private void SetupGrid()
    {
        if (Cells.Count > 0)
        {
            Cells.Clear();
        }

        _game = new HorseGame(Rows, Columns);

        foreach (var cell in _game.GridCells)
        {
            cell.SelectCommand = MoveHorseCommand;
        }

        Cells = new ObservableCollection<GridCell>(_game.GridCells);
    }
}
