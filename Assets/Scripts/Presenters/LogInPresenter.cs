using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInPresenter : Presenter
{
    private const string TEST_POST_LOG_IN = "134.122.27.5/api/auth/login";
    private const string TEST_GET_USER_DATA = "134.122.27.5/api/auth/info";
    private const string POST_LOG_IN = "https://upadventures-admin.rckgames.com/api/auth/login";
    private const string GET_USER_DATA = "https://upadventures-admin.rckgames.com/api/auth/info";

    public override void CallInteractor(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(true);

        if ((LogInMethods)list[0] == LogInMethods.PostLogIn) {
            string username = (string)list[1];
            string password = (string)list[2];
            interactor.PerformSearch((LogInMethods)list[0], POST_LOG_IN, username, password);
        }

        if ((LogInMethods)list[0] == LogInMethods.GetUserData)
        {
            string bearer = (string)list[1];
            interactor.PerformSearch((LogInMethods)list[0], GET_USER_DATA, bearer);
        }

    }

    public override void OnResult(params object[] list)
    {
        if ((LogInMethods)list[0] == LogInMethods.PostLogIn) {
            LogInEntity logInEntity = (LogInEntity)list[1];
            ProgressManager.instance.progress.userDataPersistance.id = logInEntity.user.id;
            ProgressManager.instance.progress.userDataPersistance.userName = logInEntity.user.name;
            ProgressManager.instance.progress.userDataPersistance.bearer = logInEntity.access_token;
            ProgressManager.instance.progress.userDataPersistance.UPCoins = (logInEntity.user.coins != null) ? (int)logInEntity.user.coins : 0;

            if (logInEntity.user.student_points.Count > 0) {
                ProgressManager.instance.progress.userDataPersistance.puntosAcademicos = logInEntity.user.student_points[(int)PointsTypes.Acedemic].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosAsuntosEstudiantiles = logInEntity.user.student_points[(int)PointsTypes.StudentIssues].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosCulturales = logInEntity.user.student_points[(int)PointsTypes.Cultural].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosDeportivos = logInEntity.user.student_points[(int)PointsTypes.Sports].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosMovimientoUP = logInEntity.user.student_points[(int)PointsTypes.UPMovement].amount;
            }

            if (logInEntity.user.avatar != null)
                ProgressManager.instance.progress.userDataPersistance.avatarThumbnail = logInEntity.user.avatar.media.absolute_url;
        }

        if ((LogInMethods)list[0] == LogInMethods.GetUserData)
        {
            GetUserEntity getUserEntity = (GetUserEntity)list[1];
            ProgressManager.instance.progress.userDataPersistance.id = getUserEntity.user.id;
            ProgressManager.instance.progress.userDataPersistance.userName = getUserEntity.user.name;
            ProgressManager.instance.progress.userDataPersistance.UPCoins = (getUserEntity.user.coins != null) ? (int)getUserEntity.user.coins : 0;

            if (getUserEntity.user.student_points.Count > 0)
            {
                ProgressManager.instance.progress.userDataPersistance.puntosAcademicos = getUserEntity.user.student_points[(int)PointsTypes.Acedemic].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosAsuntosEstudiantiles = getUserEntity.user.student_points[(int)PointsTypes.StudentIssues].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosCulturales = getUserEntity.user.student_points[(int)PointsTypes.Cultural].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosDeportivos = getUserEntity.user.student_points[(int)PointsTypes.Sports].amount;
                ProgressManager.instance.progress.userDataPersistance.puntosMovimientoUP = getUserEntity.user.student_points[(int)PointsTypes.UPMovement].amount;
            }

            if (getUserEntity.user.avatar != null)
                ProgressManager.instance.progress.userDataPersistance.avatarThumbnail = getUserEntity.user.avatar.media.absolute_url;
        }

        ProgressManager.instance.Save();
        AppManager.instance.LoadingViewModelSetActive(false);
        SceneTransitionManager.instance.startLoadScene("MainScene");
    }

    public override void OnFailedResult(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(false);

        if ((LogInMethods)list[0] == LogInMethods.PostLogIn)
        {
            ErrorEntity errorEntity = (ErrorEntity)list[1];

            if (errorEntity.message.Equals("api.error.email_not_verified"))
            {
                OnUnauthorizedEmailMessage();
            }
            else
            {
                OnErrorMessage();
            }
        }
        else {
            OnErrorMessage();
        }

        viewModel.DisplayOnFailedResult();
    }

    public override void OnServerError(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        if ((long)list[0] == 401 || (long)list[0] == 422) {
            UnauthorizedDisplay();
            return;
        }

        OnErrorMessage();
        viewModel.DisplayOnServerError();
    }

    public override void OnNetworkError(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(false);
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

    private void OnUnauthorizedEmailMessage()
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Correo no valido",
            "Tu correo aún no ha sido validado, hemos enviado un nuevo correo de verificación. Por favor verifica tu email e intenta de nuevo");
        popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
    }

    private void UnauthorizedDisplay()
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Correo o Contraseña Invalida",
            "El correo o la contraseña son invalidos. Por favor vuelve a intentarlo.");
        popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
    }

}
