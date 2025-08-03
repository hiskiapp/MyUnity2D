
using UnityEngine;

public class AppSetup
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnApplicationStart()
    {
        var appGameObject = new GameObject("AppController");
        var mainController = appGameObject.AddComponent<MainController>();
        appGameObject.AddComponent<PetDisplay>();
        appGameObject.AddComponent<ApiClient>();

        mainController.apiClient = appGameObject.GetComponent<ApiClient>();
        mainController.petDisplay = appGameObject.GetComponent<PetDisplay>();

        UiSetup.CreatePetListUI(mainController);

        Object.DontDestroyOnLoad(appGameObject);
    }
}
