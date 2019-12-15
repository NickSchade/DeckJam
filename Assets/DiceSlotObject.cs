using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceSlotObject : MonoBehaviour
{
    public TMP_Text _text;

    public int _condition;
    public DiceObject _dice;


    public void UpdateUi(int condition)
    {
        _condition = condition;
        _text.text = _condition.ToString();
    }
}
