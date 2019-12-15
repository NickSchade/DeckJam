using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Deck 
{
    public List<CardData> _deck;
    public List<CardData> _library;
    public List<CardData> _discard;
    public List<CardData> _hand;
    public List<CardData> _play;
    public List<CardData> _wounded;

    public Deck(List<CardData> cards)
    {
        _deck = new List<CardData>();
        foreach (CardData card in cards)
        {
            _deck.Add(card.Copy());
        }
    }



    public void ResetDeck()
    {
        _library = new List<CardData>();
        _discard = new List<CardData>();
        _hand = new List<CardData>();
        _play = new List<CardData>();
        _wounded = new List<CardData>();
    }

    public void SetDeck(List<CardData> cardsInDeck)
    {
        _deck = cardsInDeck;
    }

    public void GameStart()
    {
        ResetDeck();
        foreach (CardData card in _deck)
        {
            CardData c = card.Copy();
            c.Reset();
            _library.Add(c);
        }
    }

    public void DrawCard()
    {
        if (_library.Count == 0) return;

        CardData c = _library[0];
        _library.RemoveAt(0);
        _hand.Add(c);
    }

    public void MoveCardFromStackToStack(CardData card, List<CardData> originalStack, List<CardData> targetStack)
    {
        originalStack.Remove(card);
        targetStack.Add(card);
    }

    public void DiscardCardFromPlay(CardData card)
    {
        MoveCardFromStackToStack(card, _play, _discard);
    }
    public void WoundCardFromPlay(CardData card)
    {
        MoveCardFromStackToStack(card, _play, _wounded);
    }

    public void DiscardCardFromHand(CardData card)
    {
        MoveCardFromStackToStack(card, _hand, _discard);
    }
    public void WoundCardFromHand(CardData card)
    {
        MoveCardFromStackToStack(card, _hand, _wounded);
    }



    public void PlayCardFromHand(CardData card)
    {
        MoveCardFromStackToStack(card, _hand, _play);
    }
    public void ReturnCardToHand(CardData card)
    {
        MoveCardFromStackToStack(card, _play, _hand);
    }

    public void ShuffleStack(List<CardData> stack)
    {
        stack = stack.OrderBy(x => Random.value).ToList();
    }
    public void ShuffleLibrary()
    {
        ShuffleStack(_library);
    }
    


    public void ShuffleDiscardIntoLibrary()
    {
        ShuffleStack(_discard);
        List<CardData> cards = new List<CardData>(_discard);
        foreach (CardData card in cards)
            MoveCardFromStackToStack(card, _discard, _library);
    }
}
