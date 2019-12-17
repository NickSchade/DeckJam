using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    public GameManager _gameManager;
    public CardAnimationDamage _prefabDamage;

    List<CardAnimationDamage> _queue;
    CardAnimationDamage _currentAnimation = null;

    // Start is called before the first frame update
    void Start()
    {
        _queue = new List<CardAnimationDamage>();
    }

    public void AddAttackAnimation(CardObject card, AbilityObject ability, Color color)
    {
        CardAnimationDamage dmg = Instantiate(_prefabDamage, transform);
        dmg.SetAnimation(card, ability, color);
        _queue.Add(dmg);
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentAnimation == null && _queue.Count != 0)
        {
            CardAnimationDamage dmg = _queue[0];
            _queue.Remove(dmg);
            _currentAnimation = dmg;
            dmg.TakeDamageWrapper();
        }
            
    }
}
