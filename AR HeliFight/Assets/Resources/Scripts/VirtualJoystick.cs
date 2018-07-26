using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private Image bgImage;
    private Image mainImage;
    private Vector3 InputVector { get; set; }

    void Start()
    {
        bgImage = GetComponent<Image>();
        mainImage = transform.GetChild(0).GetComponent<Image>();
        InputVector = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos=Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImage.rectTransform
                                                                    , eventData.position
                                                                    , eventData.pressEventCamera
                                                                    , out pos))//если клацнули в круге бэкграунда
        {
            pos.x = (pos.x / bgImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImage.rectTransform.sizeDelta.y);

            float x = (bgImage.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (bgImage.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputVector = new Vector3(x, 0, y);
            InputVector = (InputVector.magnitude > 1) ? InputVector.normalized : InputVector;

            mainImage.rectTransform.anchoredPosition = new Vector3(InputVector.x * (bgImage.rectTransform.sizeDelta.x / 3),
                InputVector.z * (bgImage.rectTransform.sizeDelta.y / 3));            
        }
        }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        InputVector = Vector3.zero;
        mainImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    
    public float Horizontal()
    {
        if (InputVector.x != 0)
        {
            return InputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }
    public float Vertical()
    {
        if (InputVector.z != 0)
        {
            return InputVector.z;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
