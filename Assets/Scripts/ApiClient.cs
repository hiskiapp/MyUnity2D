using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ApiClient : MonoBehaviour
{
    private const string DefaultApiUrl = "http://localhost:5018/pets";

    public IEnumerator GetPets(System.Action<Pet[]> onSuccess, System.Action<string> onError)
    {
        var apiUrl = System.Environment.GetEnvironmentVariable("API_URL") ?? DefaultApiUrl;

    using var webRequest = UnityWebRequest.Get(apiUrl);
    yield return webRequest.SendWebRequest();

    if (webRequest.result != UnityWebRequest.Result.Success)
    {
      onError?.Invoke(webRequest.error);
    }
    else
    {
      var json = webRequest.downloadHandler.text;
      // Manually wrap the JSON array in an object to make it compatible with JsonUtility
      string wrappedJson = "{\"pets\":" + json + "}";
      PetList petList = JsonUtility.FromJson<PetList>(wrappedJson);
      onSuccess?.Invoke(petList.pets);
    }
  }
}
