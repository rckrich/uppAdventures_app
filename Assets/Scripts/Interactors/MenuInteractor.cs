using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
        StartCoroutine(Post<ResponseEntity>((string)list[0], ProgressManager.instance.progress.userDataPersistance.bearer, null, fields));
    }
}
