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

    // Scene Objects
    public CardTray _enemyTray;
    public CardTray _playerTray;
    public CardTray _handTray;
    public DeckInfo _playerDeckInfo;
    public DeckInfo _enemyDeckInfo;

    public Player _player;
    public Player _enemy;

    GamePhase _phase;

    // Start is called before the first frame update
    void Start()
    {
        _phase = GamePhase.Unstarted;
        BeginGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
            ProgressTurn();
    }


    public void BeginGame()
    {
        _phase = GamePhase.Sanctuary;
        Debug.Log("Phase is " + _phase.ToString());
        _player = new Player(Presets.StartingDeck(), 2, 1);
    }

    public void BeginMatch()
    {
        _phase = GamePhase.Begin;
        Debug.Log("Phase is " + _phase.ToString());
        _enemy = new Player(Presets.GoblinDeck(), 2, 2);
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
    }

    void DestroyCardInSlot(CardSlot s)
    {
        if (s._card == null)
            return;

        Destroy(s._card.gameObject);
        s._card = null;
    }
    

    void DisplayCards()
    {
        Debug.Log("Displaying Cards");
        List<CardTray> trays = new List<CardTray> { _playerTray, _enemyTray, _handTray };
        foreach (CardTray tray in trays)
            foreach (CardSlot slot in tray._slots)
                DestroyCardInSlot(slot);

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
            BeginGame();
        else if (_phase == GamePhase.Sanctuary)
            BeginMatch();
        else if (_phase == GamePhase.Begin)
            DrawPhase();
        else if (_phase == GamePhase.Draw)
            CombatPhase();
        else if (_phase == GamePhase.Combat)
            CleanupPhase();
        else if (_phase == GamePhase.Cleanup)
            DrawPhase();
    }

    void CombatPhase()
    {
        _phase = GamePhase.Combat;
        Debug.Log("Phase is " + _phase.ToString());
        List<CardObject> cards = GetInitiativeOrderedCards();
        foreach (CardObject card in cards)
        {
            card._data.TakeTurn(card);
            card.UpdateCard();
        }

        
    }
    void CleanupPhase()
    {
        _phase = GamePhase.Cleanup;
        Debug.Log("Phase is " + _phase.ToString());

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
    }

    



    

    List<CardObject> GetInitiativeOrderedCards()
    {
        List<CardObject> playerCards = _playerTray.GetComponentsInChildren<CardObject>().ToList();
        List<CardObject> enemyCards = _enemyTray.GetComponentsInChildren<CardObject>().ToList();

        List<CardObject> allCards = new List<CardObject>();
        allCards.AddRange(playerCards);
        allCards.AddRange(enemyCards);
        allCards = allCards.OrderBy(x => x._data.GetInitiative()).ToList();
        return allCards;
    }

}
