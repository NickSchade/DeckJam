using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceObject : MonoBehaviour
{
    public TMP_Text _text;
    public int _value;
    public DiceSlotObject _slot;
    public RawImage _highlight;
    public bool _locked = false;

    public void UpdateValue(int value)
    {
        _value = value;
        UpdateUi();
    }
    public void UpdateUi()
    {
        _text.text = _value.ToString();
    }

    public void HighlightOn()
    {
        _highlight.color = Color.white;
    }
    public void HighlightOff()
    {
        _highlight.color = Color.black;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
