using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{

    // Prefabs
    public CardObject _prefabCard;
    public CardSlot _prefabCardSlot;
    public AbilityObject _prefabAbility;
    public DiceSlotObject _prefabDiceSlot;
    public DiceObject _prefabDice;
    

    // Scene Objects
    public CardTray _enemyTray;
    public CardTray _playerTray;
    public CardTray _handTray;
    public DeckInfo _playerDeckInfo;
    public DeckInfo _enemyDeckInfo;
    public DiceTray _diceTray;
    public GameObject _GameScreen;
    public GameObject _StartScreen;
    public TutorialMessage _TutorialScreen;


    public Player _player;
    public Player _enemy;

    GamePhase _phase;

    // Start is called before the first frame update
    void Start()
    {
        _phase = GamePhase.Unstarted;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
            ProgressTurn();
    }


    public void BeginProgram()
    {
        _phase = GamePhase.Sanctuary;
    }

    public void BeginTestMatch()
    {
        Player player = Presets.PlayerTestMatch();
        Player enemy = Presets.EnemyTestMatch();

        _StartScreen.gameObject.SetActive(false);

        BeginMatch(player, enemy);
    }

    public void BeginTutorial1()
    {
        _player = Presets.PlayerTutorial1();
        _enemy = Presets.EnemyTutorial1();

        _TutorialScreen.SetText("Left Click to select & Right Click to place");
    }
    public void BeginTutorial2()
    {
        _player = Presets.PlayerTutorial2();
        _enemy = Presets.EnemyTutorial2();

        _TutorialScreen.SetText("Card Turn Order depends on Initiative " + CardObject.GetStringWithColor(">",Color.blue));
    }
    public void BeginTutorial3()
    {
        _player = Presets.PlayerTutorial3();
        _enemy = Presets.EnemyTutorial3();

        _TutorialScreen.SetText("Place Dice to activate card Abilities");
    }
    public void BeginTutorial4()
    {
        Player player = Presets.PlayerTutorial1();
        Player enemy = Presets.EnemyTutorial1();

        BeginMatch(player, enemy);
    }

    public void BeginMatch(Player player, Player enemy)
    {
        Debug.Log("Begin Match");
        _player = player;
        _enemy = enemy;
        _phase = GamePhase.Begin;

        _GameScreen.gameObject.SetActive(true);

        _enemyTray.SetSlots(_enemy._maxPlay);
        _playerTray.SetSlots(_player._maxPlay);
        _handTray.SetSlots(_player._maxHand);

        _playerDeckInfo.SetPlayer(_player);
        _player._deck.GameStart();

        _enemyDeckInfo.SetPlayer(_enemy);
        _enemy._deck.GameStart();

        DrawPhase();
    }

    public void DrawPhase()
    {
        _phase = GamePhase.Draw;
        Debug.Log("Phase is " + _phase.ToString());

        _player.DrawPhase();
        _enemy.DrawPhase();

        foreach (CardData card in _enemy._deck._hand.ToList())
            _enemy._deck.PlayCardFromHand(card);

        DisplayCards();

        if (_player.CheckDefeat())
        {
            Debug.Log("You Lost!");
            EndGame();
        }
        else if (_enemy.CheckDefeat())
        {
            Debug.Log("You won!");
            EndGame();
        }
    }

    void DestroyCardInSlot(CardSlot s)
    {
        if (s._card == null)
            return;

        Destroy(s._card.gameObject);
        s._card = null;
    }
    
    void ClearCards()
    {
        List<CardTray> trays = new List<CardTray> { _playerTray, _enemyTray, _handTray };
        foreach (CardTray tray in trays)
            foreach (CardSlot slot in tray._slots)
                DestroyCardInSlot(slot);
    }

    void DisplayCards()
    {
        ClearCards();

        foreach (CardData c in _player._deck._hand)
            _handTray.DispayCardInTray(c);

        foreach (CardData c in _enemy._deck._play)
            _enemyTray.DispayCardInTray(c);

        UpdateUi();
    }

    public void UpdateUi()
    {
        _playerDeckInfo.UpdateText();
        _enemyDeckInfo.UpdateText();;
    }

    void ProgressTurn()
    {
        if (_phase == GamePhase.Unstarted)
            BeginProgram();
        else if (_phase == GamePhase.Sanctuary)
            BeginTestMatch();
        else if (_phase == GamePhase.Begin)
            DrawPhase();
        else if (_phase == GamePhase.Draw)
            AssignDicePhase();
        else if (_phase == GamePhase.AssignDice)
            CombatPhase();
        else if (_phase == GamePhase.Combat)
            CleanupPhase();
        else if (_phase == GamePhase.Cleanup)
            DrawPhase();
    }

    void AssignDicePhase()
    {
        if (_player._diceNumber == 0)
            CombatPhase();
        else
        {
            _phase = GamePhase.AssignDice;
            List<CardTray> trays = new List<CardTray> { _playerTray, _handTray, _enemyTray };
            foreach (CardTray tray in trays)
                foreach (CardSlot slot in tray._slots)
                    if (slot._card != null)
                        slot._card._locked = true;

            for (int i = 0; i < _player._diceNumber; i++)
            {
                DiceSlotObject slot = Instantiate(_prefabDiceSlot, _diceTray.transform);
                DiceObject dice = Instantiate(_prefabDice, slot.transform);
                dice._locked = false;
                slot._dice = dice;
                dice._slot = slot;
                int r = Random.Range(1, 6);
                dice.UpdateValue(r);
            }
        }
    }

    void CombatPhase()
    {
        _phase = GamePhase.Combat;
        Debug.Log("Phase is " + _phase.ToString());
        List<CardObject> cards = GetInitiativeOrderedCards();
        foreach (CardObject card in cards)
            card.LockRequirements();
        
        foreach (CardObject card in cards)
        {
            card._data.TakeTurn(card);
            card.UpdateCard();
        }

        List<DiceSlotObject> slots = _diceTray.GetComponentsInChildren<DiceSlotObject>().ToList();
        for (int i = 0; i < slots.Count; i++)
            Destroy(slots[i].gameObject);

        
    }
    void CleanupPhase()
    {
        _phase = GamePhase.Cleanup;

        List<CardTray> trays = new List<CardTray> { _playerTray, _handTray, _enemyTray };
        foreach (CardTray tray in trays)
            foreach (CardSlot slot in tray._slots)
                if (slot._card != null)
                    slot._card._locked = false;

        foreach (CardSlot s in _playerTray._slots)
            if (s._card != null)
                s._card._data.EndTurnFromPlay(_player._deck);

        foreach (CardSlot s in _enemyTray._slots)
            if (s._card != null)
                s._card._data.EndTurnFromPlay(_enemy._deck);

        foreach (CardSlot s in _handTray._slots)
            if (s._card != null)
                s._card._data.EndTurnFromHand(_player._deck);
        
        DisplayCards();
        DrawPhase();
        
    }
    public void EndGame()
    {
        ClearCards();
        _GameScreen.gameObject.SetActive(false);
        _StartScreen.gameObject.SetActive(true);
    }
    



    

    List<CardObject> GetInitiativeOrderedCards()
    {
        List<CardObject> playerCards = _playerTray.GetComponentsInChildren<CardObject>().ToList();
        List<CardObject> enemyCards = _enemyTray.GetComponentsInChildren<CardObject>().ToList();

        List<CardObject> allCards = new List<CardObject>();
        allCards.AddRange(playerCards);
        allCards.AddRange(enemyCards);
        allCards = allCards.OrderByDescending(x => x._data.GetInitiative()).ToList();
        return allCards;
    }

}
