using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilePresenter : Presenter
{
    private const string TEST_GET_USER_DATA = "134.122.27.5/api/auth/info";
    private const string GET_USER_DATA = "https://upadventures-admin.rckgames.com/api/auth/info";

    public override void CallInteractor(params object[] list)
    {
        string bearer = ProgressManager.instance.progress.userDataPersistance.bearer;
        interactor.PerformSearch(GET_USER_DATA, bearer);
    }
}
