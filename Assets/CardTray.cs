using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTray : MonoBehaviour
{
    public GameManager _gameManager;
    public GameObject _tray;
    
    public List<CardSlot> _slots;
    // Start is called before the first frame update
    void Start()
    {
    }
    public void SetSlots(int numberOfSlots)
    {
        if (_slots != null)
            foreach (CardSlot slot in _slots)
                Destroy(slot);

        _slots = new List<CardSlot>();
        for (int i = 0; i < numberOfSlots; i++)
            _slots.Add(MakeSlot((i+1).ToString()));
    }

    public CardObject GetLastLivingCard()
    {
        for (int i = _slots.Count - 1; i >=0; i--)
        {
            if (_slots[i]._card != null)
            {
                if (_slots[i]._card._data._currentHp > 0)
                    return _slots[i]._card;
            }
        }
        return null;
    }

    CardSlot MakeSlot(string name)
    {
        CardSlot s = Instantiate(_gameManager._prefabCardSlot, _tray.transform);
        s.SetTray(this,name);
        return s;
    }

    CardSlot GetFirstFreeCardSlot()
    {
        foreach (CardSlot slot in _slots)
            if (slot._card == null)
                return slot;

        return null;
    }

    public void DispayCardInTray(CardData c)
    {
        CardSlot targetSlot = GetFirstFreeCardSlot();
        CardObject cardObject = Instantiate(_gameManager._prefabCard, targetSlot.transform);
        targetSlot._card = cardObject;
        cardObject.SetCard(_gameManager, c, targetSlot);
    }
}
