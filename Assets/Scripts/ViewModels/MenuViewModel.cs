using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuViewModel : ViewModel, IImageDownloaderObject
{
    public Image userProfileImage;
    public Text userNameText;

    public Animator animator;
    public Transform scrollViewContent;
    public GameObject[] btnsActiveViewsVisuals;

    private string userProfileThumbnail;
    private Texture2D userProfileTexture;


    public override void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list)
    {
        base.Initialize<TViewModel, TPresenter, TInteractor>(list);
        if (ProgressManager.instance.progress.userDataPersistance.avatarThumbnail.Equals(""))
        {
            userProfileImage.sprite = AppManager.instance.nullAvatar;
        }
        else {
            if (!(ProgressManager.instance.progress.userDataPersistance.avatarThumbnail.Equals(userProfileThumbnail))) {
                ImageManager.instance.GetImage(ProgressManager.instance.progress.userDataPersistance.avatarThumbnail, this);
            } 
        }
        userProfileThumbnail = ProgressManager.instance.progress.userDataPersistance.avatarThumbnail;
        userNameText.text = ProgressManager.instance.progress.userDataPersistance.userName;
        this.gameObject.SetActive(true);
    }


    private void OnEnable()
    {
        scrollViewContent.localPosition = new Vector3(0f, scrollViewContent.localPosition.y, scrollViewContent.localPosition.z);
    }


    public override void DisplayOnResult(params object[] list)
    {
        //TODO: Quitar el load automatico en el log in en le persistance data
        SceneTransitionManager.instance.startLoadScene("LogInScene");
    }

    public void OnEventsButtonOnClick() {
        /*ScreenManager.instance.ChangeView(ViewID.EventFeedViewModel);
         ScreenManager.instance.GetView(ViewID.EventFeedViewModel).Initialize<EventFeedViewModel, EventFeedPresenter, EventFeedInteractor>();

         for(int i = 0; i < btnsActiveViewsVisuals.Length; i++)
         {
             btnsActiveViewsVisuals[i].SetActive(false);
         }
         btnsActiveViewsVisuals[0].SetActive(true);
        */
        OnOpenViewButtonOnClick(0);
    }

    public void OnProfileButtonOnClick() {
        /*ScreenManager.instance.ChangeView(ViewID.ProfileViewModel);
        ScreenManager.instance.GetView(ViewID.ProfileViewModel).Initialize<ProfileViewModel, ProfilePresenter, ProfileInteractor>();

        for (int i = 0; i < btnsActiveViewsVisuals.Length; i++)
        {
            btnsActiveViewsVisuals[i].SetActive(false);
        }
        btnsActiveViewsVisuals[1].SetActive(true);
        */
        OnOpenViewButtonOnClick(1);
    }

    public void OnStoreButtonOnClick() {
        /*ScreenManager.instance.ChangeView(ViewID.StoreViewModel);
        ScreenManager.instance.GetView(ViewID.StoreViewModel).Initialize<StoreViewModel, StorePresenter, StoreInteractor>();

        for (int i = 0; i < btnsActiveViewsVisuals.Length; i++)
        {
            btnsActiveViewsVisuals[i].SetActive(false);
        }
        btnsActiveViewsVisuals[2].SetActive(true);
        */
        OnOpenViewButtonOnClick(2);
    }

    public void OnScannerButtonOnClick() {
        for (int i = 0; i < btnsActiveViewsVisuals.Length; i++)
        {
            btnsActiveViewsVisuals[i].SetActive(false);
        }
        btnsActiveViewsVisuals[3].SetActive(true);

        SceneTransitionManager.instance.startLoadScene("ScanScene");
    }

    public void OnLogOutButtonOnClick() {
        LogOutMessage();
    }

    public void OnOpenViewButtonOnClick(int value)
    {
        StartCoroutine(OpenViewButtonOnClick_Routine(value));
    }

    IEnumerator OpenViewButtonOnClick_Routine(int value)
    {
        for (int i = 0; i < btnsActiveViewsVisuals.Length; i++)
        {
            btnsActiveViewsVisuals[i].SetActive(false);
        }
        btnsActiveViewsVisuals[value].SetActive(true);

        animator.SetBool("Activate", true);
        yield return new WaitForSeconds(0.22f);
        this.gameObject.SetActive(false);

        switch (value)
        {
            case 0:
                ScreenManager.instance.ChangeView(ViewID.EventFeedViewModel);
                ScreenManager.instance.GetView(ViewID.EventFeedViewModel).Initialize<EventFeedViewModel, EventFeedPresenter, EventFeedInteractor>();
                break;
            case 1:
                ScreenManager.instance.ChangeView(ViewID.ProfileViewModel);
                ScreenManager.instance.GetView(ViewID.ProfileViewModel).Initialize<ProfileViewModel, ProfilePresenter, ProfileInteractor>();
                break;
            case 2:
                ScreenManager.instance.ChangeView(ViewID.StoreViewModel);
                ScreenManager.instance.GetView(ViewID.StoreViewModel).Initialize<StoreViewModel, StorePresenter, StoreInteractor>();
                break;
        }
    }

    public void OnCloseMenuButtonOnClick() {
        //this.gameObject.SetActive(false);
        StartCoroutine(CloseMenuButtonOnClick_Routine());
    }

    IEnumerator CloseMenuButtonOnClick_Routine()
    {
        animator.SetBool("Activate", true);
        yield return new WaitForSeconds(0.22f);
        this.gameObject.SetActive(false);
    }

    public void SetImage(Texture2D texture)
    {
        userProfileTexture = texture;
        userProfileImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, userProfileTexture.width, userProfileTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
    }

    public GameObject GetImageContainer()
    {
        return userProfileImage.gameObject;
    }

    public void OnErrorSetImage(Sprite sprite)
    {
        userProfileImage.sprite = sprite;
    }

    private void LogOutMessage()
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Downwards, "Confirmación",
            "¿Estás seguro que quieres cerrar tu sesión?", "Cerrar Sesión", "Cancelar");
        popUpViewModel.SetPopUpAction(() => { presenter.CallInteractor(); });
    }
}
