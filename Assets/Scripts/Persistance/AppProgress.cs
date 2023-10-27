using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserDataPersistance
{
    public long id = 0;
    public string userName = "";
    public string avatarThumbnail = "";
    public string bearer = "";
    public int UPCoins = 0;
    public int puntosCulturales = 0;
    public int puntosDeportivos = 0;
    public int puntosAcademicos = 0;
    public int puntosAsuntosEstudiantiles = 0;
    public int puntosMovimientoUP = 0;
}

[System.Serializable]
public class AppProgress
{
    [SerializeField] public UserDataPersistance userDataPersistance;
}
