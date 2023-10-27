using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorePresenter : Presenter
{
    private const string TEST_GET_STORE_ITEMS = "134.122.27.5/api/store/available/";
    private const string TEST_GET_USER_AVATARS = "134.122.27.5/api/auth/avatars";
    private const string GET_STORE_ITEMS = "https://upadventures-admin.rckgames.com/api/store/available/";
    private const string GET_USER_AVATARS = "https://upadventures-admin.rckgames.com/api/auth/avatars";

    public override void CallInteractor(params object[] list)
    {
        if ((StoreGetMethods)list[0] == StoreGetMethods.GetStoreItems)
            interactor.PerformSearch(StoreGetMethods.GetStoreItems, GET_STORE_ITEMS);

        if ((StoreGetMethods)list[0] == StoreGetMethods.GetUserAvatars)
            interactor.PerformSearch(StoreGetMethods.GetUserAvatars, GET_USER_AVATARS);
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
}
