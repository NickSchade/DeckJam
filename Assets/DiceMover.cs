using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

public class DiceMover : MonoBehaviour
{
    public GameManager _gameManager;

    DiceObject _dice = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        ProcessDiscreteClicks();
    }

    DiceObject GetDiceUnderMouse()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        EventSystem.current.RaycastAll(ped, hits);
        foreach (RaycastResult h in hits)
        {
            DiceObject c = h.gameObject.GetComponentInParent<DiceObject>();
            if (c != null)
                return c;
        }
        return null;
    }
    DiceSlotObject GetSlotUnderMouse()
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;
        EventSystem.current.RaycastAll(ped, hits);
        foreach (RaycastResult h in hits)
        {
            DiceSlotObject c = h.gameObject.GetComponentInParent<DiceSlotObject>();
            if (c != null)
                return c;
        }
        return null;
    }

    void SelectDice(DiceObject d)
    {
        if (d._locked) return;

        if (_dice != null)
            DeselectDice(_dice);

        _dice = d;
        d.HighlightOn();
    }
    void DeselectDice(DiceObject d)
    {
        _dice = null;
        d.HighlightOff();
    }

    void ProcessDiscreteClicks()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DiceObject c = GetDiceUnderMouse();
            if (c != null)
            {
                if (_dice != c)
                    SelectDice(c);
                else
                    DeselectDice(c);
            }

        }
        if (Input.GetMouseButtonUp(1))
        {
            DiceObject c = GetDiceUnderMouse();
            DiceSlotObject s = GetSlotUnderMouse();
            if (_dice != null &&
                s != null)
            {
                if (c == _dice)
                    DeselectDice(c);
                else if (c == null)
                    PlaceDiceInEmptySlot(s);
                else
                    SwapDice(c);
            }

        }
    }

    void PlaceDiceInEmptySlot(DiceSlotObject s)
    {
        if (_dice._locked) return;

        DiceSlotObject oldSlot = _dice._slot;

        PlaceDiceInSlot(_dice, s);
        DeselectDice(_dice);
        oldSlot._dice = null;

        _gameManager._battle.UpdateUi();
    }


    void PlaceDiceInSlot(DiceObject dice, DiceSlotObject slot)
    {
        dice._slot = slot;
        slot._dice = dice;
        dice.transform.SetParent(slot.transform);
        dice.transform.localPosition = Vector3.zero;
    }

    void SwapDice(DiceObject d)
    {
        if (d._locked || _dice._locked) return;

        DiceSlotObject swapToSlot = d._slot;
        DiceSlotObject currentSlot = _dice._slot;
        PlaceDiceInSlot(d, currentSlot);
        PlaceDiceInSlot(_dice, swapToSlot);
        DeselectDice(_dice);
    }
}
