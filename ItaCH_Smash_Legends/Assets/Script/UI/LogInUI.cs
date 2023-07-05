using Cysharp.Threading.Tasks;
using System;
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
    [SerializeField] private TextMeshProUGUI _connectionInfoText;

    private Image _background;
    private float _fadeInTime = 1f;
    private Vector3 _targetBigSize = new Vector3(1.5f, 1.5f, 1.5f);

    private Image _logoShadow;
    private Image _logo;

    private const string InputPattern = @"^[a-zA-Z가-힣]{2,8}$";

    public event Action OnAcceptNickname;

    private void Start()
    {
        Init();
        ShowOpening();
    }

    private void Init()
    {
        // 지금 얘가 문제
        Managers manager = Managers.Instance;
        // LobbyManager에서 서버 접속 상태 변경에 따라 UI 텍스트 변경 위한 구독
        Managers.LobbyManager.OnUpdateConnctionInfo -= UpdateConnectionInfoText;
        Managers.LobbyManager.OnUpdateConnctionInfo += UpdateConnectionInfoText;

        // LobbyManager에서 서버 접속 확인 후 UI 비활성화를 위한 구독
        Managers.LobbyManager.OnLogInSuccess -= CloseUI;
        Managers.LobbyManager.OnLogInSuccess += CloseUI;
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
            Managers.UserManager.UserLocalData.Name = _userInput;
            _logInField.SetActive(false);
            _loadingObject.SetActive(true);
            _loadingObject.transform.GetChild(0).GetComponent<RotatingImage>()?.StartRotation();
            Managers.LobbyManager.ConnectToServer();
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
        // 시작 크기가 더 커서 작아져야 한다면 -1 곱하기
        if (Method.IsPositive(initialDifference))
        {
            popUpSpeed *= -1;
        }
        rectTransform.localScale = startSize;

        //부호가 달라질 때까지 실행
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

    private void CloseUI()
    {
        Destroy(gameObject);
    }

    private void UpdateConnectionInfoText(string connectionInfo)
    {
        _connectionInfoText.text = connectionInfo;
    }
}
