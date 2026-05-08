using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Linq;

namespace CardGames;

public partial class HistoryWindow : Window
{
    public HistoryWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        // Pobieramy gry i odwracamy listê, ¿eby najnowsze by³y na górze
        var games = HistoryManager.LoadGames().AsEnumerable().Reverse().ToList();
        HistoryList.ItemsSource = games;
    }

    private void Close_Click(object? sender, RoutedEventArgs e) => this.Close();

    private void Clear_Click(object? sender, RoutedEventArgs e)
    {
        HistoryManager.ClearHistory();
        LoadData(); // Odœwie¿ listê (bêdzie pusta)
    }
}