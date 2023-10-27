using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        if((StoreGetMethods)list[0] == StoreGetMethods.GetStoreItems)
            StartCoroutine(Get<GetStoreItems>((string)list[1], StoreGetMethods.GetStoreItems, ProgressManager.instance.progress.userDataPersistance.bearer));

        if ((StoreGetMethods)list[0] == StoreGetMethods.GetUserAvatars)
            StartCoroutine(Get<GetAvatars>((string)list[1], StoreGetMethods.GetUserAvatars, ProgressManager.instance.progress.userDataPersistance.bearer));
    }

}
