using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RespawnTimer : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _text;
    private StringBuilder stringBuilder= new StringBuilder();   
    private int _playerId;
    //지금은 로직상 무조건 나의 playerID가 1이라, 이를 반영하여 코드 작성,
    //추후 UserData의 실제 ID를 가져와 나인지, 내가 아닌지를 구분할 예정
    public void InitRespawnTimerSettings(CharacterStatus characterStatus)
    {
        gameObject.SetActive(false);
        _playerId = characterStatus.PlayerID;
    }

    public async void CheckPlayer(CharacterStatus characterStatus)
    {
        if(characterStatus.PlayerID.Equals(_playerId))
        {
            gameObject.SetActive(true);

            UniTask[] uniTasks = new UniTask[]{
            ChangeSliderValue(characterStatus.RespawnTime),
            ChangeTextValue(characterStatus.RespawnTime)
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
