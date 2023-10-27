using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemDetailViewModel : ViewModel
{
    private const string LIMITED = "LIMITADO";
    private const string ILIMITED = "ILIMITADO";

    [Header("Store Item Detail Objects")]
    public Image itemImage;
    public Text itemNameText;
    public Text itemAvailableText;
    public Text itemDescriptionText;
    public Text coinsText;
    public GameObject activeStateButton;
    public GameObject acquiredStateButton;
    public GameObject soldOutStateButton;

    private StoreItem storeItem;
    private StoreItemsViews storeGetMethods;

    private bool itemAlreadyBought;  // Se usa solo para el Avatar
    public bool itemSoldOut;


    public override void Initialize(params object[] list)
    {
        //SetHeaderAction();
        storeItem = (StoreItem)list[0];
        itemImage.sprite = (Sprite)list[1];
        storeGetMethods = (StoreItemsViews)list[2];
        itemNameText.text = storeItem.name;
        itemAlreadyBought = (bool)list[3];  //**
        itemSoldOut = (bool)list[4];  //**

        if (storeItem.stock_type.Equals(LIMITED))
        {
            itemAvailableText.text = storeItem.stock.ToString();
        }
        else if (storeItem.stock_type.Equals(ILIMITED))
        {
            itemAvailableText.text = "Ilimitado";
        }

        itemDescriptionText.text = storeItem.description;
        coinsText.text = storeItem.price.ToString("#,##0") + " UP Coins";


        if (!itemAlreadyBought) //**
        {
            activeStateButton.SetActive(true);
            soldOutStateButton.SetActive(false);
            acquiredStateButton.SetActive(false);
        }
        else if (itemAlreadyBought)
        {
            activeStateButton.SetActive(false);
            soldOutStateButton.SetActive(false);
            acquiredStateButton.SetActive(true);
        }

        if (itemSoldOut) //**
        {
            activeStateButton.SetActive(false);
            soldOutStateButton.SetActive(true);
            acquiredStateButton.SetActive(false);
        }
    }

    public void BuyButtonOnClick()
    {
        switch (storeGetMethods)
        {
            case StoreItemsViews.GetProducts:
                ScreenManager.instance.ChangeView(ViewID.ScanPopUpViewModel, true);
                ScreenManager.instance.GetView(ViewID.ScanPopUpViewModel).Initialize();
                break;
            case StoreItemsViews.GetPromos:
                ScreenManager.instance.ChangeView(ViewID.ScanPopUpViewModel, true);
                ScreenManager.instance.GetView(ViewID.ScanPopUpViewModel).Initialize();
                break;
            case StoreItemsViews.GetAvatars:
                ScreenManager.instance.ChangeView(ViewID.BuyPopUpViewModel, true);
                ScreenManager.instance.GetView(ViewID.BuyPopUpViewModel).Initialize<BuyPopUpViewModel, BuyPopUpPresenter, BuyPopUpInteractor>(false, storeItem, itemImage.sprite);
                break;
        }
    }

    public void AcquiredButtonOnClick()
    {
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Avatar Conseguido",
            "Ya conseguiste anteriormente este Avatar. Los Avatares solo se pueden obtener una vez, puedes seleccionarlo en la sección de Perfil.");
        popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
    }

    public void SoldOutButtonOnClick()
    {
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Agotado",
            "Por el momento este producto esta agotado. Regresa después o pregunta en administración cuándo volverá a estar disponible para comprarlo.");
        popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
    }

    public void CancelButtonOnClick()
    {
        ScreenManager.instance.BackToPreviousView();
    }

    /*public override void SetHeaderAction()
    {
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
            ScreenManager.instance.BackToPreviousView();
            ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
                ScreenManager.instance.GetView(ViewID.StoreViewModel).GetComponent<StoreViewModel>().OpenStoreItemObject();
            });
        });
    }*/
}
