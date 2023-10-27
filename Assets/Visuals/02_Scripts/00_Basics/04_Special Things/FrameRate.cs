using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour
{
    // Hacemos esto para que se vea fluido el scroll porque unity baja el frame rate a 30 para moviles por temas de optimizacion
    void OnEnable()
    {
        Application.targetFrameRate = 60; 
    }

    
}
