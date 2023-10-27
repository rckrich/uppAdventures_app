using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAvatarInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        if ((ChangeAvatarMethods)list[0] == ChangeAvatarMethods.GetUserAvatars) {
            StartCoroutine(Get<GetAvatars>((string)list[1], ChangeAvatarMethods.GetUserAvatars, ProgressManager.instance.progress.userDataPersistance.bearer));
        }

        if ((ChangeAvatarMethods)list[0] == ChangeAvatarMethods.PostChangeAvatar)
        {
            List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> id = new KeyValuePair<string, string>("id", (string)list[2]);
            fields.Add(id);
            StartCoroutine(Post<ResponseEntity>((string)list[1], ChangeAvatarMethods.PostChangeAvatar, ProgressManager.instance.progress.userDataPersistance.bearer, null, fields));
        }
    }
}
