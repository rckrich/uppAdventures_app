﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Portrait_and_Landscape : MonoBehaviour
{
    void OnEnable()
    {
        Screen.orientation = ScreenOrientation.Landscape;
    }

    private void OnDisable()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}
