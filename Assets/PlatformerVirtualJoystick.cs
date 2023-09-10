using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class PlatformerVirtualJoystick : OnScreenControl, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Image horizontalJoystick;
    [SerializeField] private Image verticalJoystick;
    public float horizontalLimit = 95f;
    public float verticalLimit = 40f;
    [InputControl(layout ="Stick")]
    [SerializeField]
    private string m_controlPath;

    private Vector2 inputVector = Vector2.zero;

    protected override string controlPathInternal {
        get {
            //Debug.Log("aaaaa");
            return m_controlPath;
        }
        set => m_controlPath = value; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position - (Vector2)transform.position;
        inputVector = new Vector2(Mathf.Clamp(pos.x, -horizontalLimit, horizontalLimit), Mathf.Clamp(pos.y, 0, verticalLimit));

        horizontalJoystick.rectTransform.localPosition = new Vector2(inputVector.x, 0);
        verticalJoystick.rectTransform.localPosition = new Vector2(0, inputVector.y);

        SendValueToControl(inputVector);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        horizontalJoystick.rectTransform.localPosition = Vector2.zero;
        verticalJoystick.rectTransform.localPosition = Vector2.zero;
        SendValueToControl(Vector2.zero);
    }

    private void OnMove(InputAction.CallbackContext callbackContext)
    {
        
    }
}
