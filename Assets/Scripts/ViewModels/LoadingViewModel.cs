using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingViewModel : ViewModel
{
    public void OpenLoadingViewModel() {
        this.gameObject.SetActive(true);
    }

    public void CloseLoadingViewModel()
    {
        this.gameObject.SetActive(false);
    }

}
