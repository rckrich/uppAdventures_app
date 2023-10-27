using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPresenter : Presenter
{

    private const string GET_NEWSFEED_COVER = "https://telesurplus.com.mx/api/cover/";

    public override void CallInteractor(params object[] list)
    {
        interactor.PerformSearch(GET_NEWSFEED_COVER);
    }

    public override void OnResult(params object[] list)
    {
        Debug.Log(list[0]);
        //viewModel.DisplayOnResult(list);
    }

}
