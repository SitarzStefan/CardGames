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
        new Window
        {
            Title = "Oczko (Blackjack)",
            Width = 500,
            Height = 400
        }.Show();
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
        new Window
        {
            Title = "Gra nr 3",
            Width = 500,
            Height = 400
        }.Show();
    }
}