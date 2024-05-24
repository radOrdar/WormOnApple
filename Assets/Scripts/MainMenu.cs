using Loading;
using Services;
using Services.ScreenLoading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        ServiceLocator.Instance.Get<ILoadingScreenProvider>().LoadAndDestroy(new GameLoadingOperation());
    }
}