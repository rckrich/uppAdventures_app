using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAvatarViewModel : ViewModel, IImageDownloaderObject
{
    private const string CHANGE_AVATAR_TEXT = "Usar este Avatar";
    private const string NOT_CHANGE_AVATAR_TEXT = "No tienes Avatares";
    private const int MIN_PAGE = 1;

    [Header("Change Avatar Text References")]
    public Image avatarImage;
    public Text pageText;
    public Text buttonText;
    public Button leftPageButton;
    public Button rightPageButton;
    public Button acceptButton;
    public List<AvatarProfileButton> avatarProfileButtons;

    private List<Avatar> avatarList = new List<Avatar>();
    private int totalPages = 0;
    private int currentPage = 1;
    private int avatarCounter = 0;
    private Avatar selectedAvatar;
    private Texture2D avatarProfileTexture;

    public override void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list)
    {
        base.Initialize<TViewModel, TPresenter, TInteractor>(list);
        ImageManager.instance.GetImage(ProgressManager.instance.progress.userDataPersistance.avatarThumbnail, this);
        avatarCounter = 0;
        avatarList.Clear();
        ConfigAcceptButton();
        ClearAvatarProfileButtons();
        CheckPageButtons();
        CallPresenter(ChangeAvatarMethods.GetUserAvatars);
    }

    public override void DisplayOnResult(params object[] list)
    {
        if ((ChangeAvatarMethods)list[0] == ChangeAvatarMethods.GetUserAvatars)
        {
            foreach (Avatar avatar in ((GetAvatars)list[1]).avatars)
            {
                avatarList.Add((Avatar)avatar);
            }

            totalPages = Mathf.CeilToInt((float)avatarList.Count / (float)avatarProfileButtons.Count);
            pageText.text = currentPage + "/" + totalPages;

            if (currentPage == totalPages)
            {
                leftPageButton.interactable = false;
            }

            InitializeAvatarProfileButtons();
            SetPageButtonConfig();
            ConfigAcceptButton();
            AppManager.instance.LoadingViewModelSetActive(false);

        }

        if ((ChangeAvatarMethods)list[0] == ChangeAvatarMethods.PostChangeAvatar)
        {
            AppManager.instance.LoadingViewModelSetActive(false);
            ScreenManager.instance.BackToPreviousView();
            ScreenManager.instance.GetView(ViewID.ProfileViewModel).GetComponent<ProfileViewModel>().SetNewAvatar(avatarImage.sprite);
        }            
    }

    public override void DisplayOnFailedResult(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(false);
    }

    public override void DisplayOnNetworkError(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(false);
    }

    public override void DisplayOnServerError(params object[] list)
    {
        AppManager.instance.LoadingViewModelSetActive(false);
    }

    public void LeftButtonOnClick() {

        if (currentPage == MIN_PAGE) {
            return;
        }

        currentPage--;
        SetPageButtonConfig();
        ChangePages();
        //TODO: Do animatinon and change system for profile avatar pics
    }

    public void RightButtonOnClick() {

        if (currentPage == totalPages)
        {
            return;
        }

        currentPage++;
        SetPageButtonConfig();
        ChangePages();
        //TODO: Do animatinon and change system for profile avatar pics
    }

    public void ChangePages() {
        pageText.text = currentPage + "/" + totalPages;
        avatarCounter = (currentPage - 1) * avatarProfileButtons.Count;
        InitializeAvatarProfileButtons();
    }

    private void InitializeAvatarProfileButtons() {
        foreach (AvatarProfileButton avatarProfileButton in avatarProfileButtons)
        {
            if (avatarList.Count > avatarCounter)
            {
                avatarProfileButton.Initialize(avatarList[avatarCounter]);
                avatarCounter++;
            }
            else
            {
                avatarProfileButton.Initialize(null);
            }
        }
    }

    private void ClearAvatarProfileButtons() {
        foreach (AvatarProfileButton avatarProfileButton in avatarProfileButtons)
        {
            avatarProfileButton.Initialize(null);
        }
    }

    private void SetPageButtonConfig() {

        if (currentPage == MIN_PAGE)
        {
            leftPageButton.interactable = false;
        }
        else {
            leftPageButton.interactable = true;
        }

        if (currentPage == totalPages)
        {
            rightPageButton.interactable = false;
        }
        else {
            rightPageButton.interactable = true;
        }

        CheckPageButtons();
    }

    public void SetImage(Texture2D texture)
    {
        avatarProfileTexture = texture;
        avatarImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, avatarProfileTexture.width, avatarProfileTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
    }

    public GameObject GetImageContainer()
    {
        return avatarImage.gameObject;
    }

    public void OnErrorSetImage(Sprite sprite)
    {
        avatarImage.sprite = sprite;
    }

    public void ExitButtonOnClick() {
        if (selectedAvatar != null)
        {
            AppManager.instance.LoadingViewModelSetActive(true);
            CallPresenter(ChangeAvatarMethods.PostChangeAvatar, selectedAvatar);
        }
        else
        {
            ScreenManager.instance.BackToPreviousView();
        }
    }

    public void SetPreview(Avatar _selectedAvatar, Sprite _selectedAvatarSprite) {
        selectedAvatar = _selectedAvatar;
        avatarImage.sprite = _selectedAvatarSprite;
    }

    private void ConfigAcceptButton() {
        if (avatarList.Count <= 0)
        {
            buttonText.text = NOT_CHANGE_AVATAR_TEXT;
            acceptButton.interactable = false;
        }
        else {
            buttonText.text = CHANGE_AVATAR_TEXT;
            acceptButton.interactable = true;
        }
    }

    private void CheckPageButtons() {
        if (avatarList.Count <= 0)
        {
            leftPageButton.interactable = false;
            rightPageButton.interactable = false;
        }
    }

}
