using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnsuccessfulBuyPopUpViewModel : ViewModel
{
    public GameObject noStockWarningObj;
    public GameObject noCoinsWarningObj;
    public GameObject unexpectedErrorObj;

    private bool isScanSceneActive = false;

    public override void Initialize(params object[] list)
    {
        noStockWarningObj.SetActive(false);
        noCoinsWarningObj.SetActive(false);
        unexpectedErrorObj.SetActive(false);

        isScanSceneActive = (bool)list[0];
        switch ((string)list[1])
        {
            case "api.error.no_stock":
                noStockWarningObj.SetActive(true);
                break;
            case "api.error.not_enough_coins":
                noCoinsWarningObj.SetActive(true);
                break;
            case "":
                unexpectedErrorObj.SetActive(true);
                break;
        }
    }

    public void AcceptButtonOnClick()
    {
        if (isScanSceneActive)
        {
            ScanManager.instnace.ResetQRScan();
            this.gameObject.SetActive(false);
        }
        else {
            ScreenManager.instance.BackToPreviousView();
        }       
    }
}
