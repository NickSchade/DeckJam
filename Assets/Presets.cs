using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase { Unstarted, Sanctuary, Begin, Draw, AssignDice, Combat, Cleanup};

public static class Presets
{
    const string _basicAttackName = "Basic";
    
    public static Player PlayerTestMatch()
    {
        List<CardData> cards = new List<CardData> { Fighter(), ShieldBearer(), Militia() };
        return new Player(new Deck(cards), 2, 1, 3);
    }
    public static Player EnemyTestMatch()
    {
        List<CardData> cards = new List<CardData>()
        {
            Goblin(),
            FastGoblin(),
            ToughGoblin(),
            GoblinAssassin()
        };
        return new Player(new Deck(cards), 2, 2, 3);
    }

    public static CardData ShieldBearer()
    {
        Debug.Log("MAKING SHIELDBEARER");
        return new CardData("ShieldBearer", Allegiance.Player, 1, 8, new List<CardAbility> {new CardAbility(_basicAttackName, 1, new List<int> { 1 }) });
    }
    public static CardData Fighter()
    {
        return new CardData("Fighter", Allegiance.Player, 2, 4, new List<CardAbility> { new CardAbility(_basicAttackName, 2) });
    }
    public static CardData Militia()
    {
        return new CardData("Militia", Allegiance.Player, 3, 4, new List<CardAbility> { new CardAbility(_basicAttackName, 1) });
    }

    public static CardData FastGoblin()
    {
        return new CardData("FastGoblin", Allegiance.Enemy, 3, 1, new List<CardAbility> { new CardAbility(_basicAttackName, 1) });
    }
    public static CardData ToughGoblin()
    {
        return new CardData("ToughGoblin", Allegiance.Enemy, 1, 3, new List<CardAbility> { new CardAbility(_basicAttackName, 1) });
    }
    public static CardData GoblinAssassin()
    {
        return new CardData("GoblinAssassin", Allegiance.Enemy, 1, 2, new List<CardAbility> { new CardAbility(_basicAttackName, 2) });
    }

    // Tutorial 1 
    // "Left click to select, right click to move"
    // Hiro vs the Rat
    // Teaches: - Card Placememnt, Damage, and begins to teach Initiative Order and Wounding/VictoryCondition
    static int Tutorial1Hp = 2;
    static int Tutorial1Dmg = 1;
    public static CardData Hiro()
    {
        return new CardData("Hiro", Allegiance.Player, 2, Tutorial1Hp, new List<CardAbility> { new CardAbility("Dagger", Tutorial1Dmg) });
    }
    public static CardData Rat()
    {
        return new CardData("Rat", Allegiance.Enemy, 1, Tutorial1Hp, new List<CardAbility> { new CardAbility("Bite", Tutorial1Dmg) });
    }
    public static Player PlayerTutorial1()
    {
        List<CardData> cards = new List<CardData> {Hiro()};
        return new Player(new Deck(cards), 1, 1, 0);
    }
    public static Player EnemyTutorial1()
    {
        List<CardData> cards = new List<CardData> { Rat() };
        return new Player(new Deck(cards), 1, 1, 0);
    }


    // Tutorial 2
    // "Turn order depends on card speed [>]"
    // Mikes vs Goblins
    // Teaches: Initiative Order, Wounding/Victory Condition, Rank
    public static CardData JustMike()
    {
        return new CardData("Just Mike", Allegiance.Player, 2, 3, new List<CardAbility> { new CardAbility("Dagger", 1) });
    }
    public static CardData FastMike()
    {
        return new CardData("Fast Mike", Allegiance.Player, 3, 3, new List<CardAbility> { new CardAbility("Dagger", 1) });
    }
    public static CardData SlowMike()
    {
        return new CardData("Slow Mike", Allegiance.Player, 1, 3, new List<CardAbility> { new CardAbility("Dagger", 1) });
    }
    public static Player PlayerTutorial2()
    {
        List<CardData> cards = new List<CardData> { JustMike(), FastMike(), SlowMike() };
        return new Player(new Deck(cards), 3, 3, 0);
    }
    public static Player EnemyTutorial2()
    {
        List<CardData> cards = new List<CardData> { Goblin(), Goblin(), Goblin(), Goblin(), Goblin(), Goblin() };
        return new Player(new Deck(cards), 3, 3, 0);
    }

    public static CardData Goblin()
    {
        return new CardData("Goblin", Allegiance.Enemy, 2, 2, new List<CardAbility> { new CardAbility(_basicAttackName, 1) });
    }
    // Tutorial 3
    // Barbarian vs Zombie
    // "Place Dice in ability slots to Trigger them"
    public static CardData Barbarian()
    {
        return new CardData("Barbarian", Allegiance.Player, 2, 10, new List<CardAbility> { new CardAbility("Wild Swing", 3, new List<int> { 4 }) });
    }
    public static CardData Zombie()
    {
        return new CardData("Zombie", Allegiance.Enemy, 1, 4, new List<CardAbility> { new CardAbility("Lurch", 1) });
    }
    public static Player PlayerTutorial3()
    {
        List<CardData> cards = new List<CardData> { Barbarian() };
        return new Player(new Deck(cards), 1, 1, 1);
    }
    public static Player EnemyTutorial3()
    {
        List<CardData> cards = new List<CardData> { Zombie() };
        return new Player(new Deck(cards), 1, 1, 0);

    }
    

}
