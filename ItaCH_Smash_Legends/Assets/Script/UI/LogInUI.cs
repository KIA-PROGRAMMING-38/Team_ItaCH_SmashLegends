using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util.Method;

public class LogInUI : MonoBehaviour
{
    private string _userInput;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private GameObject _logInField;
    [SerializeField] private GameObject _loadingObject;
    [SerializeField] private TextMeshProUGUI _errorMessage;

    private Image _background;
    private float _fadeInTime = 1f;
    private Vector3 _targetBigSize = new Vector3(1.5f, 1.5f, 1.5f);

    private Image _logoShadow;
    private Image _logo;

    private const string InputPattern = @"^[a-zA-Z]{2,8}$|^[°¡-ÆR]{1,4}$";

    private void Awake()
    {
        ShowOpening();
    }
    public void ShowOpening()
    {
        _logInField.SetActive(false);
        _background = transform.GetChild(0).GetComponent<Image>();
        _logoShadow = _background.transform.GetChild(0).GetComponent<Image>();
        _logo = _logoShadow.transform.GetChild(0).GetComponent<Image>();

        RunOpening().Forget();
    }
    public void SetName()
    {

        _userInput = _inputField.text;
        if (Regex.IsMatch(_userInput, InputPattern))
        {
            _logInField.SetActive(false);
            _loadingObject.SetActive(true);
            _loadingObject.transform.GetChild(0).GetComponent<LoadingImage>()?.StartRotation();
        }
        else
        {
            _errorMessage.enabled = true;
        }
    }

    private async UniTask RunOpening()
    {
        ChangeColor(_background, Color.black, Color.white).Forget();
        await ChangeSize(_logoShadow.GetComponent<RectTransform>(), Vector3.zero, _targetBigSize, 0.5f);
        await ChangeSize(_logoShadow.GetComponent<RectTransform>(), _targetBigSize, Vector3.one, 0.3f);
        await UniTask.Delay(1000);
        ShowLogIn();
    }
    private async UniTask ChangeColor(Image image, Color startColor, Color targetColor)
    {
        float fadeInSpeed = 1 / _fadeInTime;
        image.color = startColor;

        while (image.color != targetColor)
        {
            float fadeInAmount = Time.deltaTime * fadeInSpeed;

            image.color = new Color(Mathf.Clamp01(image.color.r + fadeInAmount),
                Mathf.Clamp01(image.color.g + fadeInAmount),
                Mathf.Clamp01(image.color.b + fadeInAmount),
                Mathf.Clamp01(image.color.a + fadeInAmount));

            await UniTask.DelayFrame(1);
        }
    }

    private async UniTask ChangeSize(RectTransform rectTransform, Vector3 startSize, Vector3 targetSize, float popUpTime)
    {
        float popUpSpeed = 1 / popUpTime;
        float initialDifference = startSize.x - targetSize.x;
        // ½ÃÀÛ Å©±â°¡ ´õ Ä¿¼­ ÀÛ¾ÆÁ®¾ß ÇÑ´Ù¸é -1 °öÇÏ±â
        if (Method.IsPositive(initialDifference))
        {
            popUpSpeed *= -1;
        }
        rectTransform.localScale = startSize;

        //ºÎÈ£°¡ ´Þ¶óÁú ¶§±îÁö ½ÇÇà
        while (Method.IsPositive(initialDifference * (rectTransform.localScale.x - targetSize.x)))
        {
            float popUpAmount = Time.deltaTime * popUpSpeed;
            rectTransform.localScale = new Vector3(rectTransform.localScale.x + popUpAmount,
                rectTransform.localScale.y + popUpAmount,
                rectTransform.localScale.z + popUpAmount);

            await UniTask.DelayFrame(1);
        }
        rectTransform.localScale = targetSize;
    }
    private void ShowLogIn()
    {
        _logInField.SetActive(true);
    }
}
