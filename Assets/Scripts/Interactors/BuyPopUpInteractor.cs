using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPopUpInteractor : Interactor
{

    public override void PerformSearch(params object[] list)
    {
        BuyMethods buyMethod = (BuyMethods)list[0];

        if (BuyMethods.GetStoreItem == buyMethod)
        {
            string[] parameters = { (string)list[2] };
            StartCoroutine(Get<GetStoreItem>((string)list[1], buyMethod, ProgressManager.instance.progress.userDataPersistance.bearer, parameters));
        }

        if (BuyMethods.PostBuy == buyMethod)
        {
            List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> item_id = new KeyValuePair<string, string>("id", (string)list[2]);
            fields.Add(item_id);
            StartCoroutine(Post<ResponseEntity>((string)list[1], buyMethod, ProgressManager.instance.progress.userDataPersistance.bearer, null, fields));
        }

        if (BuyMethods.GetUserData == buyMethod)
        {
            StartCoroutine(Get<GetUserEntity>((string)list[1], buyMethod, ProgressManager.instance.progress.userDataPersistance.bearer));
        }

    }
}
