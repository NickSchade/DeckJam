using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAnimationDamage : MonoBehaviour
{
    public CardObject _target;
    public AbilityObject _ability;
    public Color _color;

    public void SetAnimation(CardObject target, AbilityObject ability, Color color)
    {
        _target = target;
        _ability = ability;
        _color = color;
    }


    public void TakeDamageWrapper()
    {
        StartCoroutine(TakeDamageCaller());
    }
    IEnumerator TakeDamageCaller()
    {
        yield return TakeDamage();
    }
    Coroutine TakeDamage()
    {
        return StartCoroutine(TakeDamageCoroutine());
    }
    IEnumerator TakeDamageCoroutine()
    {
        yield return null;
        Color originalColor = _target._cardFront.color;
        Color originalAbilityColor = _ability._abilityObject.color;
        float speed = 0.15f;
        for (float percent = 0.0f; percent < 1.0f; percent += speed)
        {
            _target._cardFront.color = Color.Lerp(originalColor, Color.red, percent);
            _ability._abilityObject.color = Color.Lerp(originalAbilityColor, Color.black, percent);
            yield return null;
        }
        for (float percent = 0.0f; percent < 1.0f; percent += speed)
        {
            _target._cardFront.color = Color.Lerp(originalColor, Color.red, 1f - percent);
            _ability._abilityObject.color = Color.Lerp(originalAbilityColor, Color.black, 1f - percent);
            yield return null;
        }
        _target._cardFront.color = originalColor;
        _ability._abilityObject.color = originalAbilityColor;
        yield return null;
        Destroy(this);
        yield return null;
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
