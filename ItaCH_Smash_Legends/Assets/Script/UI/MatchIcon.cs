using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MatchIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private Image _matchedImage;
    [SerializeField] private float _firstTurnSpeed;
    [SerializeField] private float _secondTurnSpeed;

    private float _firstTurnOffset;
    private float _secondTurnOffset;
    private Image _image;
    private RectTransform _rectTransform;

    private int _numberOfSprites;
    private int _currentSpriteIndex;
    private bool _isTurned;
    private bool _isSpriteChanged;
    private Vector3 _counterClockWiseRotateDirection;
    private Vector3 _clockWiseRotateDirection;

    private CancellationTokenSource _cancellationTokenSource;
    private bool _isInitialSettingFinished = false;

    #region const 
    private const int FirstTurnAngle = 40;
    private const int SpriteChangeAngle = 180;
    private const int EndCycleAngle = 10;
    private const int SecondTurnDelay = 200;
    private const int FirstTurnDelay = 1000;
    #endregion

    public void InitMatchIconSettings()
    {
        _image = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _numberOfSprites = _sprites.Length;
        _currentSpriteIndex = 0;
        _image.sprite = _sprites[_currentSpriteIndex];
        _counterClockWiseRotateDirection = Vector3.forward;
        _clockWiseRotateDirection = Vector3.back;
        _firstTurnOffset = _firstTurnSpeed * Time.fixedDeltaTime;
        _secondTurnOffset = _secondTurnSpeed * Time.fixedDeltaTime;
        _isInitialSettingFinished = true;
    }
    private void OnEnable()
    {
        if (_isInitialSettingFinished)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _matchedImage.enabled = false;
            RotateIcons(_cancellationTokenSource.Token).Forget();
        }
    }
    private void OnDisable()
    {
        _cancellationTokenSource?.Cancel();
    }
    private async UniTask RotateIcons(CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (_isTurned)
            {
                _rectTransform.Rotate(_clockWiseRotateDirection * (_secondTurnSpeed * Time.fixedDeltaTime));
                if (!_isSpriteChanged && Utils.CalculateAbsolute(180 - Utils.CalculateAbsolute(_rectTransform.rotation.eulerAngles.z)) < _secondTurnOffset)
                {
                    _rectTransform.rotation = Quaternion.Euler(0, 0, SpriteChangeAngle);
                    ++_currentSpriteIndex;
                    if (_currentSpriteIndex >= _numberOfSprites)
                    {
                        _currentSpriteIndex = 0;
                    }
                    _image.sprite = _sprites[_currentSpriteIndex];
                    _isSpriteChanged = true;
                }
                else if (_isSpriteChanged && _rectTransform.rotation.eulerAngles.z <= EndCycleAngle)
                {
                    _rectTransform.rotation = Quaternion.Euler(0, 0, 0);
                    await UniTask.Delay(FirstTurnDelay);
                    _isTurned = false;
                    _isSpriteChanged = false;
                }
            }
            else
            {
                _rectTransform.Rotate(_counterClockWiseRotateDirection * (_firstTurnSpeed * Time.fixedDeltaTime));
                if (_rectTransform.rotation.eulerAngles.z >= FirstTurnAngle)
                {
                    await UniTask.Delay(SecondTurnDelay);
                    _isTurned = true;
                    _rectTransform.rotation = Quaternion.Euler(0, 0, FirstTurnAngle);
                }
            }
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }
    }
    public void SetMatchCompleteImage()
    {
        _matchedImage.enabled = true;
    }
}
