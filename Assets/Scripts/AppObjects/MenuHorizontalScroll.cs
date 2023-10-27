using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuHorizontalScroll : AppObject, IEndDragHandler
{

    public GameObject exitZone;

    public Vector3 originalTransformPosition;
    private Transform content;
    private Transform menuHolder;
    private bool isOut = false;

    // Use this for initialization
    void Start () {
        content = this.GetComponent<ScrollRect>().content;
        menuHolder = content.GetChild(1);
        originalTransformPosition = content.position;

        isOut = false;
}

    public void checkExit() {
        if (this.transform.parent.InverseTransformPoint(menuHolder.position).x < this.transform.parent.InverseTransformPoint(exitZone.transform.position).x)
        {
            isOut = true;
        }
        else {
            isOut = false;
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isOut)
        {
            this.GetComponentInParent<MenuViewModel>().OnCloseMenuButtonOnClick();
        }
        else
        {
            content.position = originalTransformPosition;
        }

    }

    public void setBeginPosition()
    {
        content.position = originalTransformPosition;
    }

}
