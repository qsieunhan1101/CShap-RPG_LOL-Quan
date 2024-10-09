using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickCheckInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool isTouching = false;
    public Vector3 dir = Vector3.zero;

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouching = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isTouching == true)
        {
            FixedJoystick joy = GetComponent<FixedJoystick>();
            dir = new Vector3(joy.Horizontal, 0, joy.Vertical).normalized;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
        Debug.Log("Dirr "+dir);
    }
}
