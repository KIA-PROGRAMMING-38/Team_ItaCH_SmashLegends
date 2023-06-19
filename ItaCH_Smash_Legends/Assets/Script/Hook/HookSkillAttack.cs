using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HookSkillAttack : MonoBehaviour
{
    private ParticleSystem _startEffect;
    public ParticleSystem endEffect;

    private void Awake()
    {
        _startEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        SetShowEffect(_startEffect);
    }
    private void OnDisable()
    {
        SetShowEffect(endEffect);
    }
    private void SetShowEffect(ParticleSystem effect)
    {
        effect.transform.position = transform.position;
        effect.gameObject.SetActive(true);
        effect.Play();
    }
}
