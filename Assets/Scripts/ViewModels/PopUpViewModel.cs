using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpViewModel : ViewModel
{
    [Header("Texts to change")]
    public Text[] titleText;
    public Text[] bodyText;
    public Text[] actionButtonText;
    public Text exitButtonText;
    [Header("PopUpObjectHolders")]
    public GameObject centralPopUp;
    public GameObject downwardPopUp;

    private Action action;
    private PopUpViewModelTypes type;

    public override void Initialize(params object[] list)
    {
        type = (PopUpViewModelTypes)list[0];

        if (type == PopUpViewModelTypes.Central)
        {
            titleText[0].text = (string)list[1];
            bodyText[0].text = (string)list[2];
            //actionButtonText[0].text = (string)list[3];
            //actionButtonText[0].color = (Color)list[4];
            centralPopUp.SetActive(true);
            downwardPopUp.SetActive(false);
            return;
        }

        if (type == PopUpViewModelTypes.Downwards)
        {
            titleText[1].text = (string)list[1];
            bodyText[1].text = (string)list[2];
            actionButtonText[1].text = (string)list[3];
            exitButtonText.text = (string)list[4];
            //actionButtonText[1].color = (Color)list[5];
            //exitButtonText.color = (Color)list[6];
            downwardPopUp.SetActive(true);
            centralPopUp.SetActive(false);
            return;
        }

    }

    public void SetPopUpAction(System.Action action) {
        this.action = action;
    }

    public void ActionButtonOnClick() {
        action();
    }

    public void ExitButtonOnClick() {
        ScreenManager.instance.BackToPreviousView();
    }

}