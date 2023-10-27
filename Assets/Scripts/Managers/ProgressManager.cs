using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    private static ProgressManager self;

    public static ProgressManager instance
    {
        get
        {
            if (self == null)
            {
                self = (ProgressManager)GameObject.FindObjectOfType(typeof(ProgressManager));
            }
            return self;
        }
    }

    public static ProgressManager dontDestroyProgressM;

    public AppProgress progress;

    public void Save()
    {
        FileManager.SaveProgress(this.progress);
    }

    // Use this for initialization
    void Awake()
    {
        if (dontDestroyProgressM == null)
        {
            DontDestroyOnLoad(gameObject);
            dontDestroyProgressM = this;
        }
        else if (dontDestroyProgressM != this)
        {
            Destroy(gameObject);
        }
        InitApp();
    }

    // Update is called once per frame
    private void InitApp()
    {
        AppProgress progress = FileManager.LoadProgress();
        if (progress == null)
        {
            Debug.Log("No se ha encontrado los datos de la app, inicializando datos de la app...");
            FileManager.SaveProgress(this.progress);
#if UNITY_IOS
            FileManager.setNoBackUpIOS();
#endif
        }
        else
        {
            this.progress = progress;
        }

    }

}
