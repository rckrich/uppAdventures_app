using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deactivate_Object_With_ScrollView : MonoBehaviour
{
    public ScrollRect scrollRect;
    public HeaderViewModel headerViewModel;
    [Range(-1f, 1f)]
    public float porcentageToActivateAction1;
    //[Range(0f, 1f)]
    //public float porcentageToDeactivateAction;
    bool doAction = true;

    //Hay que llamar esta funcion desde el Editor en el Scroll View -> On Value Changeg (Vector2)
    public void Activate_Action()
    {
        /*
        if (scrollRect.verticalNormalizedPosition >= porcentageToActivateAction1)
        {
            objectToDeactivate.SetActive(false);
        }
        */



        if (scrollRect.horizontalNormalizedPosition <= porcentageToActivateAction1 && doAction)
        {
            StartCoroutine(Close_InfoLugar_Section());
        }
    }

    private void OnEnable()
    {
        scrollRect.horizontalNormalizedPosition = 0f;
        doAction = true;
    }


    IEnumerator Close_InfoLugar_Section()
    {
        doAction = false;
        headerViewModel.ActionButtonOnClick();
        scrollRect.horizontalNormalizedPosition = 0f;
        yield return new WaitForSeconds(0.50f);
        doAction = true;
        //objectToDeactivate.SetActive(false);
    }

    private void OnDisable()
    {
        doAction = true;
    }


}
