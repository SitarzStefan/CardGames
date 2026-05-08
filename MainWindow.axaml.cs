using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CardGames;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Blackjack_Click(object? sender, RoutedEventArgs e)
    {
        
        new GameModeWindow().Show();
    }

    private void Memory_Click(object? sender, RoutedEventArgs e)
    {
        new Window
        {
            Title = "Memory",
            Width = 500,
            Height = 400
        }.Show();
    }

    private void ThirdGame_Click(object? sender, RoutedEventArgs e)
    {
        ColorGuessWindow window = new ColorGuessWindow();
        window.Show();
    }
    private void ShowHistory_Click(object? sender, RoutedEventArgs e)
    {
        var historyWin = new HistoryWindow();
        historyWin.ShowDialog(this); // Otwiera historiê jako okno modalne
    }
}