using Cysharp.Threading.Tasks;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RespawnTimer : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    private StringBuilder stringBuilder = new StringBuilder();
    private int _localPlayerID;

    public void InitRespawnTimerSettings(CharacterStatus characterStatus)
    {
        gameObject.SetActive(false);
        _localPlayerID = Managers.GameRoomManager.UserLocalData.ID;
    }

    public async void CheckPlayer(int deadPlayer) // 내가 죽었는 지 확인하는 로직
    {
        if (deadPlayer.Equals(_localPlayerID))
        {
            gameObject.SetActive(true);

            UniTask[] uniTasks = new UniTask[]{
            //ChangeSliderValue(characterStatus.RespawnTime),
            //ChangeTextValue(characterStatus.RespawnTime) // TODO : 리스폰 수정 이후 반영
            };

            await UniTask.WhenAll(uniTasks);

            gameObject.SetActive(false);
        }
    }

    public async UniTask ChangeSliderValue(float targetTime)
    {
        _slider.value = 0;
        float elapsedTime = 0;
        float oneDividedByTargetTime = 1 / targetTime;
        while (_slider.value < 1)
        {
            elapsedTime += Time.deltaTime;
            _slider.value = elapsedTime * oneDividedByTargetTime;
            await UniTask.DelayFrame(1);
        }
    }
    public async UniTask ChangeTextValue(float targetTime)
    {
        float elapsedTime = 0;
        while (elapsedTime < targetTime)
        {
            ChangeText((int)(targetTime - elapsedTime));
            await UniTask.Delay(1000);
            elapsedTime += 1;
        }
    }

    private void ChangeText(int timeLeft)
    {
        stringBuilder.Clear();
        stringBuilder.Append($"부활 ({timeLeft})");
        _text.text = stringBuilder.ToString();
    }
}
