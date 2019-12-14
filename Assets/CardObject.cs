using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Required when using Event data.

public class CardObject : MonoBehaviour
{
    public TMP_Text _text;
    public RawImage _highlight;
    public RawImage _cardFront;

    public GameManager _gameManager;
    public CardData _data;
    public CardSlot _slot;

    public bool _locked = false;

    string _heartTxt = "<3";
    string _initiativeTxt = "<";
    public static string _attackTxt = "*";
    

    public void SetCard(GameManager gameManager, CardData data, CardSlot slot)
    {
        _gameManager = gameManager;
        _data = data;
        _slot = slot;
        UpdateCard();

        foreach (CardAbility ability in _data._baseAbilities)
        {
            AbilityObject a = Instantiate(_gameManager._prefabAbility, _cardFront.transform);
            a.SetAbility(this, ability);
            a.UpdateUi();
        }
    }
    public void UpdateCard()
    {
        string txt = $"{_data._name} <br> {GetHpString()} <br> {GetInitiativeString()}";// {GetAbilitiesString()}";
        Debug.Log(_data._name + "'s data is " + txt);
        _text.text = txt;
    }
    string GetHpString()
    {
        List<string> listString = new List<string>();
        for (int i = 0; i < _data._baseHp; i++)
        {
            if (i != 0 && i % 4 == 0)
                listString.Add("<br>");

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
            if (i != 0 && i % 4 == 0)
                listString.Add("<br>");

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
