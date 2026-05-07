using System;
using System.Collections.Generic;
using System.Linq;

namespace CardGames;

public class Deck
{
    private List<Card> cards;

    public Deck()
    {
        cards = new List<Card>();

        string[] suits = { "kier", "karo", "trefl", "pik" };

        foreach (var suit in suits)
        {
            for (int i = 2; i <= 10; i++)
            {
                cards.Add(new Card
                {
                    Name = $"{i}_{suit}",
                    Value = i,
                    ImagePath = $"Assets/{i}_{suit}.jpg"
                });
            }

            cards.Add(new Card
            {
                Name = $"Jopek_{suit}",
                Value = 2,
                ImagePath = $"Assets/Jopek_{suit}.jpg"
            });

            cards.Add(new Card
            {
                Name = $"Dama_{suit}",
                Value = 3,
                ImagePath = $"Assets/Dama_{suit}.jpg"
            });

            cards.Add(new Card
            {
                Name = $"Krol_{suit}",
                Value = 4,
                ImagePath = $"Assets/Krol_{suit}.jpg"
            });

            cards.Add(new Card
            {
                Name = $"As_{suit}",
                Value = 11,
                ImagePath = $"Assets/As_{suit}.jpg"
            });
        }
    }

    public void Shuffle()
    {
        var rnd = new Random();
        cards = cards.OrderBy(x => rnd.Next()).ToList();
    }

    public Card Draw()
    {
        var card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}