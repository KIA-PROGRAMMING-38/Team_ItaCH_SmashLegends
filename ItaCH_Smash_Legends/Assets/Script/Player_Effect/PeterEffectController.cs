using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeterEffectController : MonoBehaviour
{
    [SerializeField] private GameObject[] _effectPrefabs;
    private GameObject[] _effects;
    private float _scaleOffset;
    private void Start()
    {
        _scaleOffset = 1 / transform.localScale.x;
        // 이펙트를 모아서 관리하기 위해 중간 단계의 오브젝트 생성
        GameObject EffectController = Instantiate(new GameObject(), transform);
        EffectController.name = "Effect Controller";
        _effects = new GameObject[_effectPrefabs.Length];
        for(int i = 0; i < _effectPrefabs.Length; ++i)
        {
            GameObject effect = Instantiate(_effectPrefabs[i], EffectController.transform);
            _effects[i] = effect;
            _effects[i].transform.localScale = 
                new Vector3(_effects[i].transform.localScale.x * _scaleOffset, 
                _effects[i].transform.localScale.y * _scaleOffset, 
                _effects[i].transform.localScale.z * _scaleOffset);
            _effects[i].transform.position =
                new Vector3(_effects[i].transform.position.x * _scaleOffset,
                _effects[i].transform.position.y * _scaleOffset,
                _effects[i].transform.position.z * _scaleOffset);
            _effects[i].SetActive(false);
        }
    }
    public void EnableFirstAttackEffect() => _effects[0].SetActive(true);
    public void EnableSecondAttackFirstEffect() => _effects[1].SetActive(true);
    public void EnableSecondAttackSecondEffect() => _effects[2].SetActive(true);
    public void EnableFinishAttackEffect() => _effects[3].SetActive(true);
    public void EnableSmashAttackEffect() => _effects[4].SetActive(true);
}
