using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPresenter : Presenter
{
    private const string TEST_LOG_OUT = "134.122.27.5/api/auth/logout";
    private const string LOG_OUT = "https://upadventures-admin.rckgames.com/api/auth/logout";
    public override void CallInteractor(params object[] list)
    {
        interactor.PerformSearch(LOG_OUT);
    }

    public override void OnResult(params object[] list)
    {
        ProgressManager.instance.progress.userDataPersistance.id = 0;
        ProgressManager.instance.progress.userDataPersistance.userName = "";
        ProgressManager.instance.progress.userDataPersistance.avatarThumbnail = "";
        ProgressManager.instance.progress.userDataPersistance.bearer = "";
        ProgressManager.instance.progress.userDataPersistance.UPCoins = 0;
        ProgressManager.instance.progress.userDataPersistance.puntosCulturales = 0;
        ProgressManager.instance.progress.userDataPersistance.puntosDeportivos = 0;
        ProgressManager.instance.progress.userDataPersistance.puntosAcademicos = 0;
        ProgressManager.instance.progress.userDataPersistance.puntosAsuntosEstudiantiles = 0;
        ProgressManager.instance.progress.userDataPersistance.puntosMovimientoUP = 0;
        ProgressManager.instance.Save();
        SceneTransitionManager.instance.startLoadScene("LogInScene");
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

}
