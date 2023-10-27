using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileViewModel : ViewModel, IImageDownloaderObject
{
    [Header("User Object References")]
    public Image avatarImage;
    public Text userNameText;
    public Text userEmailText;
    public Text userCoinsText;

    [Header("Point Text References")]
    public Text culturalPointsText;
    public Text sportPointsText;
    public Text academicPointsText;
    public Text studentIssuesPointsText;
    public Text upMovementPointsText;
    public Text totalPointsText;

    [Header("General")]
    public Transform content;

    private Texture2D userProfileTexture;
    private bool userLoaded = false;


    private void OnEnable()
    {
        content.localPosition = new Vector3(content.localPosition.x, 0f);
    }

    public override void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list)
    {
        base.Initialize<TViewModel, TPresenter, TInteractor>(list);
        SetHeaderAction();
        userLoaded = false;
        CallPresenter();
    }

    public override void DisplayOnResult(params object[] list)
    {
        GetUserEntity getUserEntity = (GetUserEntity)list[0];
        User userEntity = getUserEntity.user;

        ProgressManager.instance.progress.userDataPersistance.id = userEntity.id;
        ProgressManager.instance.progress.userDataPersistance.userName = userEntity.name;
        ProgressManager.instance.progress.userDataPersistance.UPCoins = (userEntity.coins != null) ? (int)userEntity.coins : 0;

        if (userEntity.student_points.Count > 0) {
            ProgressManager.instance.progress.userDataPersistance.puntosAcademicos = userEntity.student_points[(int)PointsTypes.Acedemic].amount;
            ProgressManager.instance.progress.userDataPersistance.puntosAsuntosEstudiantiles = userEntity.student_points[(int)PointsTypes.StudentIssues].amount;
            ProgressManager.instance.progress.userDataPersistance.puntosCulturales = userEntity.student_points[(int)PointsTypes.Cultural].amount;
            ProgressManager.instance.progress.userDataPersistance.puntosDeportivos = userEntity.student_points[(int)PointsTypes.Sports].amount;
            ProgressManager.instance.progress.userDataPersistance.puntosMovimientoUP = userEntity.student_points[(int)PointsTypes.UPMovement].amount;
        }

        if (userEntity.avatar != null)
        {
            ProgressManager.instance.progress.userDataPersistance.avatarThumbnail = userEntity.avatar.media.absolute_url;       
        }
        ProgressManager.instance.Save();

        if (ProgressManager.instance.progress.userDataPersistance.avatarThumbnail.Equals(""))
        {
            avatarImage.sprite = AppManager.instance.nullAvatar;
        }
        else
        {
            /*
            if (userEntity.avatar != null)
            {
                if (!(userEntity.avatar.media.absolute_url.Equals(ProgressManager.instance.progress.userDataPersistance.avatarThumbnail)))
                {
                    ImageManager.instance.GetImage(ProgressManager.instance.progress.userDataPersistance.avatarThumbnail, this);
                }
            }
            */
            ImageManager.instance.GetImage(ProgressManager.instance.progress.userDataPersistance.avatarThumbnail, this);

        }

        userNameText.text = userEntity.name;
        userEmailText.text = userEntity.email;
        userCoinsText.text = (userEntity.coins != null) ? ((int)userEntity.coins).ToString("#,##0") : "0";

        if (userEntity.student_points.Count > 0)
        {
            culturalPointsText.text = userEntity.student_points[(int)PointsTypes.Cultural].amount.ToString("#,##0") + " pts";
            sportPointsText.text = userEntity.student_points[(int)PointsTypes.Sports].amount.ToString("#,##0") + " pts";
            academicPointsText.text = userEntity.student_points[(int)PointsTypes.Acedemic].amount.ToString("#,##0") + " pts";
            studentIssuesPointsText.text = userEntity.student_points[(int)PointsTypes.StudentIssues].amount.ToString("#,##0") + " pts";
            upMovementPointsText.text = userEntity.student_points[(int)PointsTypes.UPMovement].amount.ToString("#,##0") + " pts";
            totalPointsText.text = (
                userEntity.student_points[(int)PointsTypes.Cultural].amount +
                userEntity.student_points[(int)PointsTypes.Sports].amount +
                userEntity.student_points[(int)PointsTypes.Acedemic].amount +
                userEntity.student_points[(int)PointsTypes.StudentIssues].amount +
                userEntity.student_points[(int)PointsTypes.UPMovement].amount
            ).ToString("#,##0") + " pts";
        }
        else {
            culturalPointsText.text = "0 pts";
            sportPointsText.text = "0 pts";
            academicPointsText.text = "0 pts";
            studentIssuesPointsText.text = "0 pts";
            upMovementPointsText.text = "0 pts";
            totalPointsText.text = "0 pts";
        }
        
        userLoaded = true;
    }

public void FAQButtonOnClick() {
        ScreenManager.instance.ChangeView(ViewID.FAQViewModel, true);
        ScreenManager.instance.GetView(ViewID.FAQViewModel).Initialize();
    }

    public void ChangeAvatarButtonOnClick() {
        if (userLoaded) {
            ScreenManager.instance.ChangeView(ViewID.ChangeAvatarViewModel, true);
            ScreenManager.instance.GetView(ViewID.ChangeAvatarViewModel).Initialize<ChangeAvatarViewModel, ChangeAvatarPresenter, ChangeAvatarInteractor>();
        }
    }

    public void SetImage(Texture2D texture)
    {
        userProfileTexture = texture;
        avatarImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, userProfileTexture.width, userProfileTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
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

    public void SetNewAvatar(Sprite newAvatarImage)
    {
        avatarImage.sprite = newAvatarImage;
    }

    public void AssitedEventsButtonOnClick() {
        ScreenManager.instance.ChangeView(ViewID.AssitedEventFeedViewModel, true);
        ScreenManager.instance.GetView(ViewID.AssitedEventFeedViewModel).Initialize<AssistedEventFeedViewModel, AssistedEventFeedPresenter, AssistedEventFeedInteractor>();
    }

    public override void SetHeaderAction() {
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
            ScreenManager.instance.GetView(ViewID.MenuViewModel).GetComponent<MenuViewModel>().Initialize<MenuViewModel, MenuPresenter, MenuInteractor>();
        });
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetMenuSprite();
    }
}
