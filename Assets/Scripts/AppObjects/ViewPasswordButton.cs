using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ViewPasswordButton : AppObject, IPointerDownHandler, IPointerUpHandler
{
    public InputField inputField;


    public void OnPointerDown(PointerEventData eventData)
    {
        inputField.contentType = InputField.ContentType.Standard;
        inputField.ForceLabelUpdate();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputField.contentType = InputField.ContentType.Password;
        inputField.ForceLabelUpdate();
    }
}
