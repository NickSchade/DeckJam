using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase { Unstarted, Sanctuary, Begin, Draw, Combat, Cleanup};

public static class Presets
{
    const string _basicAttackName = "Basic";

    public static Deck StartingDeck()
    {
        List<CardData> cards = new List<CardData>
        {
            Fighter(),
            ShieldBearer(),
            Militia()
        };
        return new Deck(cards);
    }

    public static Deck GoblinDeck()
    {
        List<CardData> cards = new List<CardData>()
        {
            Goblin(),
            FastGoblin(),
            ToughGoblin(),
            GoblinAssassin()
        };

        return new Deck(cards);
    }

    public static CardData ShieldBearer()
    {
        return new CardData("ShieldBearer", Allegiance.Player, 3, 8, new List<CardAbility> {new CardAbility(_basicAttackName, 1, new List<int> { 1 }) });
    }
    public static CardData Fighter()
    {
        return new CardData("Fighter", Allegiance.Player, 2, 4, new List<CardAbility> { new CardAbility(_basicAttackName, 2) });
    }
    public static CardData Militia()
    {
        return new CardData("Militia", Allegiance.Player, 1, 4, new List<CardAbility> { new CardAbility(_basicAttackName, 1) });
    }

    public static CardData Goblin()
    {
        return new CardData("Goblin", Allegiance.Enemy, 2, 1, new List<CardAbility> { new CardAbility(_basicAttackName, 1) });
    }
    public static CardData FastGoblin()
    {
        return new CardData("FastGoblin", Allegiance.Enemy, 1, 2, new List<CardAbility> { new CardAbility(_basicAttackName, 1) });
    }
    public static CardData ToughGoblin()
    {
        return new CardData("ToughGoblin", Allegiance.Enemy, 4, 5, new List<CardAbility> { new CardAbility(_basicAttackName, 1) });
    }
    public static CardData GoblinAssassin()
    {
        return new CardData("GoblinAssassin", Allegiance.Enemy, 3, 3, new List<CardAbility> { new CardAbility(_basicAttackName, 2) });
    }

}
