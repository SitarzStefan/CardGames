using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.IO;

namespace CardGames;

public partial class BlackjackWindow : Window
{
    private Deck deck;
    private int playerScore;

    public BlackjackWindow()
    {
        InitializeComponent();
        StartGame();
    }

    private void StartGame()
    {
        deck = new Deck();
        deck.Shuffle();

        playerScore = 0;

        PlayerText.Text = "Punkty: 0";
        ResultText.Text = "";

        PlayerCardImage.Source = null;
    }

    private void Hit_Click(object? sender, RoutedEventArgs e)
    {
        var card = deck.Draw();

        playerScore += card.Value;

        PlayerText.Text = $"Punkty: {playerScore}";
        ResultText.Text = "";

        // ?? PEWNE £ADOWANIE OBRAZKA
        if (File.Exists(card.ImagePath))
        {
            using var stream = File.OpenRead(card.ImagePath);
            PlayerCardImage.Source = new Bitmap(stream);
        }
        else
        {
            ResultText.Text = $"BRAK PLIKU: {card.ImagePath}";
        }

        if (playerScore == 21)
            ResultText.Text = "WYGRA£EŒ!";
        else if (playerScore > 21)
            ResultText.Text = "PRZEGRA£EŒ!";
    }

    private void Reset_Click(object? sender, RoutedEventArgs e)
    {
        StartGame();
    }
}