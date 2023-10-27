using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class EventDetailViewModel : ViewModel
{
    [Header("Event Name Object")]
    public Text eventNameText;
    [Header("Event Header Objects")]
    public Image eventTypeImageBG;
    public Image eventTypeImage;
    public Text eventTypeName;
    public Text eventDepartmentName;
    [Header("Event Body Objects")]
    public Text eventLocationText;
    public Text eventStartDateText;
    public Text eventEndDateText;
    public Text eventStarTimeText;
    public Text eventEndTimeText;
    public Text eventDateAndTimeText;
    [Header("Event Economy Objects")]
    public Text eventCoinsText;
    public Text eventPointsText;
    [Header("Event Description Objects")]
    public Text eventDescriptionText;
    [Header("General")]
    public Transform content;
    public Animator animator;

    private EventEntity upEvent;
    private bool assited;

    private CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("es-ES");
    private CultureInfo cultureInfoForTime = CultureInfo.CreateSpecificCulture("en-US");

    private void OnEnable()
    {
        content.localPosition = new Vector3(content.localPosition.x, 0f);
        StartCoroutine(InfoContainer_LayoutRebuild());
    }

    IEnumerator InfoContainer_LayoutRebuild()
    {
        yield return new WaitForSeconds(0.1f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)content.transform);
    }

    public override void Initialize(params object[] list)
    {
        assited = (bool)list[0];
        SetHeaderAction();
        upEvent = (EventEntity)list[1];
        eventNameText.text = upEvent.name;
        eventTypeImageBG.color = AppManager.instance.GetEventTypeColor(upEvent.category_id);
        eventTypeImage.sprite = AppManager.instance.GetEventTypeSprite(upEvent.category_id);
        eventTypeName.text = AppManager.instance.GetEventTypeName(upEvent.category_id);
        eventDepartmentName.text = upEvent.department;
        eventLocationText.text = upEvent.place;

        //****OPCIÓN 1 -> NO SE USO****
        //*
        //eventStartDateText.text = upEvent.start.Day.ToString() + "-" + upEvent.start.Month.ToString() + "-" + upEvent.start.Year.ToString();
        //eventEndDateText.text = upEvent.end.Day.ToString() + "-" + upEvent.end.Month.ToString() + "-" + upEvent.end.Year.ToString();
        //eventStarTimeText.text = upEvent.start.Hour.ToString() + ":" + upEvent.start.Minute.ToString(); // FORMATEAR para salga am - pm
        //eventEndTimeText.text = upEvent.end.Hour.ToString() + ":" + upEvent.end.Minute.ToString();  // FORMATEAR para salga am - pm
        //*
        //****OPCIÓN 2 -> NO SE USO****
        //eventStartDateText.text = upEvent.start.Date.ToString("dddd, dd MMMM yyyy", cultureInfo);
        //eventEndDateText.text = upEvent.end.Date.ToString("dddd, dd MMMM yyyy", cultureInfo);
        //eventStarTimeText.text = upEvent.start.ToString("hh:mm tt", cultureInfoForTime);
        //eventEndTimeText.text = upEvent.end.ToString("hh:mm tt", cultureInfoForTime);
        //*
        //****OPCIÓN 3 -> SI SE USO****
        if (upEvent.start.Date == upEvent.end.Date)
        {
            //****OPCIÓN 3-1 -> NO SE USO****
            //eventDateAndTimeText.text = upEvent.start.Date.ToString("dddd, dd MMMM yyyy", cultureInfo) + "\n" + "de " +
            //    upEvent.start.ToString("hh:mm tt", cultureInfoForTime) + "a " + upEvent.end.ToString("hh:mm tt", cultureInfoForTime);

            //****OPCIÓN 3-2 -> SI SE USO****
            eventDateAndTimeText.text = 
                "El " + upEvent.start.ToString("dddd", cultureInfo) + " " + upEvent.start.ToString("dd") + " de " +
                upEvent.start.ToString("MMMM", cultureInfo) + " de " + upEvent.start.ToString("yyyy") + "\n" + "de " +
                upEvent.start.ToString("hh:mm tt", cultureInfoForTime) + " a " + upEvent.end.ToString("hh:mm tt", cultureInfoForTime);
        }
        else if (upEvent.start.Date != upEvent.end.Date)
        {
            eventDateAndTimeText.text =
                "Del " + upEvent.start.ToString("ddd", cultureInfo) + " " + upEvent.start.ToString("dd") + " de " + upEvent.start.ToString("MMM", cultureInfo) +
                " de " + upEvent.start.ToString("yyyy") + " a las " + upEvent.start.ToString("hh:mm tt", cultureInfoForTime) + " al \n" +
                upEvent.end.ToString("ddd", cultureInfo) + " " + upEvent.end.ToString("dd") + " de " + upEvent.end.ToString("MMM", cultureInfo) +
                " de " + upEvent.start.ToString("yyyy") + " a las " + upEvent.end.ToString("hh:mm tt", cultureInfoForTime);


            //"De las " + upEvent.start.ToString("hh:mm tt", cultureInfoForTime) + " del " + upEvent.start.ToString("ddd", cultureInfo) +
            //", " + upEvent.start.ToString("dd") + " de " + upEvent.start.ToString("MMMM", cultureInfo) + " de " + upEvent.start.ToString("yyyy") +
            //"\n" + "a las " + upEvent.end.ToString("hh:mm tt", cultureInfoForTime) + " del " + upEvent.end.ToString("ddd", cultureInfo) +
            //", " + upEvent.end.ToString("dd") + " de " + upEvent.end.ToString("MMMM", cultureInfo) + " de " + upEvent.start.ToString("yyyy");
        }


        eventCoinsText.text = upEvent.coins.ToString("#,##0") + " UP Coins";
        eventPointsText.text = upEvent.coins.ToString("#,##0") + " Puntos " + AppManager.instance.GetEventTypeName(upEvent.category_id);
        eventDescriptionText.text = upEvent.description;
    }

    public override void SetHeaderAction()
    {
        if (assited)
        {
            ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
                //ScreenManager.instance.BackToPreviousView();
                //ScreenManager.instance.GetView(ViewID.AssitedEventFeedViewModel).GetComponent<AssistedEventFeedViewModel>().SetHeaderAction();
                StartCoroutine(AssitedEvent_CloseRoutine());
            });
            ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetBackSprite();
        }
        else
        {
            ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
                //ScreenManager.instance.BackToPreviousView();
                //ScreenManager.instance.GetView(ViewID.EventFeedViewModel).GetComponent<EventFeedViewModel>().SetHeaderAction();
                StartCoroutine(Event_CloseRoutine());
            });
            ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetBackSprite();
        }
    }

    IEnumerator AssitedEvent_CloseRoutine()
    {
        animator.SetTrigger("Activate");

        yield return new WaitForSeconds(0.35f);

        ScreenManager.instance.BackToPreviousView();
        ScreenManager.instance.GetView(ViewID.AssitedEventFeedViewModel).GetComponent<AssistedEventFeedViewModel>().SetHeaderAction();
    }

    IEnumerator Event_CloseRoutine()
    {
        animator.SetTrigger("Activate");

        yield return new WaitForSeconds(0.35f);

        ScreenManager.instance.BackToPreviousView();
        ScreenManager.instance.GetView(ViewID.EventFeedViewModel).GetComponent<EventFeedViewModel>().SetHeaderAction();
    }
}
