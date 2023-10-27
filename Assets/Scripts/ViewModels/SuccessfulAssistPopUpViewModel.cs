using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessfulAssistPopUpViewModel : ViewModel
{
    public Image eventTypeImageBG;
    public Image eventTypeImage;
    public Text eventNameText;
    public Text addedCoinsText;
    public Text addedPointsText;


    private string eventTypeText;
    private bool isScanSceneActive = false;
    private EventEntity eventEntity;

    public override void Initialize(params object[] list)
    {
        eventEntity = (EventEntity)list[0];
        eventNameText.text = eventEntity.name;
        eventTypeText = AppManager.instance.GetEventTypeName(eventEntity.category_id);

        eventTypeImageBG.color = AppManager.instance.GetEventTypeColor(eventEntity.category_id);
        eventTypeImage.sprite = AppManager.instance.GetEventTypeSprite(eventEntity.category_id);
        addedCoinsText.text = eventEntity.coins.ToString("#,##0") + " UP Coins";
        addedPointsText.text = "+" + eventEntity.coins.ToString("#,##0") + " Puntos " + AppManager.instance.GetEventTypeName(eventEntity.category_id);

        isScanSceneActive = (bool)list[1];
    }

    public void AcceptButtonOnClick()
    {
        if (isScanSceneActive)
        {
            ScanManager.instnace.ResetQRScan();
            this.gameObject.SetActive(false);
        }
        else
        {
            ScreenManager.instance.ChangeView(ViewID.StoreViewModel);
            ScreenManager.instance.GetView(ViewID.EventFeedViewModel).Initialize<StoreViewModel, StorePresenter, StoreInteractor>();
        }
    }

    
}
