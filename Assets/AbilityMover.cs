using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class AbilityMover : MonoBehaviour
{
    public GameManager _gameManager;

    private AbilityObject _ability = null;

    void SelectAbility(AbilityObject c)
    {
        if (c._locked) return;

        if (_ability != null)
            DeselectAbility(_ability);

        _ability = c;
        c.HighlightOn();
    }
    void DeselectAbility(AbilityObject c)
    {
        _ability = null;
        c.HighlightOff();
    }

    void Update()
    {
        ProcessDiscreteClicks();
    }

    void StartCard(AbilityObject c)
    {
        if (c != null)
        {
            if (_ability != c)
                SelectAbility(c);
            else
                DeselectAbility(c);
        }
    }

    void EndCard(AbilityObject c, AbilitySlot s)
    {
        if (_ability != null &&
                s != null)
        {
            if (c == _ability)
                DeselectAbility(c);
            else if (c == null)
                PlaceCardInEmptySlot(s);
            else
                SwapCards(c);
        }
    }

    void ProcessDiscreteClicks()
    {
        if (Input.GetMouseButtonUp(0))
        {
            AbilityObject c = GetCardUnderMouse();
            StartCard(c);
        }
        if (Input.GetMouseButtonUp(1))
        {
            AbilityObject c = GetCardUnderMouse();
            AbilitySlot s = GetSlotUnderMouse();
            EndCard(c, s);
        }
    }

    void PlaceCardInEmptySlot(AbilitySlot s)
    {
        if (_ability._locked) return;

        AbilitySlot oldSlot = _ability._slot;
        
        PlaceCardInSlot(_ability, s);
        DeselectAbility(_ability);
        oldSlot._ability = null;
    }


    void PlaceCardInSlot(AbilityObject card, AbilitySlot slot)
    {
        card._slot = slot;
        slot._ability = card;
        card.transform.SetParent(slot.transform);
        card.transform.localPosition = Vector3.zero;
    }

    void SwapCards(AbilityObject cardToSwap)
    {
        if (cardToSwap._locked || _ability._locked) return;

        AbilitySlot swapToSlot = cardToSwap._slot;
        AbilitySlot currentSlot = _ability._slot;
        PlaceCardInSlot(cardToSwap, currentSlot);
        PlaceCardInSlot(_ability, swapToSlot);
        DeselectAbility(_ability);
    }


    AbilityObject GetCardUnderMouse()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        EventSystem.current.RaycastAll(ped, hits);
        foreach (RaycastResult h in hits)
        {
            AbilityObject c = h.gameObject.GetComponentInParent<AbilityObject>();
            if (c != null)
                return c;
        }
        return null;
    }

    AbilitySlot GetSlotUnderMouse()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        EventSystem.current.RaycastAll(ped, hits);
        foreach (RaycastResult h in hits)
        {
            AbilitySlot c = h.gameObject.GetComponentInParent<AbilitySlot>();
            if (c != null)
                return c;
        }
        return null;
    }
}
