using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PetTest
{
    private Scene testScene;
    private GameObject testSceneRoot;
    
    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Create a new test scene programmatically
        testScene = SceneManager.CreateScene("TestPetScene");
        SceneManager.SetActiveScene(testScene);
        
        // Create the test scene hierarchy
        yield return CreateTestScene();
        
        // Wait a frame for scene to fully load
        yield return null;
    }
    
    private IEnumerator CreateTestScene()
    {
        // Create main game object with Canvas
        testSceneRoot = new GameObject("TestSceneRoot");
        SceneManager.MoveGameObjectToScene(testSceneRoot, testScene);
        
        // Create Canvas
        GameObject canvasGO = new GameObject("Canvas");
        canvasGO.transform.SetParent(testSceneRoot.transform);
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();
        
        // Create UI elements
        GameObject loadingIndicator = new GameObject("LoadingIndicator");
        loadingIndicator.transform.SetParent(canvasGO.transform);
        loadingIndicator.AddComponent<Text>().text = "Loading...";
        
        GameObject errorPanel = new GameObject("ErrorPanel");
        errorPanel.transform.SetParent(canvasGO.transform);
        errorPanel.AddComponent<Text>().text = "Error occurred";
        errorPanel.SetActive(false);
        
        // Create pet list container
        GameObject petListContainer = new GameObject("PetListContainer");
        petListContainer.transform.SetParent(canvasGO.transform);
        
        // Create pet item prefab
        GameObject petItemPrefab = CreatePetItemPrefab();
        
        // Create and setup PetDisplay
        GameObject petDisplayGO = new GameObject("PetDisplay");
        petDisplayGO.transform.SetParent(testSceneRoot.transform);
        PetDisplay petDisplay = petDisplayGO.AddComponent<PetDisplay>();
        petDisplay.petItemPrefab = petItemPrefab;
        petDisplay.petListContainer = petListContainer.transform;
        
        // Create and setup ApiClient
        GameObject apiClientGO = new GameObject("ApiClient");
        apiClientGO.transform.SetParent(testSceneRoot.transform);
        ApiClient apiClient = apiClientGO.AddComponent<ApiClient>();
        
        // Create and setup MainController
        GameObject mainControllerGO = new GameObject("MainController");
        mainControllerGO.transform.SetParent(testSceneRoot.transform);
        MainController mainController = mainControllerGO.AddComponent<MainController>();
        mainController.apiClient = apiClient;
        mainController.petDisplay = petDisplay;
        mainController.loadingIndicator = loadingIndicator;
        mainController.errorPanel = errorPanel;
        
        yield return null;
    }
    
    private GameObject CreatePetItemPrefab()
    {
        GameObject prefab = new GameObject("PetItemPrefab");
        prefab.AddComponent<Image>(); // Background
        
        // Create cells
        GameObject idCell = new GameObject("IdCell");
        idCell.transform.SetParent(prefab.transform);
        idCell.AddComponent<Text>();
        
        GameObject nameCell = new GameObject("NameCell");
        nameCell.transform.SetParent(prefab.transform);
        nameCell.AddComponent<Text>();
        
        GameObject createdCell = new GameObject("CreatedCell");
        createdCell.transform.SetParent(prefab.transform);
        createdCell.AddComponent<Text>();
        
        GameObject updatedCell = new GameObject("UpdatedCell");
        updatedCell.transform.SetParent(prefab.transform);
        updatedCell.AddComponent<Text>();
        
        prefab.SetActive(false); // Prefabs should be inactive
        return prefab;
    }

    [UnityTest]
    public IEnumerator TestPetListDisplaysWithRealAPI()
    {
        // Wait for scene and API call to complete
        yield return new WaitForSeconds(3f);
        
        // Find the PetDisplay component
        PetDisplay petDisplay = Object.FindFirstObjectByType<PetDisplay>();
        Transform petListContainer = petDisplay.petListContainer;
        
        // Check if pets are displayed from real API
        int petCount = petListContainer.childCount;
        Assert.Greater(petCount, 0, "Should have pet data from real API");
        
        Debug.Log($"Success! Found {petCount} pets from real API");
    }
    
    [UnityTearDown]
    public IEnumerator TearDown()
    {
        // Clean up the test scene
        if (testSceneRoot != null)
        {
            Object.DestroyImmediate(testSceneRoot);
        }
        
        // Unload the test scene
        if (testScene.IsValid())
        {
            yield return SceneManager.UnloadSceneAsync(testScene);
        }
    }
}
