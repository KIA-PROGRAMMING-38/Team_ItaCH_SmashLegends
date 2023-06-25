using System.Text;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI _timerText;
    StringBuilder stringBuilder;

    public void InitTimerSettings()
    {
        _timerText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        stringBuilder = new StringBuilder();
    }
    public void ChangeTime(int time)
    {
        stringBuilder.Clear();
        int minute = time / 60;
        int second = time % 60;
        stringBuilder.Append(minute);
        stringBuilder.Append(":");
        stringBuilder.Append($"{second: 00}");
        _timerText.SetText(stringBuilder);
    }
}
