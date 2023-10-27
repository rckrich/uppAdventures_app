using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssistedEventFeedViewModel : ViewModel
{
    private const int MIN_EVENTS_COUNT = 20;

    [Header("Event Object Reference")]
    public GameObject eventObjectPrefab;
    [Header("Event Object Parent Transform Reference")]
    public Transform eventParentTransform;
    [Header("Objects to Never Delete")]
    public GameObject[] noDeleteObjects;
    [Header("No Events Text Object")]
    public GameObject noEventsTextObject;
    public GameObject contentLimiter;
    public GameObject oldContentLimiter;
    [Header("General")]
    public Transform content;
    public Animator animator;

    private bool isLoadMore;
    private ScrollRectReloaderObject reloader;
    private ScrollRectReloaderObject loadMore;
    private int lastEventID;
    private int eventCounter = 0;
    private int pageCounter;
    private List<EventEntity> events;

    private void OnEnable()
    {
        content.localPosition = new Vector3(content.localPosition.x, 0f);
    }

    public override void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list)
    {
        base.Initialize<TViewModel, TPresenter, TInteractor>(list);
        reloader = content.transform.parent.GetComponentsInParent<ScrollRectReloaderObject>()[0];
        loadMore = content.transform.parent.GetComponentsInParent<ScrollRectReloaderObject>()[1];
        reloader.SetReloadAction(() => { ReloadEventObjects(); });
        loadMore.SetReloadAction(() => { LoadMoreEventFeed(); });
        eventCounter = 0;
        pageCounter = 1;
        isLoadMore = false;
        SetHeaderAction();
        InitializeEventFeed();
    }

    public override void DisplayOnResult(params object[] list)
    {
        events = ((GetEvents)list[0]).page;

        if (events.Count <= 0)
        {
            if (!isLoadMore)
            {
                DeleteEventObjects();
                noEventsTextObject.SetActive(true);
            }
            AppManager.instance.LoadingViewModelSetActive(false);
            return;
        }

        if (!isLoadMore)
            DeleteEventObjects();

        isLoadMore = false;
        InstanceEventObject(events, ((GetEvents)list[0]).next_page);
    }

    public void ReloadEventObjects()
    {
        if (!CheckDeviceNetworkReachability())
        {
            NoInternetPopUp();
            return;
        }

        eventCounter = 0;
        pageCounter = 1;
        DeleteEventObjects();
        InitializeEventFeed();
    }

    private void LoadMoreEventFeed()
    {
        if (!CheckDeviceNetworkReachability())
        {
            NoInternetPopUp();
            return;
        }

        if (eventCounter < (MIN_EVENTS_COUNT * (pageCounter - 1)))
        {
            return;
        }

        isLoadMore = true;
        AppManager.instance.LoadingViewModelSetActive(true);
        noEventsTextObject.SetActive(false);
        presenter.CallInteractor(pageCounter);
    }

    private void InitializeEventFeed()
    {
        if (!CheckDeviceNetworkReachability())
        {
            NoInternetPopUp();
            return;
        }

        AppManager.instance.LoadingViewModelSetActive(true);
        noEventsTextObject.SetActive(false);
        presenter.CallInteractor(pageCounter);
    }

    private void InstanceEventObject(List<EventEntity> eventsEntities, int nextPage)
    {
        if (eventsEntities.Count <= 0)
        {
            noEventsTextObject.SetActive(true);
            return;
        }

        lastEventID = eventsEntities[eventsEntities.Count - 1].id;
        pageCounter = nextPage;


        for (int i = 0; i < eventsEntities.Count; i++)
        {
            GameObject eventObject = GameObject.Instantiate(eventObjectPrefab, eventParentTransform);
            EventObject eventObjectComponent = eventObject.GetComponent<EventObject>();
            eventObjectComponent.Initialize(eventsEntities[i]);
            eventObjectComponent.HasAssited(true);
            eventObjectComponent.transform.SetAsLastSibling();
        }

        contentLimiter.transform.SetAsFirstSibling();
        oldContentLimiter.transform.SetAsLastSibling();
        noEventsTextObject.SetActive(false);
        AppManager.instance.LoadingViewModelSetActive(false);
    }

    private void DeleteEventObjects()
    {
        for (int i = 0; i < eventParentTransform.childCount; i++)
        {
            if (eventParentTransform.GetChild(i).gameObject != contentLimiter)
            {
                if (eventParentTransform.GetChild(i).gameObject != oldContentLimiter)
                {
                    if (eventParentTransform.GetChild(i).gameObject != noEventsTextObject)
                    {
                        if (CanObjectBeDestroyed(eventParentTransform.GetChild(i).gameObject))
                        {
                            Destroy(eventParentTransform.GetChild(i).gameObject);
                        }
                    }
                }
            }
        }

        contentLimiter.transform.SetAsFirstSibling();
        oldContentLimiter.transform.SetAsLastSibling();
    }

    private bool CanObjectBeDestroyed(GameObject _gameObject)
    {
        for (int i = 0; i < noDeleteObjects.Length; i++)
        {
            if (_gameObject == noDeleteObjects[i])
            {
                return false;
            }
        }
        return true;
    }

    private void NoInternetPopUp()
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Sin conexión",
            "Necesitas internet para poder cargar la información, asegúrate de tener conexión a internet e intenta de nuevo");
        popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
    }

    public override void SetHeaderAction()
    {
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
            //ScreenManager.instance.BackToPreviousView();
            //ScreenManager.instance.GetView(ViewID.ProfileViewModel).SetHeaderAction();
            StartCoroutine(CloseRoutine());

        });
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetBackSprite();
    }

    IEnumerator CloseRoutine()
    {
        animator.SetTrigger("Activate");

        yield return new WaitForSeconds(0.35f);

        ScreenManager.instance.BackToPreviousView();
        ScreenManager.instance.GetView(ViewID.ProfileViewModel).SetHeaderAction();
    }
}
