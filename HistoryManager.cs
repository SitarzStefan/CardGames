using System.Collections.Generic;
using System.Text.Json;
using System.IO;


namespace CardGames;


public static class HistoryManager
{
    private static string filePath = "history.json";

    public static void SaveGame(GameHistory game)
    {
        List<GameHistory> games = LoadGames();

        games.Add(game);

        string json = JsonSerializer.Serialize(games, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        File.WriteAllText(filePath, json);
    }
    public static void ClearHistory()
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
    public static List<GameHistory> LoadGames()
    {
        if (!File.Exists(filePath))
        {
            return new List<GameHistory>();
        }

        string json = File.ReadAllText(filePath);

        return JsonSerializer.Deserialize<List<GameHistory>>(json)
               ?? new List<GameHistory>();
    }
}