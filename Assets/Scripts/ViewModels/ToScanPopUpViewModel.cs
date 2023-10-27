using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToScanPopUpViewModel : ViewModel
{
    public void OnOpenScanButtonOnClick() {
        SceneTransitionManager.instance.startLoadScene("ScanScene");
    }

    public void OnCancelButtonOnClick()
    {
        ScreenManager.instance.BackToPreviousView();
    }
}
