using UnityEngine;
using UnityEngine.Pool;

public class DamageTextPool : MonoBehaviour
{
    private IObjectPool<GameObject> _pool;
    public IObjectPool<GameObject> Pool { get => _pool; }
    private GameObject _damageTextPrefab;
    //private CharacterStatus _characterStatus; // TO DO : floating UI 리팩토링
    private Transform _characterTransform;
    private int _damage;

    private float time;
    private void Start()
    {
        InitPoolSettings();

    }
    private void Update()
    {
        if (time > 1f)
        {
            _pool.Get();
            time = 0;
        }
        time += Time.deltaTime;
    }
    public void InitPoolSettings(/*CharacterStatus characterStatus*/)
    {
        //_characterStatus = characterStatus;
        _characterTransform = transform;
        _pool = new ObjectPool<GameObject>(CreateDamageText, OnGetText, OnReleaseText, OnDestroyText, defaultCapacity: 15);
    }

    public GameObject CreateDamageText()
    {
        if (_damageTextPrefab == null)
        {
            _damageTextPrefab = Resources.Load<GameObject>("UI/FloatingDamage");
        }
        GameObject damageTextObject = Instantiate(_damageTextPrefab);
        damageTextObject.SetActive(false);

        DamageText damageText = damageTextObject.GetComponent<DamageText>();
        damageText.InitDamageTextSettings(this);
        ////if (_characterStatus != null)
        //{
        //    //_characterStatus.OnPlayerGetDamage -= damageText.ChangeText;
        //    //_characterStatus.OnPlayerGetDamage += damageText.ChangeText; // To Do : 피격 시 플로팅 데미지
        //}
        return damageTextObject;
    }

    public void OnGetText(GameObject damageText)
    {
        damageText.transform.localPosition =
        _characterTransform.localPosition + (Vector3)Random.insideUnitCircle.normalized * 0.2f;
        damageText.GetComponent<DamageText>().MoveDamageText();
        damageText.SetActive(true);
    }

    public void OnReleaseText(GameObject damageText)
    {
        damageText.SetActive(false);
    }

    public void OnDestroyText(GameObject damageText)
    {
        Destroy(damageText);
    }
}