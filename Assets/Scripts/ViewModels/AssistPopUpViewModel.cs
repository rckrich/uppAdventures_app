using UnityEngine;
using UnityEngine.UI;

public class AssistPopUpViewModel : ViewModel
{
    [Header("Buy Pop Up Objects")]
    public Text eventNameText;
    public Image eventTypeImageBG;
    public Image eventTypeImage;
    public Text eventDeparment;
    public Text eventType;
    public Text coinsToAdd;
    public Text pointsToAdd;
    public Button assistButton;

    private EventEntity eventEntity;

    public override void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list)
    {
        base.Initialize<TViewModel, TPresenter, TInteractor>(list);
        AppManager.instance.LoadingViewModelSetActive(true);
        CallPresenter(AssistMethods.GetEvent, list[0]);
    }

    public override void DisplayOnResult(params object[] list)
    {
        eventEntity = (EventEntity)list[0];

        eventNameText.text = eventEntity.name;

        eventTypeImageBG.color = AppManager.instance.GetEventTypeColor(eventEntity.category_id);
        eventTypeImage.sprite = AppManager.instance.GetEventTypeSprite(eventEntity.category_id);

        eventType.text = AppManager.instance.GetEventTypeName(eventEntity.category_id);
        eventDeparment.text = eventEntity.department;
        coinsToAdd.text = eventEntity.coins.ToString("#,##0") + " UP Coins";
        pointsToAdd.text = "+" + eventEntity.coins.ToString("#,##0") + " Puntos " + AppManager.instance.GetEventTypeName(eventEntity.category_id);

    }

    public void AssistButtonOnClick()
    {
        CallPresenter(AssistMethods.PostAssist, eventEntity.id);
    }

    public void CancelButtonOnClick()
    {
        ScanManager.instnace.ResetQRScan();
        this.gameObject.SetActive(false);
    }
}
