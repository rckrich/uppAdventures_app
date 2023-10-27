using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class FileManager
{
    private const string PERSISTENT_PROGRESS_DATA_PATH = "/appProgress.bin";

    public static AppProgress LoadProgress()
    {
        AppProgress appProgress = null;
        if (File.Exists(Application.persistentDataPath + PERSISTENT_PROGRESS_DATA_PATH))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + PERSISTENT_PROGRESS_DATA_PATH, FileMode.Open);
            appProgress = (AppProgress)formatter.Deserialize(file);
            file.Close();
            return appProgress;
        }
        else
        {
            Debug.LogWarning("No se encuentra el archivo de progresso de la App");
            return appProgress;
        }
    }

    public static void SaveProgress(AppProgress progress)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + PERSISTENT_PROGRESS_DATA_PATH);
        formatter.Serialize(file, progress);
        file.Close();
    }

#if UNITY_IOS
    public static void setNoBackUpIOS() {
        UnityEngine.iOS.Device.SetNoBackupFlag(Application.persistentDataPath + PERSISTENT_PROGRESS_DATA_PATH);
    }
#endif
}
