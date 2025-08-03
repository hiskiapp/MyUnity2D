
using UnityEngine;
using UnityEngine.UI;

public class UiSetup : MonoBehaviour
{
    public static void CreatePetListUI(MainController mainController)
    {
        // Create Canvas
        var canvasObject = new GameObject("Canvas");
        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create centered container for the pet table
        var containerObject = new GameObject("TableContainer");
        containerObject.transform.SetParent(canvas.transform, false);
        var containerRect = containerObject.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(0.5f, 0.5f);
        containerRect.anchorMax = new Vector2(0.5f, 0.5f);
        containerRect.pivot = new Vector2(0.5f, 0.5f);
        containerRect.sizeDelta = new Vector2(800, 600); // Fixed size centered table

        // Create PetListPanel
        var panelObject = new GameObject("PetListPanel");
        panelObject.transform.SetParent(containerObject.transform, false);
        var panelRect = panelObject.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 1);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;
        var panelImage = panelObject.AddComponent<Image>();
        panelImage.color = new Color(0.95f, 0.95f, 0.95f, 1f); // Light gray background

        // Add Vertical Layout Group to Panel
        var vlg = panelObject.AddComponent<VerticalLayoutGroup>();
        vlg.padding = new RectOffset(20, 20, 20, 20);
        vlg.spacing = 15;
        vlg.childControlHeight = false;
        vlg.childControlWidth = true;
        vlg.childForceExpandHeight = false;
        vlg.childForceExpandWidth = true;

        // Create Title
        var titleObject = new GameObject("Title");
        titleObject.transform.SetParent(panelObject.transform, false);
        var titleText = titleObject.AddComponent<Text>();
        titleText.text = "Pet Management Table";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 28;
        titleText.fontStyle = FontStyle.Bold;
        titleText.color = new Color(0.1f, 0.1f, 0.2f, 1f);
        titleText.alignment = TextAnchor.MiddleCenter;
        var titleRect = titleObject.GetComponent<RectTransform>();
        titleRect.sizeDelta = new Vector2(0, 50);

        // Create Table Header
        var headerObject = new GameObject("TableHeader");
        headerObject.transform.SetParent(panelObject.transform, false);
        var headerRect = headerObject.AddComponent<RectTransform>();
        headerRect.sizeDelta = new Vector2(0, 40);
        var headerImage = headerObject.AddComponent<Image>();
        headerImage.color = new Color(0.2f, 0.3f, 0.5f, 1f); // Dark blue header background
        
        var headerLayout = headerObject.AddComponent<HorizontalLayoutGroup>();
        headerLayout.padding = new RectOffset(15, 15, 10, 10);
        headerLayout.spacing = 10;
        headerLayout.childControlHeight = true;
        headerLayout.childControlWidth = false;
        headerLayout.childForceExpandHeight = true;
        headerLayout.childAlignment = TextAnchor.MiddleCenter;

        // Create header columns
        CreateHeaderColumn("ID", headerObject, 80);
        CreateHeaderColumn("Pet Name", headerObject, 200);
        CreateHeaderColumn("Created Date", headerObject, 180);
        CreateHeaderColumn("Updated Date", headerObject, 180);


        // Create Scroll View for table rows
        var scrollViewObject = new GameObject("TableScrollView");
        scrollViewObject.transform.SetParent(panelObject.transform, false);
        var scrollRect = scrollViewObject.AddComponent<ScrollRect>();
        var scrollViewRect = scrollViewObject.GetComponent<RectTransform>();
        scrollViewRect.sizeDelta = new Vector2(0, 0); // Will be controlled by layout group
        var scrollLayoutElement = scrollViewObject.AddComponent<LayoutElement>();
        scrollLayoutElement.flexibleHeight = 1; // Take remaining space
        scrollViewObject.AddComponent<Image>().color = new Color(1f, 1f, 1f, 1f); // White background for table

        // Create Viewport
        var viewportObject = new GameObject("Viewport");
        viewportObject.transform.SetParent(scrollViewObject.transform, false);
        var viewportRect = viewportObject.AddComponent<RectTransform>();
        viewportRect.anchorMin = new Vector2(0, 0);
        viewportRect.anchorMax = new Vector2(1, 1);
        viewportRect.pivot = new Vector2(0, 1);
        viewportRect.offsetMin = Vector2.zero;
        viewportRect.offsetMax = Vector2.zero;
        viewportObject.AddComponent<Mask>().showMaskGraphic = false;

