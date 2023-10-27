using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderViewModel : ViewModel
{
    [Header("Game Objects Settings")]
    public Button actionButton;

    [Header("UI Settings")]
    public Sprite menuSprite;
    public Sprite backSprite;

    protected Action actionButtonAction;

    public void SetActionButtonAction(Action _action)
    {
        actionButtonAction = _action;
    }

    public void ActionButtonOnClick() {
        actionButtonAction();
    }

    public void SetMenuSprite() {
        actionButton.transform.GetChild(0).GetComponent<Image>().sprite = menuSprite;
    }

    public void SetBackSprite() {
        actionButton.transform.GetChild(0).GetComponent<Image>().sprite = backSprite;
    }

}
