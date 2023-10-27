using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuccessfulBuyPopUpViewModel : ViewModel, IImageDownloaderObject
{
    public Text itemNameText;
    public Image itemImage;
    public Transform infoContainer;

    private bool isScanSceneActive = false;
    private Texture2D storeItemTexture;

    private void OnEnable()
    {
        StartCoroutine(InfoContainer_LayoutRebuild());
    }

    IEnumerator InfoContainer_LayoutRebuild()
    {
        yield return new WaitForSeconds(0.1f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)infoContainer.transform);
    }

    public override void Initialize(params object[] list)
    {
        itemNameText.text = (string)list[0];
        string imageURL = (string)list[1];
        if (!imageURL.Equals(""))
        {
            ImageManager.instance.GetImage(imageURL, this);
        }
        isScanSceneActive = (bool)list[2];
    }

    public void AcceptButtonOnClick() {
        if (isScanSceneActive)
        {
            ScanManager.instnace.ResetQRScan();
            this.gameObject.SetActive(false);
        }
        else {
            ScreenManager.instance.ChangeView(ViewID.StoreViewModel);
            ScreenManager.instance.GetView(ViewID.StoreViewModel).Initialize<StoreViewModel, StorePresenter, StoreInteractor>();
        }
    }

    public void SetImage(Texture2D texture)
    {
        storeItemTexture = texture;
        itemImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, storeItemTexture.width, storeItemTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
    }

    public GameObject GetImageContainer()
    {
        return itemImage.gameObject;
    }

    public void OnErrorSetImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }
}
