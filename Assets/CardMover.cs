﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class CardMover : MonoBehaviour
{
    public GameManager _gameManager;

    private CardObject _card = null;
    
    void SelectCard(CardObject c)
    {
        if (c._locked) return;

        if (_card != null)
            DeselectCard(_card);

        _card = c;
        c.HighlightOn();
    }
    void DeselectCard(CardObject c)
    {
        _card = null;
        c.HighlightOff();
    }
    
    void Update()
    {
        ProcessDiscreteClicks();
    }

    void ProcessDiscreteClicks()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CardObject c = GetCardUnderMouse();
            if (c != null && c._slot._tray != _gameManager._enemyTray)
            {
                if (_card != c)
                    SelectCard(c);
                else
                    DeselectCard(c);
            }

        }
        if (Input.GetMouseButtonUp(1))
        {
            CardObject c = GetCardUnderMouse();
            CardSlot s = GetSlotUnderMouse();
            if (_card != null &&
                s != null && 
                s._tray != _gameManager._enemyTray)
            {
                if (c == _card)
                    DeselectCard(c);
                else if (c == null)
                    PlaceCardInEmptySlot(s);
                else
                    SwapCards(c);
            }
                
        }
                
    }

    void PlaceCardInEmptySlot(CardSlot s)
    {
        if (_card._locked) return;

        CardSlot oldSlot = _card._slot;
        
        if (oldSlot._tray != s._tray)
        {
            if (oldSlot._tray == _gameManager._handTray)
                _gameManager._player._deck.PlayCardFromHand(_card._data);
            else
                _gameManager._player._deck.ReturnCardToHand(_card._data);
        }

        PlaceCardInSlot(_card, s);
        DeselectCard(_card);
        oldSlot._card = null;

        _gameManager.UpdateUi();
    }
    

    void PlaceCardInSlot(CardObject card, CardSlot slot)
    {
        card._slot = slot;
        slot._card = card;
        card.transform.SetParent(slot.transform);
        card.transform.localPosition = Vector3.zero;
    }

    void SwapCards(CardObject cardToSwap)
    {
        if (cardToSwap._locked || _card._locked) return;

        CardSlot swapToSlot = cardToSwap._slot;
        CardSlot currentSlot = _card._slot;
        PlaceCardInSlot(cardToSwap, currentSlot);
        PlaceCardInSlot(_card, swapToSlot);
        DeselectCard(_card);
    }
    

    CardObject GetCardUnderMouse()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        EventSystem.current.RaycastAll(ped, hits);
        foreach (RaycastResult h in hits)
        {
            CardObject c = h.gameObject.GetComponentInParent<CardObject>();
            if (c != null)
                return c;
        }
        return null;
    }

    CardSlot GetSlotUnderMouse()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        EventSystem.current.RaycastAll(ped, hits);
        foreach (RaycastResult h in hits)
        {
            CardSlot c = h.gameObject.GetComponentInParent<CardSlot>();
            if (c != null)
                return c;
        }
        return null;
    }
}
