using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{

    private static SceneTransitionManager self;
    public static SceneTransitionManager instance
    {
        get
        {
            if (self == null)
            {
                self = GameObject.FindObjectOfType<SceneTransitionManager>();
            }
            return self;
        }
    }

    private const string TRANSITION_SCENE_NAME = "TransitionScene";
    private const float LOAD_SCENE_TIME_ACTIVATION = 0.9f;

    private string _sceneToUnload;
    private bool _loadingScene;

    public GameObject loadingCanvas;

    public float minLoadingTime;

    public static SceneTransitionManager dontDestroySceneTransitionM;

    void Awake()
    {
        if (dontDestroySceneTransitionM == null)
        {
            DontDestroyOnLoad(gameObject);
            dontDestroySceneTransitionM = this;
        }
        else if (dontDestroySceneTransitionM != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        _loadingScene = false;
    }

    public void startLoadScene(string sceneName)
    {
        if (!_loadingScene)
        {
            StartCoroutine(loadProcess(sceneName, minLoadingTime));
        }
    }

    public void startLoadScene(string sceneName, float minLoadingTime)
    {
        if (!_loadingScene)
        {
            StartCoroutine(loadProcess(sceneName, minLoadingTime));
        }
    }

    private IEnumerator loadProcess(string sceneName, float minLoadingTime)
    {
        //Activate loading animation
        loadingCanvas.GetComponent<Animator>().SetBool("Loading", true);

        //Get origin scene name
        _sceneToUnload = SceneManager.GetActiveScene().name;

        //Load LoadingScene
        _loadingScene = true;
        AsyncOperation asyncOperationToLoadingScene = SceneManager.LoadSceneAsync(TRANSITION_SCENE_NAME, LoadSceneMode.Additive);
        asyncOperationToLoadingScene.allowSceneActivation = false;
        yield return new WaitForSeconds(minLoadingTime);
        while (asyncOperationToLoadingScene.progress < LOAD_SCENE_TIME_ACTIVATION)
        {
            yield return null;
        }
        asyncOperationToLoadingScene.allowSceneActivation = true;
        while (!asyncOperationToLoadingScene.isDone)
        {
            yield return null;
        }

        //Unload origin scene
        AsyncOperation asyncOperationToUnloadOriginScene = SceneManager.UnloadSceneAsync(_sceneToUnload);
        while (!asyncOperationToUnloadOriginScene.isDone)
        {
            yield return null;
        }

        //Garbage Collector collects, UnloadUnusedAssets and reset Event Manager
        AsyncOperation asyncOperationUnloadUnusedAssets = Resources.UnloadUnusedAssets(); ;
        while (!asyncOperationUnloadUnusedAssets.isDone)
        {
            yield return null;
        }
        System.GC.Collect();
        EventManager.instance.resetEventManager();

        //Load LoadingScene
        _loadingScene = true;
        AsyncOperation asyncOperationToDestinyScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncOperationToDestinyScene.allowSceneActivation = false;
        yield return new WaitForSeconds(minLoadingTime);
        while (asyncOperationToDestinyScene.progress < LOAD_SCENE_TIME_ACTIVATION)
        {
            yield return null;
        }
        asyncOperationToDestinyScene.allowSceneActivation = true;
        while (!asyncOperationToDestinyScene.isDone)
        {
            yield return null;
        }

        //Unload LoadingScene scene
        AsyncOperation asyncOperationToUnloadLoadingScene = SceneManager.UnloadSceneAsync(TRANSITION_SCENE_NAME);
        while (!asyncOperationToUnloadLoadingScene.isDone)
        {
            yield return null;
        }

        _loadingScene = false;

        //Deactivate loading animation
        loadingCanvas.GetComponent<Animator>().SetBool("Loading", false);
    }

    public Scene GetCurrentScene() {
        return SceneManager.GetActiveScene();
    }

}
