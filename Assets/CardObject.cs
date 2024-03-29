﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Required when using Event data.
using System.Linq;

public class CardObject : MonoBehaviour
{
    public TMP_Text _name;
    public TMP_Text _hp;
    public TMP_Text _initiative;

    public RawImage _highlight;
    public RawImage _cardFront;

    public GameManager _gameManager;
    public CardData _data;
    public CardSlot _slot;
    public Dictionary<CardAbility, AbilityObject> _abilities = new Dictionary<CardAbility, AbilityObject>();

    public bool _locked = false;

    string _heartTxt = "<3";
    string _initiativeTxt = ">";
    public static string _attackTxt = "*";

    public void LockRequirements()
    {
        List<AbilityObject> abilities = GetComponentsInChildren<AbilityObject>().ToList();
        foreach (AbilityObject ability in abilities)
        {
            for (int i = 0; i < ability._requirementSlots.Count; i++)
            {
                int val = ability._requirementSlots[i]._dice == null ? 0 : ability._requirementSlots[i]._dice._value;
                ability._ability._requirements[i] = val;
            }
            List<DiceObject> diceObjects = ability.GetComponentsInChildren<DiceObject>().ToList();
            foreach (DiceObject dice in diceObjects)
                dice._locked = true;
        }

    }




    public void SetCard(GameManager gameManager, CardData data, CardSlot slot)
    {
        _gameManager = gameManager;
        _data = data;
        _slot = slot;
        UpdateCard();
        UpdateAbilityUi();
    }
    public void UpdateCard()
    {
        _name.text = _data._name;
        _hp.text = GetHpString();
        _initiative.text = GetInitiativeString();
    }
    public void UpdateAbilityUi()
    {
        foreach (CardAbility ability in _data._baseAbilities)
        {
            AbilityObject a = Instantiate(_gameManager._prefabAbility, _cardFront.transform);
            a.SetAbility(this, ability);
            a.UpdateUi();
            _abilities[ability] = a;
        }
        foreach (CardAbility ability in _data._slottedAbilities)
        {
            AbilitySlot slot = Instantiate(_gameManager._prefabAbilitySlot, _cardFront.transform);
            if (ability != null)
            {
                AbilityObject a = Instantiate(_gameManager._prefabAbility, slot.transform);
                a.SetAbility(this, ability);
                a.UpdateUi();
                _abilities[ability] = a;
            }
        }
    }
    string GetHpString()
    {
        List<string> listString = new List<string>();
        for (int i = 0; i < _data._baseHp; i++)
        {
            Color color = i < _data._currentHp ? Color.red : Color.black;
            string heart = GetStringWithColor(_heartTxt, color);
            listString.Add(heart);
        }
        return string.Join("", listString);
    }
    string GetInitiativeString()
    {
        List<string> listString = new List<string>();
        for (int i = 0; i < _data.GetInitiative(); i++)
        {
            Color color = Color.blue;
            string arrow = GetStringWithColor(_initiativeTxt, color);
            listString.Add(arrow);
        }
        return string.Join("", listString);
    }
    

    public static string GetStringWithColor(string s, Color c)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(c)}>{s}</color>";
    }

    public void HighlightOn()
    {
        _highlight.color = Color.black;
    }
    public void HighlightOff()
    {
        _highlight.color = Color.white;
    }
}
