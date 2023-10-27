using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {
        StartCoroutine(Get<GetUserEntity>((string)list[0], (string)list[1]));
    }
}
