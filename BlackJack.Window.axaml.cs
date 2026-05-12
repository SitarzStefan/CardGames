using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace CardGames;

public partial class BlackjackWindow : Window
{
    private Deck deck = null!;

    private int playerScore = 0;
    private int player2Score = 0;

    private GameMode mode;

    private int currentPlayer = 1;

    private bool isGameOver = false;

    private int playerIndex = 0;
    private int opponentIndex = 0;

    public BlackjackWindow()
    {
        InitializeComponent();
        mode = GameMode.Bot;
        StartGame();
    }

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
        isGameOver = false;

        playerIndex = 0;
        opponentIndex = 0;

        PlayerCardsPanel.Items.Clear();
        OpponentCardsPanel.Items.Clear();

        PlayerText.Text = "Gracz 1: 0";
        OpponentText.Text = "Gracz 2: 0";
        ResultText.Text = "";

        HitButton.IsEnabled = true;

        OpponentTitle.Text = mode == GameMode.Bot
            ? "BOT"
            : "GRACZ 2";
    }

    private void Hit_Click(object? sender, RoutedEventArgs e)
    {
        if (isGameOver)
            return;

        var card = deck.Draw();

        if (mode == GameMode.TwoPlayers)
        {
            if (currentPlayer == 1)
            {
                playerScore += card.Value;

                PlayerText.Text = $"Gracz 1: {playerScore}";

                AddCard(
                    PlayerCardsPanel,
                    card.ImagePath,
                    ref playerIndex
                );
            }
            else
            {
                player2Score += card.Value;

                OpponentText.Text = $"Gracz 2: {player2Score}";

                AddCard(
                    OpponentCardsPanel,
                    card.ImagePath,
                    ref opponentIndex
                );
            }
        }
        else
        {
            playerScore += card.Value;

            PlayerText.Text = $"Gracz 1: {playerScore}";

            AddCard(
                PlayerCardsPanel,
                card.ImagePath,
                ref playerIndex
            );
        }

        UpdateCenter(card.ImagePath);

        if (playerScore > 21 || player2Score > 21)
        {
            isGameOver = true;
            FinishGame();
        }
    }

    private void Stop_Click(object? sender, RoutedEventArgs e)
    {
        if (mode == GameMode.TwoPlayers)
        {
            if (currentPlayer == 1)
            {
                currentPlayer = 2;

                ResultText.Text = "Tura Gracza 2";

                return;
            }

            FinishGame();
        }
        else
        {
            _ = BotPlay();
        }
    }

    private async Task BotPlay()
    {
        int botScore = 0;

        while (botScore < 17)
        {
            await Task.Delay(1000);

            var card = deck.Draw();

            botScore += card.Value;

            OpponentText.Text = $"Bot: {botScore}";

            AddCard(
                OpponentCardsPanel,
                card.ImagePath,
                ref opponentIndex
            );

            UpdateCenter(card.ImagePath);
        }

        player2Score = botScore;

        FinishGame();
    }

    private void AddCard(
        ItemsControl panel,
        string path,
        ref int index)
    {
        if (!File.Exists(path))
            return;

        using var stream = File.OpenRead(path);

        var bmp = new Bitmap(stream);

        var img = new Image
        {
            Source = bmp,
            Width = 150,
            Height = 200
        };

        panel.Items.Add(img);

        index++;

        double availableWidth = 180;

        double cardWidth = 150;

        double offset;

        if (index <= 1)
        {
            offset = 0;
        }
        else
        {
            offset =
                (availableWidth - cardWidth)
                / (index - 1);

            if (offset < 20)
                offset = 20;
        }

        for (int i = 0; i < panel.Items.Count; i++)
        {
            if (panel.Items[i] is Image cardImage)
            {
                Canvas.SetLeft(
                    cardImage,
                    i * offset
                );

                Canvas.SetTop(cardImage, 0);
            }
        }
    }

    private void UpdateCenter(string path)
    {
        if (File.Exists(path))
        {
            using var stream = File.OpenRead(path);

            PlayerCardImage.Source =
                new Bitmap(stream);
        }
    }

    private void FinishGame()
    {
        isGameOver = true;

        HitButton.IsEnabled = false;

        bool p1Bust = playerScore > 21;
        bool p2Bust = player2Score > 21;

        if (p1Bust && p2Bust)
        {
            ResultText.Text = "REMIS";
            return;
        }

        if (p1Bust)
        {
            ResultText.Text = "WYGRA£ GRACZ 2";
            return;
        }

        if (p2Bust)
        {
            ResultText.Text = "WYGRA£ GRACZ 1";
            return;
        }

        if (playerScore > player2Score)
        {
            ResultText.Text = "WYGRA£ GRACZ 1";
        }
        else if (playerScore < player2Score)
        {
            ResultText.Text = "WYGRA£ GRACZ 2";
        }
        else
        {
            ResultText.Text = "REMIS";
        }
    }
}