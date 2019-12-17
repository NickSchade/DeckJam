using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameManager _gameManager;
    public CardSlot _slot;
    public GameObject _abilityPanel;

    public Player _player;
    int iCardIndex = 0;

    public List<CardAbility> _inventory;

    public void BeginManagement(Player player, List<CardAbility> inventory)
    {
        _player = player;
        _inventory = inventory;
        UpdateCardUi();
        SetInventoryUi();
    }

    void SetInventoryUi()
    {
        foreach (CardAbility ability in _inventory)
        {
            AbilitySlot slot = Instantiate(_gameManager._prefabAbilitySlot, _abilityPanel.transform);
            if (ability != null)
            {
                AbilityObject a = Instantiate(_gameManager._prefabAbility, slot.transform);
                a.SetAbility(_gameManager, ability);
                a.UpdateUi();
                a._locked = false;
            }
        }
    }

    void UpdateCardUi()
    {
        if (_slot._card != null)
            Destroy(_slot._card.gameObject);

        CardData card = _player._deck._deck[iCardIndex];
        card.Reset();
        CardObject co = Instantiate(_gameManager._prefabCard, _slot.transform);
        co._data = card;
        co.SetCard(_gameManager, card, _slot);
        _slot._card = co;
    }


    public void NextCard()
    {
        iCardIndex++;
        if (iCardIndex == _player._deck._deck.Count)
            iCardIndex = 0;

        UpdateCardUi();
    }
    public void PreviousCard()
    {
        iCardIndex--;
        if (iCardIndex == -1)
            iCardIndex = _player._deck._deck.Count - 1;

        UpdateCardUi();
    }
}
