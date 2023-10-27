using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
#if PLATFORM_IOS
using UnityEngine.iOS;
#endif

public class ScanManager : MonoBehaviour
{
    private const string EVENT_TYPE = "events";
    private const string STORE_TYPE = "items";

    [SerializeField]
    private RawImage _previewRawImage;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;
    [SerializeField]
    private RectTransform _scanZone;
    private bool _isCamAvailable;
    private WebCamTexture _cameraTexture;
    private bool _canScan = false;
    private WaitForEndOfFrame delay = new WaitForEndOfFrame();

    private static ScanManager _instance;
    public static ScanManager instnace {
        get {
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<ScanManager>();
            return _instance;
        }
    }

    private void Start()
    {
#if UNITY_EDITOR
        SetUpWebCam();
#endif

#if PLATFORM_ANDROID
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            SetUpWebCam();
        }
        else {
            
            NoCameraErrorPopUp();
        }
#endif

#if PLATFORM_IOS
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            SetUpWebCam();
        }
        else
        {
            NoCameraErrorPopUp();
        }
#endif

    }

    private void Update()
    {
        UpdateCameraRender();
    }

    private void OnGUI()
    {
        if (_canScan && _isCamAvailable) {
            Scan();
        }
    }

    public void OnQRScanFinishedCallback(string result) {
        string[] parts = result.Split('/');
        string type = parts[0];
        string id = parts[1];

        if (type.Equals(EVENT_TYPE)) {

            ScreenManager.instance.ChangeView(ViewID.AssistPopUpViewModel);
            ScreenManager.instance.GetView(ViewID.AssistPopUpViewModel).GetComponent<AssistPopUpViewModel>().Initialize<AssistPopUpViewModel, AssistPopUpPresenter, AssistPopUpInteractor>(id);

        } else if (type.Equals(STORE_TYPE)) {
            ScreenManager.instance.ChangeView(ViewID.BuyPopUpViewModel);
            ScreenManager.instance.GetView(ViewID.BuyPopUpViewModel).GetComponent<BuyPopUpViewModel>().Initialize<BuyPopUpViewModel, BuyPopUpPresenter, BuyPopUpInteractor>(true, id);
        }
    }

    public void StopQRScan()
    {
        _cameraTexture.Stop();
    }

    public void ResetQRScan()
    {
        _canScan = true;
    }

    public void OnClickScan() {
        Scan();
    }

    private void UpdateCameraRender()
    {
        if (_isCamAvailable == false)
        {
            return;
        }

        float ratio = (float)_cameraTexture.width / (float)_cameraTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;

        //==== Esto hace que se vea corrija la vista cuando la camara se ve en espejo
        float scaleY = _cameraTexture.videoVerticallyMirrored ? -1f : 1f;
        _previewRawImage.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        //====

        int orientation = -_cameraTexture.videoRotationAngle;
        _previewRawImage.rectTransform.localEulerAngles = new Vector3(0, 0, orientation);
    }

    private void SetUpWebCam()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

#if UNITY_EDITOR
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].isFrontFacing == true)
            {
                //_cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);   // Si ponemos esto se ve muy pixeleado y aparetnemente no hay una razón para que sea este width y height -> preguntar
                _cameraTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                break;
            }
        }
#else
        for (int i = 0; i < devices.Length; i++) {
            if (devices[i].isFrontFacing == false) {
                //_cameraTexture = new WebCamTexture(devices[i].name, (int)_scanZone.rect.width, (int)_scanZone.rect.height);   // Si ponemos esto se ve muy pixeleado y aparetnemente no hay una razón para que sea este width y height -> preguntar
                _cameraTexture = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
                break;
            }
        }
#endif

        if (_cameraTexture == null) {
            NoCameraErrorPopUp();
            return;
        }

        _cameraTexture.Play();

        _previewRawImage.texture = _cameraTexture;

        _isCamAvailable = true;
        _canScan = true;
    }

    private void Scan() {

        _canScan = false;

        try
        {
            IBarcodeReader barcodeReader = new BarcodeReader();
            Result result = barcodeReader.Decode(_cameraTexture.GetPixels32(), _cameraTexture.width, _cameraTexture.height);
            if (result != null)
            {
                OnQRScanFinishedCallback(result.Text);
            }
            else {
                StartCoroutine(CR_OnResultNull());
            }
        }
        catch (Exception ex) {
            Debug.LogWarning(ex.Message);
            ResetQRScan();
        }
    }

    private void ScanErrorPopUp() {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "Error al escanear",
            "Por favor intentalo más tarde");
        popUpViewModel.SetPopUpAction(() => {
            StartCoroutine(CR_OnResultNull());
            ScreenManager.instance.BackToPreviousView(); });
    }

    private void NoCameraErrorPopUp()
    {
        AppManager.instance.LoadingViewModelSetActive(false);
        ScreenManager.instance.ChangeView(ViewID.PopUpViewModel, true);
        PopUpViewModel popUpViewModel = (PopUpViewModel)ScreenManager.instance.GetView(ViewID.PopUpViewModel);
        popUpViewModel.Initialize(PopUpViewModelTypes.Central, "No se encontró permiso de acceso a la cámara",
            "Accede a la configuración de tu dispositivo, en la sección de aplicaciones, en permisos, concede acceso a UP Adventures a usar la cámara de tú dispositivo para poder escanear los códigos QR.");
        popUpViewModel.SetPopUpAction(() => {
            ScreenManager.instance.GetView(ViewID.PopUpViewModel).gameObject.SetActive(false);
            SceneTransitionManager.instance.startLoadScene("MainScene");
        });
    }

    private IEnumerator CR_OnResultNull() {
        yield return delay;
        ResetQRScan();
    }
}
