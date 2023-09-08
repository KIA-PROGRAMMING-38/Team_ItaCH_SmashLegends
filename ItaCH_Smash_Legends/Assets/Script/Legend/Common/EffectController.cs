using Cysharp.Threading.Tasks;
using UnityEditor.Rendering.Universal.ShaderGraph;
using UnityEngine;

public enum MaterialType
{
    Hit,
    Invincible
}

public abstract class EffectController : MonoBehaviour
{
    [SerializeField] private GameObject[] _effectPrefabs;
    [SerializeField] private ParticleSystem _dieSmokeEffect;
    [SerializeField] private ParticleSystem _dieEffect;
    protected GameObject[] _effects;
    private Rigidbody _rigidbody;
    private float _scaleOffset;

    public readonly int FLASH_COUNT = 5;
    public readonly int HANG_JUMP_FLASH_COUNT = 3;

    [SerializeField] private Renderer[] _renderer;

    private Material[] _defaultMaterial;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _scaleOffset = 1 / transform.localScale.x;
        // 이펙트를 모아서 관리하기 위해 중간 단계의 오브젝트 생성
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
    public void DisableDieSmokeEffect()
    {
        if (_dieSmokeEffect.gameObject.activeSelf)
        {
            _dieSmokeEffect.gameObject.SetActive(false);
        }
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
    public void SetDieEffect()
    {
        _dieEffect.gameObject.SetActive(true);
        _dieEffect.transform.position = transform.position;
        _dieEffect.transform.forward = _rigidbody.velocity;
        _dieEffect.Play();
    }

    private void InitMaterial()
    {
        _defaultMaterial = new Material[_renderer.Length];

        for (int i = 0; i < _renderer.Length; ++i)
        {
            Material material = Instantiate(_renderer[i].material);
            _renderer[i].material = material;
            _defaultMaterial[i] = _renderer[i].material;
        }
    }

    private void OnFlashEffect(MaterialType materialType)
    {
        for (int i = 0; i < _renderer.Length; ++i)
        {
            _renderer[i].material = Managers.ResourceManager.GetMaterial(materialType);
            _renderer[i].material.mainTexture = _defaultMaterial[i].mainTexture;
        }
    }
    private void OffFlashEffect()
    {
        for (int i = 0; i < _renderer.Length; ++i)
        {
            _renderer[i].material = _defaultMaterial[i];
        }
    }

    public async UniTaskVoid StartHitFlashEffet()
    {
        int count = 3;
        while (count > 0)
        {
            OnFlashEffect(MaterialType.Hit);
            await UniTask.Delay(80);
            OffFlashEffect();
            await UniTask.Delay(80);
            --count;
        }
    }
    public async UniTaskVoid StartInvincibleFlashEffet(int count)
    {
        while (count > 0)
        {
            OnFlashEffect(MaterialType.Invincible);
            await UniTask.Delay(50);
            OffFlashEffect();
            await UniTask.Delay(50);
            --count;
        }
    }
}
