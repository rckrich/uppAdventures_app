using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssistedEventFeedInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        string[] parameters = new string[] { "attended/" + ((int)list[1]).ToString(), "page/" + ((int)list[2]).ToString() };
        StartCoroutine(Get<GetEvents>((string)list[0], ProgressManager.instance.progress.userDataPersistance.bearer, parameters));
    }
}
