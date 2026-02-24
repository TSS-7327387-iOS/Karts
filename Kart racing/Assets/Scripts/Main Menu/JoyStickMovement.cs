
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class JoyStickMovement : MonoBehaviour
{
    [SerializeField] private Vector2 JoystickSize = new Vector2(200, 200);
    private Finger MovementFinger;
    [SerializeField] RectTransform joyStickObj;
    [SerializeField] Image knob;
    // Start is called before the first frame update
    Vector2 startPos;

    //Vector3 joyStickPosition;

    private void Awake()
    {
        //joyStickPosition = joyStickObj.localPosition;
    }


    private void Start()
    {
        startPos = joyStickObj.anchoredPosition;
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += HandleFingerDown;
        ETouch.Touch.onFingerUp += HandleLoseFinger;
        ETouch.Touch.onFingerMove += HandleFingerMove;


        //MyChange
        //joyStickObj.localPosition = joyStickPosition;
        //knob.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= HandleFingerDown;
        ETouch.Touch.onFingerUp -= HandleLoseFinger;
        ETouch.Touch.onFingerMove -= HandleFingerMove;
        EnhancedTouchSupport.Disable();

    }
    private void HandleFingerMove(Finger movedFinger)
    {
        if (movedFinger == MovementFinger)
        {
            Vector2 knobPosition;
            float maxMovement = JoystickSize.x / 2f;
            ETouch.Touch currentTouch = movedFinger.currentTouch;

            if (Vector2.Distance(
                currentTouch.screenPosition,
                joyStickObj.anchoredPosition
            ) > maxMovement)
            {
                knobPosition = (
                                   currentTouch.screenPosition - joyStickObj.anchoredPosition
                               ).normalized
                               * maxMovement;
            }
            else
            {
                knobPosition = currentTouch.screenPosition - joyStickObj.anchoredPosition;
            }

            //Joystick.Knob.anchoredPosition = knobPosition;
        }

    }

    private void HandleFingerDown(Finger touchedFinger)
    {
        if (MovementFinger == null && touchedFinger.screenPosition.x <= Screen.width/2)
        {
            MovementFinger = touchedFinger;
            //Joystick.gameObject.SetActive(true);
            ////knob.enabled=true;
            joyStickObj.sizeDelta = JoystickSize;
            joyStickObj.anchoredPosition = touchedFinger.screenPosition;// ClampStartPosition(touchedFinger.screenPosition);
        }
    }


    private void HandleLoseFinger(Finger lostFinger)
    {
        if (lostFinger == MovementFinger)
        {
            MovementFinger = null;
            //Joystick.Knob.anchoredPosition = Vector2.zero;
            // Joystick.gameObject.SetActive(false);
            joyStickObj.anchoredPosition = startPos;
            ////knob.enabled = false;
        }
    }

    private Vector2 ClampStartPosition(Vector2 startPosition)
    {
        if (startPosition.x < JoystickSize.x / 2)
        {
            startPosition.x = JoystickSize.x / 2;
        }

        if (startPosition.y < JoystickSize.y / 2)
        {
            startPosition.y = JoystickSize.y / 2;
        }
        else if (startPosition.y > Screen.height - JoystickSize.y / 2)
        {
            startPosition.y = Screen.height - JoystickSize.y / 2;
        }

        return startPosition;
    }
}
