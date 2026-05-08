using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CardGames;

public partial class GameModeWindow : Window
{
    public GameModeWindow()
    {
        InitializeComponent();
    }

    private void TwoPlayers_Click(object? sender, RoutedEventArgs e)
    {
        new BlackjackWindow(GameMode.TwoPlayers).Show();
        Close();
    }

    private void Bot_Click(object? sender, RoutedEventArgs e)
    {
        new BlackjackWindow(GameMode.Bot).Show();
        Close();
    }
}