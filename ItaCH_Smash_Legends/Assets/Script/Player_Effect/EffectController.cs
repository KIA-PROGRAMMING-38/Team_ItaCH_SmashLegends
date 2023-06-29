using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class EffectController : MonoBehaviour
{
    [SerializeField] private GameObject[] _effectPrefabs;
    [SerializeField] private ParticleSystem _dieSmokeEffect;
    [SerializeField] private ParticleSystem _dieEffect;
    protected GameObject[] _effects;
    private CharacterStatus _characterStatus;
    private Rigidbody _rigidbody;
    private PlayerHit _playerHit;
    private float _scaleOffset;

    private Color _hitColor = new Color(0.302f, 0.192f, 0.075f);
    private Color _invincibleColor = new Color(0.425f, 0.425f, 0.425f);

    [SerializeField] private Renderer[] _renderer;

    private void Awake()
    {
        _playerHit = GetComponent<PlayerHit>();
        _rigidbody = GetComponent<Rigidbody>();
        _characterStatus = GetComponent<CharacterStatus>();
        SetEventSubscription();
    }
    private void Start()
    {
        _scaleOffset = 1 / transform.localScale.x;
        // ����Ʈ�� ��Ƽ� �����ϱ� ���� �߰� �ܰ��� ������Ʈ ����
        GameObject EffectController = Instantiate(new GameObject(), transform);
        EffectController.name = "Effect Controller";
        _effects = new GameObject[_effectPrefabs.Length];
        for (int i = 0; i < _effectPrefabs.Length; ++i)
        {
            GameObject effect = Instantiate(_effectPrefabs[i], EffectController.transform);
            _effects[i] = effect;
            _effects[i].transform.localScale =
                new Vector3(_effects[i].transform.localScale.x * _scaleOffset,
                _effects[i].transform.localScale.y * _scaleOffset,
                _effects[i].transform.localScale.z * _scaleOffset);
            _effects[i].transform.position =
                new Vector3(transform.position.x + (_effects[i].transform.position.x * _scaleOffset),
                transform.position.y + (_effects[i].transform.position.y * _scaleOffset),
                transform.position.z + (_effects[i].transform.position.z * _scaleOffset));
            _effects[i].SetActive(false);
        }
        InitMaterial();
        CreateDieSmokeEffect();
        CreateDieEffect();
    }

    public void SetDieSmokeEffect()
    {
        _dieSmokeEffect.gameObject.SetActive(true);
        _dieSmokeEffect.transform.position = transform.position;
        _dieSmokeEffect.Play();
    }
    private void CreateDieSmokeEffect()
    {
        _dieSmokeEffect = Instantiate(_dieSmokeEffect, transform);
        _dieSmokeEffect.transform.localScale *= _scaleOffset;
        _dieSmokeEffect.gameObject.SetActive(false);
    }
    private void CreateDieEffect()
    {
        _dieEffect = Instantiate(_dieEffect, transform.position, Quaternion.identity);
        _dieEffect.gameObject.SetActive(false);
    }
    private void SetDieEffect()
    {
        _dieEffect.gameObject.SetActive(true);
        _dieEffect.transform.position = transform.position;
        _dieEffect.transform.forward = _rigidbody.velocity;
        _dieEffect.Play();
    }
    private void SetEventSubscription()
    {
        _characterStatus.OnPlayerDieEffect -= SetDieEffect;
        _characterStatus.OnPlayerDieEffect += SetDieEffect;
        _characterStatus.OnPlayerDieSmokeEffect -= SetDieSmokeEffect;
        _characterStatus.OnPlayerDieSmokeEffect += SetDieSmokeEffect;
    }
    private void InitMaterial()
    {
        for (int i = 0; i < _renderer.Length; ++i)
        {
            Material material = Instantiate(_renderer[i].material);
            _renderer[i].material = material;
        }
    }
    private void SetHitEffectColor()
    {
        for (int i = 0; i < _renderer.Length; ++i)
        {
            _renderer[i].material.SetColor("_EmissionColor", _hitColor);
        }
    }
    private void SetInvincibleEffectColor()
    {
        for (int i = 0; i < _renderer.Length; ++i)
        {
            _renderer[i].material.SetColor("_EmissionColor", _invincibleColor);
        }
    }
    private void OnFlashEffect()
    {
        for (int i = 0; i < _renderer.Length; ++i)
        {
            _renderer[i].material.EnableKeyword("_EMISSION");
        }
    }
    private void OffFlashEffect()
    {
        for (int i = 0; i < _renderer.Length; ++i)
        {
            _renderer[i].material.DisableKeyword("_EMISSION");
        }
    }

    public async UniTaskVoid StartHitFlashEffet()
    {
        int count = 3;
        SetHitEffectColor();
        while (count > 0)
        {
            OnFlashEffect();
            await UniTask.Delay(80);
            OffFlashEffect();
            await UniTask.Delay(80);
            --count;
        }
    }
    public async UniTaskVoid StartInvincibleFlashEffet(int count)
    {
        SetInvincibleEffectColor();
        _playerHit.invincible = true;
        while (count > 0)
        {
            OnFlashEffect();
            await UniTask.Delay(50);
            OffFlashEffect();
            await UniTask.Delay(50);
            --count;
        }
        _playerHit.invincible = false;
    }
}
