using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Scene_Manager : Singleton<Scene_Manager> 
{
	public GameObject loadingCanvas;
	string m_loadingTransition;
	string[] availableScenes = new string[2] {"01_Title", "02_Main"}; 

	void Start ()
	{
		m_loadingTransition = "LoadingTransition";
	}

    /// <summary>
    /// Btn to load a scene
    /// **0 => Title**,  **1 => Main**
    /// </summary>
    /// <param name="value">Value.</param>
    public void Btn_SceneToLoad_IntValue(int value)
	{
		string sceneToLoad = availableScenes [value];

		StartCoroutine (LoadSceneRoutine (sceneToLoad));
	}

    public void Btn_SceneToLoad_StringValue(string value)
    {
        string sceneToLoad = value;

        StartCoroutine(LoadSceneRoutine(sceneToLoad));
    }

    IEnumerator LoadSceneRoutine(string sceneToLoad)
	{
		// Activamos objeto o animacion del loading
		//loadingCanvas.transform.GetChild (0).gameObject.SetActive (true);
		loadingCanvas.GetComponent<Animator> ().SetBool ("Loading", true);

		yield return new WaitForSeconds (0.5f);	

		// Activamos la transicion a la escena vacia para liberar memoria
		AsyncOperation asyncTransition = SceneManager.LoadSceneAsync (m_loadingTransition, LoadSceneMode.Single); 		

		while (!asyncTransition.isDone) 												
		{
			yield return null;
		}
			
		yield return new WaitForSeconds (1f);	

		// Activamos la transicion a la escena a la que originalmente queriamos ir
		AsyncOperation async = SceneManager.LoadSceneAsync (sceneToLoad, LoadSceneMode.Single); 		

		while (!async.isDone) 												
		{
			yield return null;
		}
			
		yield return new WaitForSeconds (0.5f);	

		//loadingCanvas.transform.GetChild (0).gameObject.SetActive (false);
		loadingCanvas.GetComponent<Animator> ().SetBool ("Loading", false);
	}
}
