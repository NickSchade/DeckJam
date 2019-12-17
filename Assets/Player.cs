using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public int _maxHand;
    public int _maxPlay;
    public int _diceNumber;
    public Deck _deck;

    public Player(Deck deck, int maxHand, int maxPlay, int diceNumber)
    {
        _deck = deck;
        _maxHand = maxHand;
        _maxPlay = maxPlay;
        _diceNumber = diceNumber;
    }

    public bool CheckDefeat()
    {
        return _deck._library.Count == 0 && _deck._discard.Count == 0 && _deck._hand.Count == 0 && _deck._play.Count == 0;
    }

    public void DrawPhase()
    {
        int cardsToDraw = _maxHand - _deck._hand.Count;
        for (int i = 0; i < cardsToDraw; i++)
        {
            if (_deck._library.Count == 0)
                _deck.ShuffleDiscardIntoLibrary();

            if (CheckDefeat())
                return;

            _deck.DrawCard();
        }
    }
}
