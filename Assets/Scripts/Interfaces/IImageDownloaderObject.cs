using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IImageDownloaderObject
{
    void SetImage(Texture2D texture);
    GameObject GetImageContainer();
    void OnErrorSetImage(Sprite sprite);
}
