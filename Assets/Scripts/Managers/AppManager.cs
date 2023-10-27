using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
#endif

[System.Serializable]
public class EventImageType {
    public int id;
    public string typeName;
    public Sprite typeSprite;
    public Color typeColor;
}

public class AppManager : Manager
{
    private static AppManager _instance;
    public static AppManager instance {
        get {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<AppManager>();

            return _instance;
        }
    }

    public List<EventImageType> eventImageTypeList;
    public Sprite nullAvatar;
    private LoadingViewModel loadingViewModel;

    private void Awake()
    {
        loadingViewModel = ScreenManager.instance.GetView(ViewID.LoadingViewModel).GetComponent<LoadingViewModel>();
    }

    private void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif

#if PLATFORM_IOS
        StartCoroutine(FindWebCams());
#endif
    }

    public Sprite GetEventTypeSprite(int _id)
    {
        foreach (EventImageType eventImageType in eventImageTypeList) {
            if (_id == eventImageType.id)
                return eventImageType.typeSprite;
        }

        return null;
    }

    public string GetEventTypeName(int _id)
    {
        foreach (EventImageType eventImageType in eventImageTypeList)
        {
            if (_id == eventImageType.id)
                return eventImageType.typeName;
        }

        return "";
    }

    public Color GetEventTypeColor(int _id)
    {
        foreach (EventImageType eventImageType in eventImageTypeList)
        {
            if (_id == eventImageType.id)
                return eventImageType.typeColor;
        }

        return new Color32(54, 64, 187, 255);
    }

    public int GetAllUserAllPoints() {
        return (ProgressManager.instance.progress.userDataPersistance.puntosAcademicos +
            ProgressManager.instance.progress.userDataPersistance.puntosAsuntosEstudiantiles +
            ProgressManager.instance.progress.userDataPersistance.puntosCulturales +
            ProgressManager.instance.progress.userDataPersistance.puntosDeportivos +
            ProgressManager.instance.progress.userDataPersistance.puntosMovimientoUP);
    }

    public int GetUserUPCoins()
    {
        return (ProgressManager.instance.progress.userDataPersistance.UPCoins);
    }

    public void LoadingViewModelSetActive(bool _value) {
        if (_value)
        {
            loadingViewModel.OpenLoadingViewModel();
        }
        else {
            loadingViewModel.CloseLoadingViewModel();
        }
    }

#if PLATFORM_IOS
    private IEnumerator FindWebCams()
    {
        findWebCams();

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("webcam found");
        }
        else
        {
            Debug.Log("webcam not found");
        }

    }

    void findWebCams()
    {
        foreach (var device in WebCamTexture.devices)
        {
            Debug.Log("Name: " + device.name);
        }
    }
#endif

}
