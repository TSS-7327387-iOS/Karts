
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using PowerslideKartPhysics;
using System.Collections;

public class TouchButtonManager : MonoBehaviour
{
    public GameObject leftButton;
    public GameObject rightButton;
    //public GameObject rearMirrorButton; // Add reference to RearMirrorButton
    public List<GameObject> ignoredUIElements; // List of UI elements to ignore
    private List<int> activeLeftTouches = new List<int>();
    private List<int> activeRightTouches = new List<int>();

    private int latestTouchID = -1;
    private bool latestTouchIsLeft = false;

    public InputManager InputManager;

    private RectTransform leftButtonRect;
    private RectTransform rightButtonRect;
    private float canvasWidth;
    private float canvasHeight;
    public UIControl kartUI;

    void Start()
    {
        leftButtonRect = leftButton.GetComponent<RectTransform>();
        rightButtonRect = rightButton.GetComponent<RectTransform>();

        canvasWidth = Screen.width;
        canvasHeight = Screen.height;
    }

    void Update()
    {
        HandleTouchInput();
    }
    private Dictionary<int, float> touchStartTimes = new Dictionary<int, float>(); // Store touch start times
    private bool isDrifting = false; // Track drift state
    void HandleTouchInput()
    {
        foreach (Touch touch in Input.touches)
        {
            Vector2 touchPosition = touch.position;

            // **Check if the touch is over the UI button**
            if (IsTouchOverUIElements(ignoredUIElements))
            {
                return; // Ignore input but let the button work normally
            }

            if (touch.phase == TouchPhase.Began)
            {
                if (touchPosition.y < Screen.height / 4)
                {
                    return;
                }

                if (touchPosition.x < Screen.width / 2) // Left side
                {
                    activeLeftTouches.Add(touch.fingerId);
                    latestTouchID = touch.fingerId;
                    latestTouchIsLeft = true;
                    touchStartTimes[touch.fingerId] = Time.time; // Store start time
                    SetButtonPosition(leftButton, leftButtonRect, touchPosition);
                }
                else // Right side
                {
                    activeRightTouches.Add(touch.fingerId);
                    latestTouchID = touch.fingerId;
                    latestTouchIsLeft = false;
                    touchStartTimes[touch.fingerId] = Time.time; // Store start time
                    SetButtonPosition(rightButton, rightButtonRect, touchPosition);
                }
            }

            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (touch.fingerId == latestTouchID)
                {
                    if (latestTouchIsLeft)
                        SetButtonPosition(leftButton, leftButtonRect, touch.position);
                    else
                        SetButtonPosition(rightButton, rightButtonRect, touch.position);
                }
            }
            /*else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (activeLeftTouches.Contains(touch.fingerId))
                    activeLeftTouches.Remove(touch.fingerId);
                if (activeRightTouches.Contains(touch.fingerId))
                    activeRightTouches.Remove(touch.fingerId);

                if (touch.fingerId == latestTouchID)
                {
                    if (activeRightTouches.Count > 0)
                    {
                        latestTouchID = activeRightTouches[activeRightTouches.Count - 1];
                        latestTouchIsLeft = false;
                        InputManager.SetSteerMobile(0);
                        InputManager.SetDriftMobile(false);
                    }
                    else if (activeLeftTouches.Count > 0)
                    {
                        latestTouchID = activeLeftTouches[activeLeftTouches.Count - 1];
                        latestTouchIsLeft = true;
                        InputManager.SetSteerMobile(0);
                        InputManager.SetDriftMobile(false);
                    }
                    else
                    {
                        latestTouchID = -1;
                    }
                }
                else
                {
                    if (kartUI.targetKart != null)
                        kartUI.targetKart.CancelDrift();
                }
            }*/
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (activeLeftTouches.Contains(touch.fingerId))
                    activeLeftTouches.Remove(touch.fingerId);
                if (activeRightTouches.Contains(touch.fingerId))
                    activeRightTouches.Remove(touch.fingerId);

                if (touchStartTimes.ContainsKey(touch.fingerId))
                    touchStartTimes.Remove(touch.fingerId);

                if (touch.fingerId == latestTouchID)
                {
                    if (activeRightTouches.Count > 0)
                    {
                        latestTouchID = activeRightTouches[activeRightTouches.Count - 1];
                        latestTouchIsLeft = false;
                        InputManager.SetSteerMobile(0);
                        InputManager.SetDriftMobile(false);
                        isDrifting = false;
                    }
                    else if (activeLeftTouches.Count > 0)
                    {
                        latestTouchID = activeLeftTouches[activeLeftTouches.Count - 1];
                        latestTouchIsLeft = true;
                        InputManager.SetSteerMobile(0);
                        InputManager.SetDriftMobile(false);
                        isDrifting = false;
                    }
                    else
                    {
                        latestTouchID = -1;
                        isDrifting = false;
                    }
                }
                else
                {
                    if (kartUI.targetKart != null)
                        kartUI.targetKart.CancelDrift();
                }
            }
        }

        /*   if (latestTouchID != -1)
           {
               if (latestTouchIsLeft)
               {
                   InputManager.SetSteerMobile(-1);
                   InputManager.SetDriftMobile(true);

               }
               else
               {
                   InputManager.SetSteerMobile(1);
                   InputManager.SetDriftMobile(true);

               }
           }
           else
           {
               InputManager.SetSteerMobile(0);
               InputManager.SetDriftMobile(false);
           }*/
        if (latestTouchID != -1)
        {
            float holdDuration = Time.time - (touchStartTimes.ContainsKey(latestTouchID) ? touchStartTimes[latestTouchID] : 0);

            if (latestTouchIsLeft)
            {
                InputManager.SetSteerMobile(-1);

                if (holdDuration > 0.5f && !isDrifting) // Changed from 1f to 0.5f
                {
                    InputManager.SetDriftMobile(true);
                    isDrifting = true;
                }
            }
            else
            {
                InputManager.SetSteerMobile(1);

                if (holdDuration > 0.5f && !isDrifting) // Changed from 1f to 0.5f
                {
                    InputManager.SetDriftMobile(true);
                    isDrifting = true;
                }
            }
        }
        else
        {
            InputManager.SetSteerMobile(0);
            InputManager.SetDriftMobile(false);
            isDrifting = false; // Reset drift state when no touch is active
        }


        // **Cancel Drift Coroutine if No Active Touches**


        leftButton.SetActive(activeLeftTouches.Count > 0);
        rightButton.SetActive(activeRightTouches.Count > 0);

    }
  
    void SetButtonPosition(GameObject button, RectTransform buttonRect, Vector2 position)
    {
        button.transform.position = ClampPosition(position, buttonRect);
    }

    private Vector2 ClampPosition(Vector2 position, RectTransform buttonRect)
    {
        float buttonWidth = buttonRect.rect.width;
        float buttonHeight = buttonRect.rect.height;

        float clampedX = Mathf.Clamp(position.x, buttonWidth / 2, canvasWidth - buttonWidth / 2);
        float clampedY = Mathf.Clamp(position.y, buttonHeight / 2, canvasHeight - buttonHeight / 2);

        return new Vector2(clampedX, clampedY);
    }

    // **Method to Check if Touch is Over a Specific UI Element**
    /*  private bool IsTouchOverUIElement(GameObject uiElement)
      {
          PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.GetTouch(0).position };
          List<RaycastResult> results = new List<RaycastResult>();
          EventSystem.current.RaycastAll(eventData, results);

          foreach (RaycastResult result in results)
          {
              if (result.gameObject == uiElement)
              {
                  return true;
              }
          }
          return false;
      }*/
    private bool IsTouchOverUIElements(List<GameObject> uiElements)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = Input.GetTouch(0).position };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (uiElements.Contains(result.gameObject)) // Check if the result is in the list
            {
                return true;
            }
        }
        return false;
    }
}

