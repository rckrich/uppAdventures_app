using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyPopUpViewModel : ViewModel, IImageDownloaderObject
{
    private const string LIMITED = "LIMITADO";
    private const string ILIMITED = "ILIMITADO";

    [Header("Buy Pop Up Objects")]
    public Image itemImage;
    public Text itemNameText;
    public Text itemAvailableText;
    public Text itemCost;
    public Text userCoinsText;
    public Text exchangeCoinsText;
    public Button buyButton;

    private StoreItem storeItem;
    private bool isScanSceneActive;
    private Texture2D storeItemTexture;

    public override void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list)
    {
        base.Initialize<TViewModel, TPresenter, TInteractor>(list);

        isScanSceneActive = (bool)list[0];

        if (isScanSceneActive)
        {
            AppManager.instance.LoadingViewModelSetActive(true);
            CallPresenter(BuyMethods.GetStoreItem, list[1]);
        }
        else {
            storeItem = (StoreItem)list[1];
            itemImage.sprite = (Sprite)list[2];
            itemNameText.text = storeItem.name;

            if (storeItem.stock_type.Equals(LIMITED)){
                itemAvailableText.text = storeItem.stock.ToString();
            }
            else if (storeItem.stock_type.Equals(ILIMITED)){
                itemAvailableText.text = "Ilimitado";
            }
            
            itemCost.text = "-" + storeItem.price.ToString("#,##0") + " UP Coins";
            //userCoinsText.text = storeItem.price.ToString("#,##0") + " UP Coins";
            int userCoins = ProgressManager.instance.progress.userDataPersistance.UPCoins;
            int exchange = userCoins - storeItem.price;
            userCoinsText.text = userCoins.ToString("#,##0") + " UP Coins";
            exchangeCoinsText.text = exchange.ToString("#,##0") + " UP Coins";
            buyButton.interactable = (exchange >= 0) ? true : false;

            exchangeCoinsText.color = (exchange >= 0) ? new Color32(47, 54, 202, 255) : new Color32(255, 0, 0, 255);
        }
    }

    public override void DisplayOnResult(params object[] list)
    {
        GetStoreItem getStoreItem = (GetStoreItem)list[0];
        storeItem = getStoreItem.item;

        string imageURL = storeItem.media.absolute_url;
        if (!imageURL.Equals(""))
        {
            ImageManager.instance.GetImage(imageURL, this);
        }

        itemNameText.text = storeItem.name;

        if (storeItem.stock_type.Equals(LIMITED))
        {
            itemAvailableText.text = storeItem.stock.ToString();
        }
        else if (storeItem.stock_type.Equals(ILIMITED))
        {
            itemAvailableText.text = "Ilimitado";
        }
        itemCost.text = "-" + storeItem.price.ToString("#,##0") + " UP Coins";
        //userCoinsText.text = storeItem.price.ToString();
        int userCoins = ProgressManager.instance.progress.userDataPersistance.UPCoins;
        int exchange = userCoins - storeItem.price;
        userCoinsText.text = userCoins.ToString("#,##0") + " UP Coins";
        exchangeCoinsText.text = exchange.ToString("#,##0") + " UP Coins";
        buyButton.interactable = (exchange >= 0) ? true : false;

        exchangeCoinsText.color = (exchange >= 0) ? new Color32(47, 54, 202, 255) : new Color32(255, 0, 0, 255);
    }

    public void BuyButtonOnClick()
    {
        CallPresenter(BuyMethods.PostBuy, isScanSceneActive, storeItem);
    }

    public void CancelButtonOnClick()
    {
        if (isScanSceneActive)
        {
            ScanManager.instnace.ResetQRScan();
            this.gameObject.SetActive(false);
        }
        else {
            ScreenManager.instance.BackToPreviousView();
        }
    }

    void IImageDownloaderObject.SetImage(Texture2D texture)
    {
        storeItemTexture = texture;
        itemImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, storeItemTexture.width, storeItemTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
    }

    GameObject IImageDownloaderObject.GetImageContainer()
    {
        return itemImage.gameObject;
    }

    public void OnErrorSetImage(Sprite sprite)
    {
        itemImage.sprite = sprite;
    }
}
