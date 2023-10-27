using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UPCardViewModel : ViewModel
{
    [Header("Texts to change")]
    public Text UPCoinsText;


    public override void Initialize(params object[] list)
    {
        UPCoinsText.text = (string)list[0];
    }


    public void ExitButtonOnClick()
    {
        ScreenManager.instance.BackToPreviousView();
    }

}
