using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistPopUpPresenter : Presenter
{
    private const string TEST_GET_EVENT = "134.122.27.5/api/events/";
    private const string TEST_ASSIST_EVENT = "134.122.27.5/api/events/attend/id";
    private const string TEST_GET_USER_DATA = "134.122.27.5/api/auth/info";
    private const string GET_EVENT = "https://upadventures-admin.rckgames.com/api/events/";
    private const string ASSIST_EVENT = "https://upadventures-admin.rckgames.com/api/events/attend/id";
    private const string GET_USER_DATA = "https://upadventures-admin.rckgames.com/api/auth/info";

    private EventEntity eventEntity;
    private string errorMessage;

    public override void CallInteractor(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(true);

        AssistMethods assistMethod = (AssistMethods)list[0];

        if (AssistMethods.GetEvent == assistMethod)
        {
            interactor.PerformSearch(assistMethod, GET_EVENT, list[1]);
        }

        if (AssistMethods.PostAssist == assistMethod)
        {
            interactor.PerformSearch(assistMethod, ASSIST_EVENT, list[1]);
        }

        if (AssistMethods.GetUserData == assistMethod)
        {
            interactor.PerformSearch(assistMethod, GET_USER_DATA);
        }
    }

    public override void OnResult(params object[] list)
    {
        AssistMethods assistMethod = (AssistMethods)list[0];

        if (AssistMethods.GetEvent == assistMethod)
        {
            GetEvent getEvent = (GetEvent)list[1];
            eventEntity = getEvent.@event;
            viewModel.DisplayOnResult(getEvent.@event);
            AppManager.instance.LoadingViewModelSetActive(false);
            return;
        }

        if (AssistMethods.PostAssist == assistMethod)
        {
            assistMethod = AssistMethods.GetUserData;
            CallInteractor(assistMethod);
            return;
        }

        if (AssistMethods.GetUserData == assistMethod)
        {

            GetUserEntity getUserEntity = (GetUserEntity)list[1];
            ProgressManager.instance.progress.userDataPersistance.id = getUserEntity.user.id;
            ProgressManager.instance.progress.userDataPersistance.userName = getUserEntity.user.name;
            ProgressManager.instance.progress.userDataPersistance.UPCoins = (getUserEntity.user.coins != null) ? (int)getUserEntity.user.coins : 0;
            ProgressManager.instance.progress.userDataPersistance.puntosAcademicos = getUserEntity.user.student_points[(int)PointsTypes.Acedemic].amount;
            ProgressManager.instance.progress.userDataPersistance.puntosAsuntosEstudiantiles = getUserEntity.user.student_points[(int)PointsTypes.StudentIssues].amount;
            ProgressManager.instance.progress.userDataPersistance.puntosCulturales = getUserEntity.user.student_points[(int)PointsTypes.Cultural].amount;
            ProgressManager.instance.progress.userDataPersistance.puntosDeportivos = getUserEntity.user.student_points[(int)PointsTypes.Sports].amount;
            ProgressManager.instance.progress.userDataPersistance.puntosMovimientoUP = getUserEntity.user.student_points[(int)PointsTypes.UPMovement].amount;

            if (getUserEntity.user.avatar != null)
                ProgressManager.instance.progress.userDataPersistance.avatarThumbnail = getUserEntity.user.avatar.media.absolute_url;

            ScreenManager.instance.ChangeView(ViewID.SuccessfulAssistPopUpViewModel, false);
            ScreenManager.instance.GetView(ViewID.SuccessfulAssistPopUpViewModel).GetComponent<SuccessfulAssistPopUpViewModel>().Initialize(eventEntity, true);
            AppManager.instance.LoadingViewModelSetActive(false);
        }
    }

    public override void OnFailedResult(params object[] list)
    {
        AssistMethods assistMethod = (AssistMethods)list[0];

        if (AssistMethods.GetEvent == assistMethod) {
            OnErrorMessage();
            viewModel.SetActive(false);
            AppManager.instance.LoadingViewModelSetActive(false);
        }

        if (AssistMethods.PostAssist == assistMethod) {
            ErrorEntity errorEntity = (ErrorEntity)list[1];
            errorMessage = errorEntity.message;
            if (errorEntity.message.Equals("api.error.event_finished")) {
                ScreenManager.instance.ChangeView(ViewID.UnsuccessfulAssistPopUpViewModel, false);
                ScreenManager.instance.GetView(ViewID.UnsuccessfulAssistPopUpViewModel).GetComponent<UnsuccessfulAssistPopUpViewModel>().Initialize(errorEntity.message);
            }
            else if (errorEntity.message.Equals("api.error.event_already_attended"))
            {
                ScreenManager.instance.ChangeView(ViewID.UnsuccessfulAssistPopUpViewModel, false);
                ScreenManager.instance.GetView(ViewID.UnsuccessfulAssistPopUpViewModel).GetComponent<UnsuccessfulAssistPopUpViewModel>().Initialize(errorEntity.message);
            }
            else if (errorEntity.message.Equals("api.error.event_not_started"))
            {
                ScreenManager.instance.ChangeView(ViewID.UnsuccessfulAssistPopUpViewModel, false);
                ScreenManager.instance.GetView(ViewID.UnsuccessfulAssistPopUpViewModel).GetComponent<UnsuccessfulAssistPopUpViewModel>().Initialize(errorEntity.message);
            }
            else if (errorEntity.message.Equals("api.error.not_in_campus"))
            {
                ScreenManager.instance.ChangeView(ViewID.UnsuccessfulAssistPopUpViewModel, false);
                ScreenManager.instance.GetView(ViewID.UnsuccessfulAssistPopUpViewModel).GetComponent<UnsuccessfulAssistPopUpViewModel>().Initialize(errorEntity.message);
            }
            else
            {
                OnErrorMessage();
            }

            viewModel.SetActive(false);
            AppManager.instance.LoadingViewModelSetActive(false);
        }

        if (AssistMethods.GetUserData == assistMethod) {
            OnErrorMessage();
            viewModel.SetActive(false);
            AppManager.instance.LoadingViewModelSetActive(false);
        }
        
    }

    public override void OnServerError(params object[] list)
    {
        OnErrorMessage();
        viewModel.SetActive(false);
        AppManager.instance.LoadingViewModelSetActive(false);
    }

    public override void OnNetworkError(params object[] list)
    {
        OnErrorMessage();
        viewModel.SetActive(false);
        AppManager.instance.LoadingViewModelSetActive(false);
    }

    private void OnErrorMessage()
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Error de conexión",
            "Ha ocurrido un problema, por favor intentalo más tarde" + errorMessage);
        popUpViewModel.SetPopUpAction(() => { 
            ScreenManager.instance.BackToPreviousView();
            ScanManager.instnace.ResetQRScan();
        });
    }
}
