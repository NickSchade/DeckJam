using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Allegiance { Player, Enemy};

public class CardData
{
    public string _name;
    public Allegiance _allegiance;
    public int _baseHp;
    public int _currentHp;

    public int _baseInitiative;
   
    public List<CardAbility> _baseAbilities;
    public CardAbility[] _slottedAbilities;

    public CardData(string name, Allegiance allegiance, int initiative, int hp, List<CardAbility> abilities, CardAbility[] slottedAbilities)
    {
        _name = name;
        _allegiance = allegiance;
        _baseInitiative = initiative;
        _baseHp = hp;
        
        _baseAbilities = abilities;
        _slottedAbilities = slottedAbilities;
    }
    public void Reset()
    {
        _currentHp = _baseHp;
    }
    public int GetInitiative()
    {
        return _baseInitiative;
    }
    public void TakeDamage(int damage)
    {
        _currentHp -= damage;
    }
    public CardData Copy()
    {
        List<CardAbility> abilities = new List<CardAbility>();
        foreach (CardAbility ability in _baseAbilities)
            abilities.Add(ability.Copy());
        
        CardAbility[] slotted = new CardAbility[_slottedAbilities.Length];
        for (int i = 0; i < _slottedAbilities.Length; i++)
            slotted[i] = _slottedAbilities[i];

        return new CardData(_name, _allegiance, _baseInitiative, _baseHp, abilities, slotted);
    }
    
    public void TakeTurn(CardObject parent)
    {
        if (_currentHp <= 0)
           return;

        foreach (CardAbility ability in _baseAbilities)
            ability.UseAbility(parent);
    }

    public void EndTurnFromPlay(Deck parentDeck)
    {
        if (_currentHp > 0)
            parentDeck.DiscardCardFromPlay(this);
        else
            parentDeck.WoundCardFromPlay(this);
    }

    public void EndTurnFromHand(Deck parentDeck)
    {
        if (_currentHp > 0)
            parentDeck.DiscardCardFromHand(this);
        else
            parentDeck.WoundCardFromHand(this);
    }
    
}
