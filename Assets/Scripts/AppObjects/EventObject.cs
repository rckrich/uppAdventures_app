using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Globalization;


public class EventObject : AppObject
{
    public Image eventTypeImageBG;
    public Image eventTypeImage;
    //public Text eventTitleText;
    public TextMeshProUGUI eventTitleText;
    public Text eventDateText;
    public Text eventTimeText;
    public Text eventPointsText;

    public EventEntity upEvent;

    private bool hasAssited = false;

    private CultureInfo cultureInfoForTime = CultureInfo.CreateSpecificCulture("en-US");


    public override void Initialize(params object[] list)
    {
        upEvent = (EventEntity)list[0];
        eventTypeImageBG.color = AppManager.instance.GetEventTypeColor(upEvent.category_id);
        eventTypeImage.sprite = AppManager.instance.GetEventTypeSprite(upEvent.category_id);
        eventTitleText.text = upEvent.name;
        eventDateText.text = upEvent.start.Day.ToString() + "/" + upEvent.start.Month.ToString() + "/" + upEvent.start.Year.ToString();
        //eventTimeText.text = upEvent.start.Hour.ToString() + ":" + upEvent.start.Minute.ToString();
        eventTimeText.text = upEvent.start.ToString("h:mm tt", cultureInfoForTime);
        eventPointsText.text = "+" + upEvent.coins.ToString("#,##0");
    }

    public void HasAssited(bool _value) {
        hasAssited = _value;
    }

    public void EventButtonOnClick()
    {
        if (hasAssited)
        {
            InvokeEvent<AssitedEventButtonOnClickEvent>(new AssitedEventButtonOnClickEvent(upEvent));
        }
        else {
            InvokeEvent<EventButtonOnClickEvent>(new EventButtonOnClickEvent(upEvent));
        }
        
    }
}
