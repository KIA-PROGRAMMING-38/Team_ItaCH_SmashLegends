using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class JoyStickController : MonoBehaviour
{
    [SerializeField] private Vector2 _joystickSize = new Vector2(300, 300);
    [SerializeField] private FloatingJoyStick _joystick;
    private PlayerMove _playerMove;
    private PlayerStatus _playerStatus;
    private Animator _animator;
    
    private Finger _movementFinger;
    private Vector2 _movementAmount;
    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        _playerStatus = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleMove;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleMove;
        EnhancedTouchSupport.Disable();
    }

    private void HandleMove(Finger MovedFinger)
    {
        if(MovedFinger == _movementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = _joystickSize.x / 2f;
            ETouch.Touch currentTouch = MovedFinger.currentTouch;

            if (Vector2.Distance(currentTouch.screenPosition, _joystick.rectTransform.anchoredPosition) > maxMovement)
            {
                knobPosition = (currentTouch.screenPosition - _joystick.rectTransform.anchoredPosition).normalized * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - _joystick.rectTransform.anchoredPosition;
            }

            _joystick.Knob.anchoredPosition = knobPosition;
            _movementAmount = knobPosition / maxMovement;
        }
    }

    private void HandleLoseFinger(Finger LostFinger)
    {
        if(LostFinger == _movementFinger)
        {
            _movementFinger = null;
            _joystick.rectTransform.anchoredPosition = new Vector2(395,290);
            _joystick.Knob.anchoredPosition = Vector2.zero;
            _movementAmount = Vector2.zero;
        }
    }

    private void HandleFingerDown(Finger TouchedFinger)
    {
        if(_movementFinger == null && TouchedFinger.screenPosition.x <= Screen.width / 2f)
        {
            _movementFinger = TouchedFinger;
            _movementAmount = Vector2.zero;
            _joystick.rectTransform.anchoredPosition = ClampStartPosition(TouchedFinger.screenPosition);
        }
    }

    private Vector2 ClampStartPosition(Vector2 StartPoisition)
    {
        if(StartPoisition.x < _joystickSize.x / 2)
        {
            StartPoisition.x = _joystickSize.x / 2;
        }
        if(StartPoisition.y < _joystickSize.y / 2)
        {
            StartPoisition.y = _joystickSize.y / 2;
        }
        else if(StartPoisition.y > Screen.height - _joystickSize.y / 2)
        {
            StartPoisition.y = Screen.height - _joystickSize.y / 2;
        }

        return StartPoisition;
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 scaledMovement = 1.5f * Time.deltaTime * new Vector3(_movementAmount.x, 0, _movementAmount.y);

        if (scaledMovement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(scaledMovement);
            transform.Translate(Vector3.forward * (5.4f * Time.deltaTime));
            _animator.SetBool(AnimationHash.Run, true);
        }
        else
        {
            _animator.SetBool(AnimationHash.Run, false);
        }
    }
    }
