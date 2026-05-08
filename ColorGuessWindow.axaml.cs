using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CardGames;

public partial class ColorGuessWindow : Window
{
    private Random random = new Random();
    private int score = 0;
    private int rounds = 0;
    private const int MAX_ROUNDS = 10;
    private string? currentCardFile = null;
    private List<string> deck = new List<string>();

    private readonly string[] allCards = {
        "2_pik.jpg", "3_pik.jpg", "4_pik.jpg", "5_pik.jpg", "6_pik.jpg", "7_pik.jpg", "8_pik.jpg", "9_pik.jpg", "10_pik.jpg",
        "2_trefl.jpg", "3_trefl.jpg", "4_trefl.jpg", "5_trefl.jpg", "6_trefl.jpg", "7_trefl.jpg", "8_trefl.jpg", "9_trefl.jpg", "10_trefl.jpg",
        "2_kier.jpg", "3_kier.jpg", "4_kier.jpg", "5_kier.jpg", "6_kier.jpg", "7_kier.jpg", "8_kier.jpg", "9_kier.jpg", "10_kier.jpg",
        "2_karo.jpg", "3_karo.jpg", "4_karo.jpg", "5_karo.jpg", "6_karo.jpg", "7_karo.jpg", "8_karo.jpg", "9_karo.jpg", "10_karo.jpg",
        "as_pik.jpg", "as_trefl.jpg", "as_kier.jpg", "as_karo.jpg",
        "krol_pik.jpg", "krol_trefl.jpg", "krol_kier.jpg", "krol_karo.jpg",
        "krolowa_pik.jpg", "krolowa_trefl.jpg", "krolowa_kier.jpg", "krolowa_karo.jpg",
        "jopek_pik.jpg", "jopek_trefl.jpg", "jopek_kier.jpg", "jopek_karo.jpg"
    };

    public ColorGuessWindow()
    {
        InitializeComponent();
        StartNewGame(); // Wywo³ujemy tutaj, ¿eby karta pojawi³a siê na starcie
    }

    private void StartNewGame()
    {
        score = 0;
        rounds = 0;
        // Tasowanie talii
        deck = allCards.OrderBy(x => random.Next()).ToList();

        // Pobieramy pierwsz¹ kartê i j¹ wyœwietlamy
        currentCardFile = DrawCard();
        UpdateUI(currentCardFile);

        ResultText.Text = "Pierwsza karta wylosowana. Wy¿ej czy ni¿ej?";
        HigherBtn.IsEnabled = true;
        LowerBtn.IsEnabled = true;
    }

    private string DrawCard()
    {
        if (deck.Count == 0) deck = allCards.ToList();
        string card = deck[0];
        deck.RemoveAt(0);
        return card;
    }

    private void UpdateUI(string cardFile)
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", cardFile);
        if (File.Exists(path))
        {
            CardImage.Source = new Bitmap(path);
        }
        ScoreText.Text = $"Punkty: {score} / {rounds}";
    }

    // Obs³uga przycisków
    private void Higher_Click(object? sender, RoutedEventArgs e) => PlayRound(true);
    private void Lower_Click(object? sender, RoutedEventArgs e) => PlayRound(false);

    private void Restart_Click(object? sender, RoutedEventArgs e) => StartNewGame();
    private void Exit_Click(object? sender, RoutedEventArgs e) => this.Close();

    private void PlayRound(bool guessedHigher)
    {
        if (rounds >= MAX_ROUNDS) return;

        string nextCardFile = DrawCard();
        int currentVal = GetValue(currentCardFile!);
        int nextVal = GetValue(nextCardFile);

        bool isTie = (nextVal == currentVal);
        bool correct = false;

        if (isTie)
        {
            // Jeœli jest remis, gracz zawsze dostaje punkt (opcja przyjazna graczowi)
            correct = true;
        }
        else if (guessedHigher && nextVal > currentVal)
        {
            correct = true;
        }
        else if (!guessedHigher && nextVal < currentVal)
        {
            correct = true;
        }

        // Obs³uga punktacji i komunikatów
        if (isTie)
        {
            ResultText.Text = $"REMIS! ({currentVal} na {currentVal}). Dostajesz punkt!";
            score++;
        }
        else if (correct)
        {
            ResultText.Text = "DOBRZE! ";
            score++;
        }
        else
        {
            ResultText.Text = "LE! ";
        }

        // Dodanie szczegó³ów do tekstu wyniku
        if (!isTie)
        {
            ResultText.Text += $"By³o: {currentVal}, Jest: {nextVal}";
        }

        rounds++;
        currentCardFile = nextCardFile;
        UpdateUI(currentCardFile);

        if (rounds >= MAX_ROUNDS)
        {
            EndGame();
        }
    }

    private int GetValue(string fileName)
    {
        if (fileName.Contains("as")) return 14;
        if (fileName.Contains("krol")) return 13;
        if (fileName.Contains("krolowa")) return 12;
        if (fileName.Contains("jopek")) return 11;
        string numPart = fileName.Split('_')[0];
        return int.TryParse(numPart, out int val) ? val : 0;
    }

    private void EndGame()
    {
        HigherBtn.IsEnabled = false;
        LowerBtn.IsEnabled = false;
        ResultText.Text = $"KONIEC! Wynik: {score}/{MAX_ROUNDS}. Zagraj jeszcze raz?";

        // Zapis do historii (opcjonalnie)
        try
        {
            var history = new GameHistory
            {
                PlayerName = "Gracz",
                GameName = "Wy¿ej/Ni¿ej",
                Result = $"{score}/{MAX_ROUNDS}",
                Date = DateTime.Now.ToString("g")
            };
            HistoryManager.SaveGame(history);
        }
        catch { /* Ignoruj b³¹d zapisu jeœli manager nie gotowy */ }
    }
}