using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility 
{
    public string _name;
    public int _attack;
    public List<int> _conditions;
    public int[] _requirements;

    public CardAbility(string name, int attack, List<int> conditions = null)
    {
        _name = name;
        _attack = attack;

        _conditions = conditions == null ? new List<int>() : conditions;

        _requirements = new int[_conditions.Count];
    }

    public CardAbility Copy()
    {
        return new CardAbility(_name, _attack, _conditions);
    }
    public int GetAttack()
    {
        return _attack;
    }
    CardObject GetTarget(CardObject parent)
    {
        return parent._data._allegiance == Allegiance.Player ? parent._gameManager._battle._enemyTray.GetLastLivingCard() : parent._gameManager._battle._playerTray.GetLastLivingCard();
    }
    public void UseAbility(CardObject parent)
    {
        for (int i = 0; i < _conditions.Count; i++)
            if (_requirements[i] < _conditions[i])
                return;

        CardObject target = GetTarget(parent);

        if (target == null)
            return;

        UseAbilityOnTarget(parent, target);
    }

    void UseAbilityOnTarget(CardObject parent, CardObject target)
    {
        target._data.TakeDamage(GetAttack());
        target._gameManager._cardAnimator.AddAttackAnimation(target, parent._abilities[this], Color.red);
        target.UpdateCard();
    }
}