        // Create Content object for table rows
        var contentObject = new GameObject("TableContent");
        contentObject.transform.SetParent(viewportObject.transform, false);
        var contentRect = contentObject.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0, 1);
        contentRect.anchorMax = new Vector2(1, 1);
        contentRect.pivot = new Vector2(0.5f, 1);
        
        var contentVlg = contentObject.AddComponent<VerticalLayoutGroup>();
        contentVlg.padding = new RectOffset(0, 0, 0, 0);
        contentVlg.spacing = 1; // Small spacing between rows
        contentVlg.childAlignment = TextAnchor.UpperCenter;
        contentVlg.childControlHeight = false;
        contentVlg.childControlWidth = true;
        contentVlg.childForceExpandHeight = false;

        var contentSizeFitter = contentObject.AddComponent<ContentSizeFitter>();
        contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        scrollRect.viewport = viewportRect;
        scrollRect.content = contentRect;


        // Create Table Row Prefab
        var petRowPrefab = new GameObject("PetRowPrefab");
        var rowLayout = petRowPrefab.AddComponent<LayoutElement>();
        rowLayout.minHeight = 35;
        var rowImage = petRowPrefab.AddComponent<Image>();
        rowImage.color = new Color(0.98f, 0.98f, 0.98f, 1f); // Very light gray for rows
        
        var rowHlg = petRowPrefab.AddComponent<HorizontalLayoutGroup>();
        rowHlg.padding = new RectOffset(15, 15, 8, 8);
        rowHlg.spacing = 10;
        rowHlg.childControlHeight = true;
        rowHlg.childControlWidth = false;
        rowHlg.childForceExpandHeight = true;
        rowHlg.childAlignment = TextAnchor.MiddleLeft;

        // Create table cells matching header columns
        var idCellObject = new GameObject("IdCell");
        idCellObject.transform.SetParent(petRowPrefab.transform, false);
        var idText = idCellObject.AddComponent<Text>();
        idText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        idText.fontSize = 16;
        idText.color = new Color(0.1f, 0.1f, 0.2f, 1f);
        idText.alignment = TextAnchor.MiddleCenter;
        var idLayout = idCellObject.AddComponent<LayoutElement>();
        idLayout.preferredWidth = 80;

        var nameCellObject = new GameObject("NameCell");
        nameCellObject.transform.SetParent(petRowPrefab.transform, false);
        var nameText = nameCellObject.AddComponent<Text>();
        nameText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        nameText.fontSize = 16;
        nameText.fontStyle = FontStyle.Bold;
        nameText.color = new Color(0.1f, 0.1f, 0.2f, 1f);
        nameText.alignment = TextAnchor.MiddleLeft;
        var nameLayout = nameCellObject.AddComponent<LayoutElement>();
        nameLayout.preferredWidth = 200;

        var createdCellObject = new GameObject("CreatedCell");
        createdCellObject.transform.SetParent(petRowPrefab.transform, false);
        var createdText = createdCellObject.AddComponent<Text>();
        createdText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        createdText.fontSize = 14;
        createdText.color = new Color(0.3f, 0.3f, 0.4f, 1f);
        createdText.alignment = TextAnchor.MiddleCenter;
        var createdLayout = createdCellObject.AddComponent<LayoutElement>();
        createdLayout.preferredWidth = 180;

        var updatedCellObject = new GameObject("UpdatedCell");
        updatedCellObject.transform.SetParent(petRowPrefab.transform, false);
        var updatedText = updatedCellObject.AddComponent<Text>();
        updatedText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        updatedText.fontSize = 14;
        updatedText.color = new Color(0.3f, 0.3f, 0.4f, 1f);
        updatedText.alignment = TextAnchor.MiddleCenter;
        var updatedLayout = updatedCellObject.AddComponent<LayoutElement>();
        updatedLayout.preferredWidth = 180;

        petRowPrefab.SetActive(false);


        // Create Loading Indicator
        var loadingIndicatorObject = new GameObject("LoadingIndicator");
        loadingIndicatorObject.transform.SetParent(canvas.transform, false);
        var loadingText = loadingIndicatorObject.AddComponent<Text>();
        loadingText.text = "Loading...";
        loadingText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        loadingText.fontSize = 24;
        loadingText.color = Color.black;
        loadingText.alignment = TextAnchor.MiddleCenter;
        var loadingRect = loadingIndicatorObject.GetComponent<RectTransform>();
        loadingRect.anchorMin = new Vector2(0.5f, 0.5f);
        loadingRect.anchorMax = new Vector2(0.5f, 0.5f);
        loadingRect.pivot = new Vector2(0.5f, 0.5f);
        loadingRect.sizeDelta = new Vector2(200, 50);


        // Create Error Panel
        var errorPanelObject = new GameObject("ErrorPanel");
        errorPanelObject.transform.SetParent(canvas.transform, false);
        var errorImage = errorPanelObject.AddComponent<Image>();
        errorImage.color = new Color(1, 0.3f, 0.3f, 0.8f); // Reddish background for error
        var errorRect = errorPanelObject.GetComponent<RectTransform>();
        errorRect.anchorMin = new Vector2(0.5f, 0.5f);
        errorRect.anchorMax = new Vector2(0.5f, 0.5f);
        errorRect.pivot = new Vector2(0.5f, 0.5f);
        errorRect.sizeDelta = new Vector2(350, 120);

        var errorTextObject = new GameObject("ErrorText");
        errorTextObject.transform.SetParent(errorPanelObject.transform, false);
        var errorText = errorTextObject.AddComponent<Text>();
        errorText.text = "Failed to load pets. Please try again later.";
        errorText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        errorText.fontSize = 20;
        errorText.color = Color.white;
        errorText.alignment = TextAnchor.MiddleCenter;


        // Assign to MainController
        mainController.loadingIndicator = loadingIndicatorObject;
        mainController.errorPanel = errorPanelObject;

        // Assign to PetDisplay
        var petDisplay = mainController.GetComponent<PetDisplay>();
        petDisplay.petItemPrefab = petRowPrefab;
        petDisplay.petListContainer = contentObject.transform;
    }

    private static void CreateHeaderColumn(string title, GameObject parent, float width)
    {
        var columnObject = new GameObject(title + "Header");
        columnObject.transform.SetParent(parent.transform, false);
        var columnText = columnObject.AddComponent<Text>();
        columnText.text = title;
        columnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        columnText.fontSize = 16;
        columnText.fontStyle = FontStyle.Bold;
        columnText.color = Color.white;
        columnText.alignment = TextAnchor.MiddleCenter;
        var columnLayout = columnObject.AddComponent<LayoutElement>();
        columnLayout.preferredWidth = width;
    }
}
