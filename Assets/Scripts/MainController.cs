
using UnityEngine;

public class MainController : MonoBehaviour
{
        [HideInInspector] public ApiClient apiClient;
    [HideInInspector] public PetDisplay petDisplay;
    public GameObject loadingIndicator;
    public GameObject errorPanel;

    void Start()
    {
        loadingIndicator.SetActive(true);
        errorPanel.SetActive(false);

        StartCoroutine(apiClient.GetPets(
            pets =>
            {
                loadingIndicator.SetActive(false);
                petDisplay.DisplayPets(pets);
            },
            error =>
            {
                loadingIndicator.SetActive(false);
                errorPanel.SetActive(true);
                Debug.LogError(error);
            }
        ));
    }
}
