using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanHeaderViewModel : HeaderViewModel
{
    private void Start()
    {
        actionButtonAction = () => {
            SceneTransitionManager.instance.startLoadScene("MainScene");
        };
    }
}
