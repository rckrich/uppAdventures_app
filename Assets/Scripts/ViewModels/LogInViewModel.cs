using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogInViewModel : ViewModel
{
    //private const string FORGOT_PASSWORD = "http://134.122.27.5/password/reset";
    private const string FORGOT_PASSWORD = "https://upadventures-admin.rckgames.com/password/reset";


    public InputField userInputField;
    public InputField passwordInputField;
    public GameObject splashPanel;
    public float splashTime;

    void Start()
    {
        Initialize<LogInViewModel, LogInPresenter, LogInInteractor>();
        splashPanel.SetActive(true);

        if (!ProgressManager.instance.progress.userDataPersistance.bearer.Equals(""))
        {
            presenter.CallInteractor(LogInMethods.GetUserData, ProgressManager.instance.progress.userDataPersistance.bearer);
            Invoke("ActivateSplashCountDown", 5f);
        }
        else
        {
            StartCoroutine(SplashCountDown());
        }
    }

    public void LogInButtonOnClick() {

        if (!CheckDeviceNetworkReachability())
        {
            ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
            PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
            popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Sin conexión",
                "Necesitas internet para poder cargar la información, asegúrate de tener conexión a internet e intenta de nuevo");
            popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
            return;
        }

        if (!userInputField.text.Equals("") && !passwordInputField.text.Equals("")) {
            presenter.CallInteractor(LogInMethods.PostLogIn, userInputField.text, passwordInputField.text);
        }
    }

    public void ForgotPasswordButtonOnClick() {
        //Application.OpenURL(FORGOT_PASSWORD);

        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Downwards, "¿Olvidaste tu contraseña?",
            "Serás dirigido fuera de la app para cambiar tu contraseña", "Cambiar contraseña", "Cancelar");
        popUpViewModel.SetPopUpAction(() => { Application.OpenURL(FORGOT_PASSWORD); });
        return;
    }

    void ActivateSplashCountDown()
    {
        //Se llama en START, Usamos esta Función por si hay un usuario registriado en progress manager pero el internet es malo y no lo deja acceder
        StartCoroutine(SplashCountDown());
    }

    private IEnumerator SplashCountDown() {
        splashPanel.GetComponent<Animator>().SetTrigger("Activate");

        yield return new WaitForSeconds(splashTime);

        splashPanel.SetActive(false);
    }

}
