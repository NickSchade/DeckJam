using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle : MonoBehaviour
{
    public CardTray _enemyTray;
    public CardTray _playerTray;
    public CardTray _handTray;
    public DeckInfo _playerDeckInfo;
    public DeckInfo _enemyDeckInfo;
    public DiceTray _diceTray;
    public GameManager _gameManager;

    public Player _player;
    public Player _enemy;
    GamePhase _phase;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
            ProgressTurn();
    }

    public void BeginMatch(Player player, Player enemy)
    {
        Debug.Log("Begin Match");
        _player = player;
        _enemy = enemy;
        _phase = GamePhase.Begin;

        gameObject.SetActive(true);

        _enemyTray.SetSlots(_enemy._maxPlay);
        _playerTray.SetSlots(_player._maxPlay);
        _handTray.SetSlots(_player._maxHand);

        _playerDeckInfo.SetPlayer(_player);
        _player._deck.GameStart();

        _enemyDeckInfo.SetPlayer(_enemy);
        _enemy._deck.GameStart();

        DrawPhase();
    }
    void ClearCards()
    {
        List<CardTray> trays = new List<CardTray> { _playerTray, _enemyTray, _handTray };
        foreach (CardTray tray in trays)
            foreach (CardSlot slot in tray._slots)
                slot.DestroyCardInSlot();
    }
    void ClearCardSlots()
    {
        List<CardTray> trays = new List<CardTray> { _playerTray, _enemyTray, _handTray };
        foreach (CardTray tray in trays)
            tray.DestroySlotsInTray();
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
        _enemyDeckInfo.UpdateText(); ;
    }

    public void ProgressTurn()
    {
        try
        {
            Debug.Log("Progress Turn " + _phase);
            if (_phase == GamePhase.Begin || _phase == GamePhase.CheckEndGame)
                DrawPhase();
            else if (_phase == GamePhase.Draw)
                AssignDicePhase();
            else if (_phase == GamePhase.AssignDice)
                CombatPhase();
            else if (_phase == GamePhase.Combat)
                CleanupPhase();
            else if (_phase == GamePhase.Cleanup)
                CheckEndGamePhase();
        }
        catch
        {
            Debug.Log("ProgressTurn Failed");
        }
    }
    void DrawPhase()
    {
        _phase = GamePhase.Draw;

        _player.DrawPhase();
        _enemy.DrawPhase();

        foreach (CardData card in _enemy._deck._hand.ToList())
            _enemy._deck.PlayCardFromHand(card);

        DisplayCards();
    }
    void AssignDicePhase()
    {
        if (_player._diceNumber == 0)
        {
            CombatPhase();
        }
        else
        {
            _phase = GamePhase.AssignDice;
            Debug.Log("starting " + _phase + " phase");
            List<CardTray> trays = new List<CardTray> { _playerTray, _handTray, _enemyTray };
            foreach (CardTray tray in trays)
                foreach (CardSlot slot in tray._slots)
                    if (slot._card != null)
                        slot._card._locked = true;

            for (int i = 0; i < _player._diceNumber; i++)
            {
                DiceSlotObject slot = Instantiate(_gameManager._prefabDiceSlot, _diceTray.transform);
                DiceObject dice = Instantiate(_gameManager._prefabDice, slot.transform);
                dice._locked = false;
                slot._dice = dice;
                dice._slot = slot;
                int r = Random.Range(1, 6);
                dice.UpdateValue(r);
            }
            Debug.Log("ending " + _phase + " phase");
        }
    }
    void CombatPhase()
    {
        _phase = GamePhase.Combat;
        Debug.Log("starting " + _phase + " phase");
        List<CardObject> cards = GetInitiativeOrderedCards();
        foreach (CardObject card in cards)
            card.LockRequirements();

        foreach (CardObject card in cards)
        {
            card._data.TakeTurn(card);
            card.UpdateCard();
        }


        Debug.Log("Finished Taking Card Turns");

        List<DiceSlotObject> slots = _diceTray.GetComponentsInChildren<DiceSlotObject>().ToList();
        for (int i = 0; i < slots.Count; i++)
            Destroy(slots[i].gameObject);

        Debug.Log("ending " + _phase + " phase");
    }
    void CleanupPhase()
    {
        _phase = GamePhase.Cleanup;
        Debug.Log("starting " + _phase + " phase");

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
        Debug.Log("ending " + _phase + " phase");

        CheckEndGamePhase();

    }
    public void CheckEndGamePhase()
    {
        _phase = GamePhase.CheckEndGame;

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
        else
        {
        }
    }

    public void EndGame()
    {
        ClearCardSlots();
        gameObject.SetActive(false);
        _gameManager._StartScreen.gameObject.SetActive(true);
        _phase = GamePhase.Sanctuary;
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
