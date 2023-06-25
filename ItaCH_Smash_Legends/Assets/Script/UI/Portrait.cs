using Cysharp.Threading.Tasks;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    private Image _portrait;
    private GameObject _respawnTimer;
    private RotatingImage _rotateCircle;
    private TextMeshProUGUI _timeLeft;

    public void InitPortraitSetting(Sprite sprite)
    {
        _portrait = GetComponent<Image>();
        _respawnTimer = transform.GetChild(0).gameObject;
        _rotateCircle = _respawnTimer.transform.GetChild(0).GetComponent<RotatingImage>();
        _timeLeft = _respawnTimer.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        _portrait.sprite = sprite;
        _respawnTimer.SetActive(false);
    }

    public async void StartRespawnTimer(CharacterStatus characterStatus)
    {
        float respawnTime = characterStatus.RespawnTime;
        _respawnTimer.SetActive(true);
        _portrait.color = Color.gray;
        _rotateCircle.StartRotation();
        await ChangeTextValue(respawnTime);
        _respawnTimer.SetActive(false);
        _portrait.color = Color.white;
    }

    public async UniTask ChangeTextValue(float targetTime)
    {
        _timeLeft.text = targetTime.ToString();
        StringBuilder stringBuilder = new StringBuilder();
        float elapsedTime = 0;
        while (elapsedTime < targetTime)
        {
            stringBuilder.Clear();
            stringBuilder.Append((int)(targetTime - elapsedTime));
            _timeLeft.text = stringBuilder.ToString();
            await UniTask.Delay(1000);
            elapsedTime += 1;
        }
    }
}
