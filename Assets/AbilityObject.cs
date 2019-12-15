using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityObject : MonoBehaviour
{
    public TMP_Text _text;
    public RawImage _requirementsObject;
    public CardAbility _ability;
    public CardObject _parent;
    public List<DiceSlotObject> _requirementSlots;

    public void SetAbility(CardObject parent, CardAbility ability)
    {
        _parent = parent;
        _ability = ability;
        _requirementSlots = new List<DiceSlotObject>();
        for (int i = 0; i < _ability._conditions.Count; i++)
        {
            DiceSlotObject diceSlot = Instantiate(_parent._gameManager._prefabDiceSlot, _requirementsObject.transform);
            diceSlot.UpdateUi(_ability._conditions[i]);
            _requirementSlots.Add(diceSlot);
        }
    }
    public void UpdateUi()
    {
        _text.text = GetAbilitiesString();
    }
    string GetAbilitiesString()
    {
        List<string> listString = new List<string>();
        string s = $@"{_ability._name} <br> {CardObject.GetStringWithColor(CardObject._attackTxt, Color.red)}{_ability.GetAttack()}";
        listString.Add(s);
        return string.Join("", listString);
    }
}
