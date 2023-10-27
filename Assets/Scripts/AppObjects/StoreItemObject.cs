using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StoreItemObject : AppObject, IImageDownloaderObject, IPointerClickHandler
{
    public Image itemProfileImage;
    public Text itemValueText;
    public Text itemNameText;
    public Color nonInteractibleColor;
    //[HideInInspector]
    public bool buttonInteraction;
    //[HideInInspector]
    public bool itemAlreadyBought;  // Se usa solo para el Avatar
    public bool itemSoldOut;

    private Texture2D itemProfileTexture;
    private StoreItemsViews storeGetMethod;
    private StoreItem storeItem;

    public override void Initialize(params object[] list)
    {
        if (list == null)
        {
            itemProfileImage.sprite = null;
            buttonInteraction = false;
            this.gameObject.SetActive(false);

            itemValueText.text = "";                    
            itemNameText.text = "";                     
            itemProfileImage.color = Color.white;       
            itemAlreadyBought = false;
            itemSoldOut = false;
            return;
        }

        itemProfileImage.sprite = null;
        buttonInteraction = false;
        storeGetMethod = (StoreItemsViews)list[1];
        storeItem = (StoreItem)list[0];
        string imageURL = storeItem.media.absolute_url;
        if (!imageURL.Equals(""))
        {
            ImageManager.instance.GetImage(imageURL, this);
        }
        itemValueText.text = storeItem.price.ToString();
        itemNameText.text = storeItem.name;
        buttonInteraction = true;
        this.gameObject.SetActive(true);
    }

    public void SetImage(Texture2D texture)
    {
        itemProfileTexture = texture;
        itemProfileImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, itemProfileTexture.width, itemProfileTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
    }

    public GameObject GetImageContainer()
    {
        return this.gameObject;
    }

    public void OnErrorSetImage(Sprite sprite)
    {
        itemProfileImage.sprite = sprite;
    }

    public void SetItemBought(bool value)   //No hay que bloquear nunca el boton
    {
        itemAlreadyBought = value;
    }

    public void SetItemSoldOut(bool value)
    {
        itemSoldOut = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if (buttonInteraction && !itemAlreadyBought) //No hay que bloquear nunca el boton
        //{
            ScreenManager.instance.ChangeView(ViewID.StoreItemDetailViewModel, true);
            ScreenManager.instance.GetView(ViewID.StoreItemDetailViewModel).Initialize(storeItem, itemProfileImage.sprite, storeGetMethod, itemAlreadyBought, itemSoldOut);
        //}
    }
}
