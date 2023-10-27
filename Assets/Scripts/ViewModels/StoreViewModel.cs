using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreViewModel : ViewModel
{
    private const int MIN_PAGE = 1;

    [Header("Store Item Object Array")]
    public Text pageText;
    public Button leftPageButton;
    public Button rightPageButton;
    public List<StoreItemObject> storeItemButtons;
    [Header("Store Object References")]
    public GameObject storeMainHolderObject;
    public GameObject storeItemHolderObject;
    [Header("User Object References")]
    public Text coinsText;
    [Header("General")]
    public Text proudctTitleText;
    public Animator animator;

    private StoreItemsViews storeGetMethods;
    private List<Producto> products = new List<Producto>();
    private List<Promocion> promotions = new List<Promocion>();
    private List<Avatar> avatars = new List<Avatar>();
    private List<Avatar> boughtAvatars = new List<Avatar>();
    private int totalPages = 0;
    private int currentPage = 1;
    private int storeItemsCounter = 0;

    public override void Initialize<TViewModel, TPresenter, TInteractor>(params object[] list)
    {
        base.Initialize<TViewModel, TPresenter, TInteractor>(list);
        SetHeaderAction();
        AppManager.instance.LoadingViewModelSetActive(false);
        coinsText.text = AppManager.instance.GetUserUPCoins().ToString();
        storeGetMethods = StoreItemsViews.GetProducts;
        if (currentPage == totalPages)
        {
            leftPageButton.interactable = false;
        }
        storeItemsCounter = 0;
        OpenStoreMainObject(true);
        AppManager.instance.LoadingViewModelSetActive(true);
        products.Clear();
        promotions.Clear();
        avatars.Clear();
        boughtAvatars.Clear();
        ClearStoreItemButtons();
        CheckPageButtons();
        CallPresenter(StoreGetMethods.GetStoreItems);
    }

    public override void CallPresenter(params object[] list)
    {
        base.CallPresenter(list);
    }

    public override void DisplayOnResult(params object[] list)
    {
        if ((StoreGetMethods)list[0] == StoreGetMethods.GetStoreItems) {
            foreach (Producto product in ((GetStoreItems)list[1]).productos)
            {
                products.Add(product);
            }

            foreach (Promocion promo in ((GetStoreItems)list[1]).promociones)
            {
                promotions.Add(promo);
            }

            foreach (Avatar avatar in ((GetStoreItems)list[1]).avatares)
            {
                avatars.Add(avatar);
            }

            CallPresenter(StoreGetMethods.GetUserAvatars);
        }

        if ((StoreGetMethods)list[0] == StoreGetMethods.GetUserAvatars)
        {
            if (((GetAvatars)list[1]).avatars.Count > 0)
            {
                foreach (Avatar avatar in ((GetAvatars)list[1]).avatars)
                {
                    boughtAvatars.Add(avatar);
                }
            }
            else {
                boughtAvatars.Clear();
            }

            AppManager.instance.LoadingViewModelSetActive(false);
        }
    }

    public void LeftButtonOnClick()
    {
        if (currentPage == MIN_PAGE)
        {
            return;
        }

        currentPage--;
        SetPageButtonConfig();
        ChangePages();
        //TODO: Do animatinon and change system for profile avatar pics
    }

    public void RightButtonOnClick()
    {
        if (currentPage == totalPages)
        {
            return;
        }

        currentPage++;
        SetPageButtonConfig();
        ChangePages();
        //TODO: Do animatinon and change system for profile avatar pics
    }

    public void ChangePages()
    {
        pageText.text = currentPage + "/" + totalPages;
        storeItemsCounter = (currentPage - 1) * storeItemButtons.Count;
        InitializeStoreItemButtons();
    }

    private void SetStoreItems()
    {
        if (storeGetMethods == StoreItemsViews.GetProducts)
        {
            totalPages = Mathf.CeilToInt((float)products.Count / (float)storeItemButtons.Count);
        }

        if (storeGetMethods == StoreItemsViews.GetPromos)
        {
            totalPages = Mathf.CeilToInt((float)promotions.Count / (float)storeItemButtons.Count);
        }

        if (storeGetMethods == StoreItemsViews.GetAvatars)
        {
            totalPages = Mathf.CeilToInt((float)avatars.Count / (float)storeItemButtons.Count);
        }


        pageText.text = currentPage + "/" + totalPages;

        if (currentPage == totalPages)
        {
            leftPageButton.interactable = false;
        }

        ClearStoreItemButtons();
        InitializeStoreItemButtons();
        SetPageButtonConfig();
    }

    private void InitializeStoreItemButtons()
    {
        foreach (StoreItemObject storeItemObject in storeItemButtons)
        {

            if (storeGetMethods == StoreItemsViews.GetProducts) {
                if (products.Count > storeItemsCounter)
                {
                    storeItemObject.Initialize(products[storeItemsCounter], storeGetMethods);
                    //storeItemObject.SetItemBought(ItemAlreadyBought(products[storeItemsCounter]));
                    storeItemObject.SetItemSoldOut(ItemSoldOut(products[storeItemsCounter]));
                    storeItemsCounter++;
                }
                else
                {
                    storeItemObject.Initialize(null);
                }
            }

            if (storeGetMethods == StoreItemsViews.GetPromos)
            {
                if (promotions.Count > storeItemsCounter)
                {
                    storeItemObject.Initialize(promotions[storeItemsCounter], storeGetMethods);
                    //storeItemObject.SetItemBought(ItemAlreadyBought(promotions[storeItemsCounter]));
                    storeItemObject.SetItemSoldOut(ItemSoldOut(products[storeItemsCounter]));
                    storeItemsCounter++;
                }
                else
                {
                    storeItemObject.Initialize(null);
                }
            }

            if (storeGetMethods == StoreItemsViews.GetAvatars)
            {
                if (avatars.Count > storeItemsCounter)
                {
                    storeItemObject.Initialize(avatars[storeItemsCounter], storeGetMethods);
                    storeItemObject.SetItemBought(ItemAlreadyBought(avatars[storeItemsCounter]));
                    storeItemsCounter++;
                }
                else
                {
                    storeItemObject.Initialize(null);
                }
            }

            
        }
    }

    private void ClearStoreItemButtons() {
        foreach (StoreItemObject storeItemObject in storeItemButtons)
        {
            storeItemObject.Initialize(null);
        }
    }

    public void GetProductsButtonOnClick()
    {
        proudctTitleText.text = "Productos";
        storeGetMethods = StoreItemsViews.GetProducts;
        OpenStoreItemObject();
        ResetStoreProductObject();
        SetStoreItems();
    }

    public void GetPromosButtonOnClick()
    {
        proudctTitleText.text = "Promociones";
        storeGetMethods = StoreItemsViews.GetPromos;
        OpenStoreItemObject();
        ResetStoreProductObject();
        SetStoreItems();
    }

    public void GetAvatarsButtonOnClick()
    {
        proudctTitleText.text = "Avatares";
        storeGetMethods = StoreItemsViews.GetAvatars;
        //avatars.Clear(); // Es solo para pruebas
        OpenStoreItemObject();
        ResetStoreProductObject();
        SetStoreItems();
    }

    private bool ItemAlreadyBought(StoreItem item) {
        return (item.stock > 0) ? false : true;
    }

    private bool ItemAlreadyBought(Avatar avatar)
    {
        foreach (Avatar boughtAvatar in boughtAvatars) {
            if (boughtAvatar.id == avatar.id) {
                return true;
            }
        }
        return false;
    }

    private bool ItemSoldOut(StoreItem item)
    {
        return (item.stock <= 0) ? true : false;
    }

    public void OpenStoreItemObject() {
        storeItemHolderObject.SetActive(true);
        //storeMainHolderObject.SetActive(false);
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
            StartCoroutine(CloseRoutine_StoreItemObject());
            OpenStoreMainObject(false);
        });
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetBackSprite();
    }

    private void OpenStoreMainObject(bool resetStore)
    {
        if (resetStore) {
            storeItemHolderObject.SetActive(false);
        }
        storeMainHolderObject.SetActive(true);
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
            ScreenManager.instance.GetView(ViewID.MenuViewModel).GetComponent<MenuViewModel>().Initialize<MenuViewModel, MenuPresenter, MenuInteractor>();
        });
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetMenuSprite();
    }

    IEnumerator CloseRoutine_StoreItemObject()
    {
        animator.SetTrigger("Activate");

        yield return new WaitForSeconds(0.35f);

        storeItemHolderObject.SetActive(false);
        //storeMainHolderObject.SetActive(true);
    }

    private void ResetStoreProductObject() {
        totalPages = 0;
        currentPage = 1;
        storeItemsCounter = 0;
    }

    private void SetPageButtonConfig()
    {

        if (currentPage == MIN_PAGE)
        {
            leftPageButton.interactable = false;
        }
        else
        {
            leftPageButton.interactable = true;
        }

        if (currentPage == totalPages)
        {
            rightPageButton.interactable = false;
        }
        else
        {
            rightPageButton.interactable = true;
        }

        CheckPageButtons();
    }

    private void NoInternetPopUp()
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Sin conexión",
            "Necesitas internet para poder cargar la información, asegúrate de tener conexión a internet e intenta de nuevo");
        popUpViewModel.SetPopUpAction(() => { ScreenManager.instance.BackToPreviousView(); });
    }

    public void UPCardButtonOnClick()
    {
        if (!CheckDeviceNetworkReachability())
        {
            NoInternetPopUp();
            return;
        }

        ScreenManager.instance.ChangeView(ViewID.UPCardViewModel, true);
        UPCardViewModel upCardViewModel = (UPCardViewModel)ScreenManager.instance.GetView(ViewID.UPCardViewModel);
        upCardViewModel.Initialize(AppManager.instance.GetUserUPCoins().ToString() + " UP Coins");
    }

    public override void SetHeaderAction()
    {
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetActionButtonAction(() => {
            ScreenManager.instance.GetView(ViewID.MenuViewModel).GetComponent<MenuViewModel>().Initialize<MenuViewModel, MenuPresenter, MenuInteractor>();
        });
        ScreenManager.instance.GetHeaderView().GetComponent<HeaderViewModel>().SetMenuSprite();
    }

    private void CheckPageButtons()
    {

        if (storeGetMethods == StoreItemsViews.GetProducts) {
            if (products.Count <= 0)
            {
                leftPageButton.interactable = false;
                rightPageButton.interactable = false;
            }
        }

        if (storeGetMethods == StoreItemsViews.GetPromos)
        {
            if (promotions.Count <= 0)
            {
                leftPageButton.interactable = false;
                rightPageButton.interactable = false;
            }
        }

        if (storeGetMethods == StoreItemsViews.GetAvatars)
        {
            if (avatars.Count <= 0)
            {
                leftPageButton.interactable = false;
                rightPageButton.interactable = false;
            }
        }


    }

}
