using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFeedPresenter : Presenter
{
    private const string TEST_GET_EVENTS = "134.122.27.5/api/events/";
    private const string GET_EVENTS = "https://upadventures-admin.rckgames.com/api/events/";
    private const int MIN_EVENTS_COUNT = 50;

    private void Start()
    {
        AddEventListener<EventButtonOnClickEvent>(EventButtonOnClickEventListener);
    }

    public override void CallInteractor(params object[] list)
    {
        interactor.PerformSearch(GET_EVENTS, MIN_EVENTS_COUNT, (int)list[0]);
    }

    public override void OnResult(params object[] list)
    {
        viewModel.DisplayOnResult(list);
    }

    public override void OnFailedResult(params object[] list)
    {
        OnErrorMessage();
        viewModel.DisplayOnFailedResult();
    }

    public override void OnServerError(params object[] list)
    {
        OnErrorMessage();
        viewModel.DisplayOnServerError();
    }

    public override void OnNetworkError(params object[] list)
    {
        OnErrorMessage();
        viewModel.DisplayOnNetworkError();
    }

    private void OnErrorMessage()
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Error de conexión",
            "Ha ocurrido un problema, por favor intentalo más tarde");
        popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
    }

    private void EventButtonOnClickEventListener(EventButtonOnClickEvent eventData) {
        ScreenManager.instance.ChangeView(ViewID.EventDetailViewModel, true);
        ScreenManager.instance.GetView(ViewID.EventDetailViewModel).Initialize(false, eventData.GetEventEntity());
    }
}
