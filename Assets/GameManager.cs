using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    // Prefabs
    public CardObject _prefabCard;
    public CardSlot _prefabCardSlot;
    public AbilitySlot _prefabAbilitySlot;
    public AbilityObject _prefabAbility;
    public DiceSlotObject _prefabDiceSlot;
    public DiceObject _prefabDice;

    // Scene Objects
    public Battle _battle;
    public GameObject _StartScreen;
    public TutorialMessage _TutorialScreen;
    public CardAnimator _cardAnimator;
    public DeckManager _deckManager;
    
    public void BeginTutorial1()
    {
        _TutorialScreen.SetMessage("Left Click to select & Right Click to place", GameScreen.Battle, Presets.PlayerTutorial1(), Presets.EnemyTutorial1());
    }
    public void BeginTutorial2()
    {
        _TutorialScreen.SetMessage("Card Turn Order depends on Initiative " + CardObject.GetStringWithColor(">", Color.blue), GameScreen.Battle, Presets.PlayerTutorial2(), Presets.EnemyTutorial2());
    }
    public void BeginTutorial3()
    {
        _TutorialScreen.SetMessage("Place Dice to activate card Abilities", GameScreen.Battle, Presets.PlayerTutorial3(), Presets.EnemyTutorial3());
    }
    public void BeginTutorial4()
    {
        _TutorialScreen.SetMessage("Slot abilities in cards before the Match!", GameScreen.DeckManage, Presets.PlayerTutorial4(), Presets.EnemyTutorial4(), Presets.InventoryTutorial4());
    }
}
