using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnsuccessfulAssistPopUpViewModel : ViewModel
{
    public GameObject notAvailableWarningObj;
    public GameObject assistedEventWarningObj;
    public GameObject noStartedEventWarningObj;
    public GameObject notInCampusEventWarningObj;

    public override void Initialize(params object[] list)
    {
        notAvailableWarningObj.SetActive(false);
        assistedEventWarningObj.SetActive(false);
        noStartedEventWarningObj.SetActive(false);
        notInCampusEventWarningObj.SetActive(false);
        switch ((string)list[0]) {
            case "api.error.event_finished":
                notAvailableWarningObj.SetActive(true);
                break;
            case "api.error.event_already_attended":
                assistedEventWarningObj.SetActive(true);
                break;
            case "api.error.event_not_started":
                noStartedEventWarningObj.SetActive(true);
                break;
            case "api.error.not_in_campus":
                notInCampusEventWarningObj.SetActive(true);
                break;
        }
    }

    public void AcceptButtonOnClick()
    {
        ScanManager.instnace.ResetQRScan();
        this.gameObject.SetActive(false);
    }
}
