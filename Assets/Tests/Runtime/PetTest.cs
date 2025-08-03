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
        
        // Wait for Start() to be called by Unity
        yield return new WaitForEndOfFrame();
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
        Debug.Log("Starting API integration test...");
        
        // Find the PetDisplay component
        PetDisplay petDisplay = Object.FindFirstObjectByType<PetDisplay>();
        Transform petListContainer = petDisplay.petListContainer;
        MainController mainController = Object.FindFirstObjectByType<MainController>();
        
        // Wait for API call to complete with timeout
        float timeout = 10f;
        float timer = 0f;
        
        while (timer < timeout)
        {
            yield return new WaitForSeconds(0.5f);
            timer += 0.5f;
            
            // Check if we have pets loaded or error occurred
            if (petListContainer.childCount > 0)
            {
                Debug.Log($"Success! Found {petListContainer.childCount} pets from real API");
                Assert.Greater(petListContainer.childCount, 0, "Should have pet data from real API");
                yield break;
            }
            
            // Check if error panel is active (API failed)
            if (mainController.errorPanel.activeInHierarchy)
            {
                Debug.LogError("Error panel is active, API call failed");
                Assert.Fail("API call failed - error panel is active");
                yield break;
            }
            
            Debug.Log($"Waiting for API response... ({timer}s)");
        }
        
        // Timeout reached
        Debug.LogError($"Test timed out after {timeout}s. Pet count: {petListContainer.childCount}, Error panel active: {mainController.errorPanel.activeInHierarchy}");
        Assert.Fail($"Test timed out after {timeout}s waiting for API response");
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
