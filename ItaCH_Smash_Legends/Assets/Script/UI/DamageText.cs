using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

public class DamageText : MonoBehaviour
{
    private IObjectPool<GameObject> _pool;
    private TextMeshPro _text;
    private int _damagePresented = 0;
    public void InitDamageTextSettings(DamageTextPool damageTextPool)
    {
        _pool = damageTextPool.Pool;
    }
    public void MoveDamageText()
    {
        PopDamageText().Forget();
    }

    private async UniTask PopDamageText()
    {
        int currentFrame = 0;
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        Vector3 startPosition = transform.position;

        while (currentFrame <= 15 && transform != null)
        {
            float t = Mathf.InverseLerp(0, 15, currentFrame);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            currentFrame++;
            await UniTask.DelayFrame(1);
        }

        await UniTask.DelayFrame(45);
        currentFrame = 0;
        while (currentFrame <= 30 && transform != null)
        {
            float t = Mathf.InverseLerp(0, 30, currentFrame);
            transform.position = Vector3.Lerp(targetPosition, startPosition, t);
            currentFrame++;
            await UniTask.DelayFrame(1);
        }
        _pool.Release(this.gameObject);
    }

    public void ChangeText(int damage)
    {
        if (_damagePresented != damage)
        {
            _text.text = damage.ToString();
            _damagePresented = damage;
        }
    }
}