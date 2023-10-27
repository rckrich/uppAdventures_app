using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvatarPresenter : Presenter
{
    private const string TEST_GET_USER_AVATARS = "134.122.27.5/api/auth/avatars";
    private const string TEST_POST_CHANGE_AVATAR = "134.122.27.5/api/auth/avatar";
    private const string GET_USER_AVATARS = "https://upadventures-admin.rckgames.com/api/auth/avatars";
    private const string POST_CHANGE_AVATAR = "https://upadventures-admin.rckgames.com/api/auth/avatar";

    private Avatar selectedAvatar;

    private void Start()
    {
        AddEventListener<ChangeAvatarOnClickEvent>(ChangeAvatarOnClickEventListener);
    }

    public override void CallInteractor(params object[] list)
    {
        ChangeAvatarMethods changeAvatarMethods = (ChangeAvatarMethods)list[0];

        if ((ChangeAvatarMethods)list[0] == ChangeAvatarMethods.GetUserAvatars)
        {
            interactor.PerformSearch(ChangeAvatarMethods.GetUserAvatars, GET_USER_AVATARS);
        }

        if ((ChangeAvatarMethods)list[0] == ChangeAvatarMethods.PostChangeAvatar)
        {
            selectedAvatar = (Avatar)list[1];
            interactor.PerformSearch(ChangeAvatarMethods.PostChangeAvatar, POST_CHANGE_AVATAR, selectedAvatar.id.ToString());
        }

    }

    public override void OnResult(params object[] list)
    {
        if ((ChangeAvatarMethods)list[0] == ChangeAvatarMethods.GetUserAvatars)
        {
            viewModel.DisplayOnResult(list);
        }

        if ((ChangeAvatarMethods)list[0] == ChangeAvatarMethods.PostChangeAvatar)
        {
            ProgressManager.instance.progress.userDataPersistance.avatarThumbnail = selectedAvatar.media.absolute_url;
            ProgressManager.instance.Save();
            viewModel.DisplayOnResult(list[0], selectedAvatar.media.absolute_url);
        }
            
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

    private void ChangeAvatarOnClickEventListener(ChangeAvatarOnClickEvent eventData) {
        //AppManager.instance.LoadingViewModelSetActive(true);
        //CallInteractor(ChangeAvatarMethods.PostChangeAvatar, eventData.GetAvatar());
        ((ChangeAvatarViewModel)viewModel).SetPreview(eventData.GetAvatar(), eventData.GetSprite());
    }

}
