using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace CardGames;

public partial class BlackjackWindow : Window
{
    private Deck deck = null!;

    private int playerScore;
    private int player2Score;

    private GameMode mode;
    private int currentPlayer = 1;

    private bool isBlocked = false;

    // konstruktor dla preview / fallback
    public BlackjackWindow()
    {
        InitializeComponent();
        mode = GameMode.Bot;
        StartGame();
    }

    // konstruktor gry
    public BlackjackWindow(GameMode mode)
    {
        InitializeComponent();
        this.mode = mode;
        StartGame();
    }

    private void StartGame()
    {
        deck = new Deck();
        deck.Shuffle();

        playerScore = 0;
        player2Score = 0;
        currentPlayer = 1;
        isBlocked = false;

        PlayerText.Text = "Gracz 1: 0";
        BotText.Text = "";
        ResultText.Text = "";
    }

    private void Hit_Click(object? sender, RoutedEventArgs e)
    {
        if (isBlocked || deck == null)
            return;

        var card = deck.Draw();

        if (mode == GameMode.Bot)
        {
            playerScore += card.Value;
            PlayerText.Text = $"Gracz: {playerScore}";
        }
        else
        {
            if (currentPlayer == 1)
            {
                playerScore += card.Value;
                PlayerText.Text = $"Gracz 1: {playerScore}";
            }
            else
            {
                player2Score += card.Value;
                PlayerText.Text = $"Gracz 2: {player2Score}";
            }
        }

        if (File.Exists(card.ImagePath))
        {
            using var stream = File.OpenRead(card.ImagePath);
            PlayerCardImage.Source = new Bitmap(stream);
        }

        if (playerScore > 33 || player2Score > 33)
        {
            isBlocked = true;
            ResultText.Text = "Przekroczono 33!";
        }
    }

    private async void Stop_Click(object? sender, RoutedEventArgs e)
    {
        if (mode == GameMode.TwoPlayers)
        {
            if (currentPlayer == 1)
            {
                currentPlayer = 2;
                ResultText.Text = "Tura Gracza 2";
            }
            else
            {
                EndGame();
            }
        }
        else
        {
            await BotPlay();
        }
    }

    private async Task BotPlay()
    {
        ResultText.Text = "Bot gra...";

        int botScore = 0;

        while (botScore < 17)
        {
            await Task.Delay(2000);

            var card = deck.Draw();
            botScore += card.Value;

            BotText.Text = $"Bot: {botScore}";
        }

        if (playerScore > botScore && playerScore <= 33)
            ResultText.Text = "WYGRA£EŒ";
        else
            ResultText.Text = "PRZEGRA£EŒ";
    }

    private void EndGame()
    {
        if (playerScore > player2Score && playerScore <= 33)
            ResultText.Text = "Wygra³ Gracz 1";
        else
            ResultText.Text = "Wygra³ Gracz 2";
    }
}