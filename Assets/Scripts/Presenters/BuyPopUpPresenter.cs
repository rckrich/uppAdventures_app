using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPopUpPresenter : Presenter
{
    private const string TEST_GET_STORE_ITEM = "134.122.27.5/api/store/";
    private const string TEST_BUY_ITEMS = "134.122.27.5/api/store/purchase/id";
    private const string TEST_GET_USER_DATA = "134.122.27.5/api/auth/info";
    private const string GET_STORE_ITEM = "https://upadventures-admin.rckgames.com/api/store/";
    private const string BUY_ITEMS = "https://upadventures-admin.rckgames.com/api/store/purchase/id";
    private const string GET_USER_DATA = "https://upadventures-admin.rckgames.com/api/auth/info";

    private bool isScanSceneActive = false;
    private StoreItem storeItem;

    public override void CallInteractor(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(true);

        isScanSceneActive = false;
        BuyMethods buyMethod = (BuyMethods)list[0];

        if (BuyMethods.GetStoreItem == buyMethod)
        {
            interactor.PerformSearch(buyMethod, GET_STORE_ITEM, list[1]);
        }

        if (BuyMethods.PostBuy == buyMethod)
        {
            isScanSceneActive = (bool)list[1];
            storeItem = (StoreItem)list[2];
            interactor.PerformSearch(buyMethod, BUY_ITEMS, storeItem.id.ToString());
        }

        if (BuyMethods.GetUserData == buyMethod)
        {
            isScanSceneActive = (bool)list[1];
            interactor.PerformSearch(buyMethod, GET_USER_DATA);
        }
    }

    public override void OnResult(params object[] list)
    {
        BuyMethods buyMethod = (BuyMethods)list[0];

        if (BuyMethods.GetStoreItem == buyMethod)
        {
            viewModel.DisplayOnResult(list[1]);
            AppManager.instance.LoadingViewModelSetActive(false);
            return;
        }

        if (BuyMethods.PostBuy == buyMethod)
        {
            buyMethod = BuyMethods.GetUserData;
            CallInteractor(buyMethod, isScanSceneActive);
            return;
        }

        if (BuyMethods.GetUserData == buyMethod) {
            GetUserEntity getUserEntity = (GetUserEntity)list[1];
            ProgressManager.instance.progress.userDataPersistance.id = getUserEntity.user.id;
            ProgressManager.instance.progress.userDataPersistance.userName = getUserEntity.user.name;
            ProgressManager.instance.progress.userDataPersistance.UPCoins = (getUserEntity.user.coins != null) ? (int)getUserEntity.user.coins : 0;

            if (getUserEntity.user.student_points.Count > 0) {

                ProgressManager.instance.progress.userDataPersistance.puntosAcademicos = getUserEntity.user.student_points[(int)PointsTypes.Acedemic].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosAsuntosEstudiantiles = getUserEntity.user.student_points[(int)PointsTypes.StudentIssues].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosCulturales = getUserEntity.user.student_points[(int)PointsTypes.Cultural].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosDeportivos = getUserEntity.user.student_points[(int)PointsTypes.Sports].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosMovimientoUP = getUserEntity.user.student_points[(int)PointsTypes.UPMovement].amount;
            }

            if (getUserEntity.user.avatar != null)
                ProgressManager.instance.progress.userDataPersistance.avatarThumbnail = getUserEntity.user.avatar.media.absolute_url;

            ScreenManager.instance.ChangeView(ViewID.SuccessfulBuyPopUpViewModel, false);
            ScreenManager.instance.GetView(ViewID.SuccessfulBuyPopUpViewModel).GetComponent<SuccessfulBuyPopUpViewModel>().Initialize(
                storeItem.name,
                storeItem.media.absolute_url,
                isScanSceneActive
            );
            AppManager.instance.LoadingViewModelSetActive(false);
        }
    }

    public override void OnFailedResult(params object[] list)
    {
        BuyMethods buyMethod = (BuyMethods)list[0];

        if (BuyMethods.GetStoreItem == buyMethod)
        {
            OnErrorMessage();
            viewModel.SetActive(false);
            AppManager.instance.LoadingViewModelSetActive(false);
        }

        if (BuyMethods.PostBuy == buyMethod)
        {
            ErrorEntity errorEntity = (ErrorEntity)list[1];

            if (errorEntity.message.Equals("api.error.no_stock"))
            {
                ScreenManager.instance.ChangeView(ViewID.UnsuccessfulBuyPopUpViewModel, false);
                ScreenManager.instance.GetView(ViewID.UnsuccessfulBuyPopUpViewModel).GetComponent<UnsuccessfulBuyPopUpViewModel>().Initialize(isScanSceneActive, errorEntity.message);
            }
            else if (errorEntity.message.Equals("api.error.not_enough_coins"))
            {
                ScreenManager.instance.ChangeView(ViewID.UnsuccessfulBuyPopUpViewModel, false);
                ScreenManager.instance.GetView(ViewID.UnsuccessfulBuyPopUpViewModel).GetComponent<UnsuccessfulBuyPopUpViewModel>().Initialize(isScanSceneActive, errorEntity.message);
            }
            else
            {
                ScreenManager.instance.ChangeView(ViewID.UnsuccessfulBuyPopUpViewModel, false);
                ScreenManager.instance.GetView(ViewID.UnsuccessfulBuyPopUpViewModel).GetComponent<UnsuccessfulBuyPopUpViewModel>().Initialize(isScanSceneActive, "");
            }

            AppManager.instance.LoadingViewModelSetActive(false);
        }

        if (BuyMethods.GetUserData == buyMethod)
        {
            OnErrorMessage();
            viewModel.SetActive(false);
            AppManager.instance.LoadingViewModelSetActive(false);
        }
    }

    public override void OnServerError(params object[] list)
    {
        OnErrorMessage();

        if (isScanSceneActive)
        {
            viewModel.SetActive(false);
            AppManager.instance.LoadingViewModelSetActive(false);
        }
        else {
            viewModel.DisplayOnServerError();
        }
        
    }

    public override void OnNetworkError(params object[] list)
    {
        OnErrorMessage();

        if (isScanSceneActive)
        {
            viewModel.SetActive(false);
            AppManager.instance.LoadingViewModelSetActive(false);
        }
        else
        {
            viewModel.DisplayOnServerError();
        }
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
