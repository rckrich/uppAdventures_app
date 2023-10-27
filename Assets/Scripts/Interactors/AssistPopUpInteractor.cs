using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistPopUpInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        AssistMethods assistMethod = (AssistMethods)list[0];

        if (AssistMethods.GetEvent == assistMethod)
        {
            string[] parameters = { (string)list[2] };
            StartCoroutine(Get<GetEvent>((string)list[1], assistMethod, ProgressManager.instance.progress.userDataPersistance.bearer, parameters));
        }

        if (AssistMethods.PostAssist == assistMethod)
        {
            List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> item_id = new KeyValuePair<string, string>("id", ((int)list[2]).ToString());
            fields.Add(item_id);
            StartCoroutine(Post<ResponseEntity>((string)list[1], assistMethod, ProgressManager.instance.progress.userDataPersistance.bearer, null, fields));
        }

        if (AssistMethods.GetUserData == assistMethod)
        {
            StartCoroutine(Get<GetUserEntity>((string)list[1], assistMethod, ProgressManager.instance.progress.userDataPersistance.bearer));
        }
    }
}
