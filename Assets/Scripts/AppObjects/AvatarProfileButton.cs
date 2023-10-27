using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AvatarProfileButton : AppObject, IImageDownloaderObject, IPointerClickHandler
{
    public Image avatarProfileImage;
    [HideInInspector]
    public bool buttonInteraction;

    private Avatar avatarEntity;
    private Texture2D avatarProfileTexture;
    private bool isImageLoaded = false;

    public override void Initialize(params object[] list)
    {
        if (list == null) {
            avatarProfileImage.sprite = null;
            buttonInteraction = false;
            return;
        }

        buttonInteraction = false;
        avatarEntity = (Avatar)list[0];
        string imageURL = avatarEntity.media.absolute_url;
        if (!imageURL.Equals(""))
        {
            isImageLoaded = false;
            ImageManager.instance.GetImage(imageURL, this);
        }
        
    }

    public void SetImage(Texture2D texture)
    {
        avatarProfileTexture = texture;
        avatarProfileImage.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, avatarProfileTexture.width, avatarProfileTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)this.transform);
        buttonInteraction = true;
        isImageLoaded = true;
    }

    public GameObject GetImageContainer()
    {
        return this.gameObject;
    }

    public void OnErrorSetImage(Sprite sprite)
    {
        avatarProfileImage.sprite = sprite;
        buttonInteraction = true;
        isImageLoaded = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (buttonInteraction && isImageLoaded) {
            InvokeEvent<ChangeAvatarOnClickEvent>(new ChangeAvatarOnClickEvent(avatarEntity, avatarProfileImage.sprite));
        }

    }
}
