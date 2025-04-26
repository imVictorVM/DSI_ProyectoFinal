using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectorController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement[] tabs;

    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Obtiene todas las pestañas.
        tabs = new VisualElement[5];
        for (int i = 0; i < 5; i++)
        {
            tabs[i] = root.Q<VisualElement>($"tab{i + 1}");
        }

        // Suscribe al evento de cambio de nivel.
        SmoothCameraLookAt.OnLevelChanged += UpdateSelectedTab;
    }

    private void UpdateSelectedTab(int levelIndex)
    {
        // Deselecciona todas las pestañas.
        foreach (var tab in tabs)
        {
            tab.RemoveFromClassList("selected");
        }

        // Selecciona la pestaña actual.
        if (levelIndex >= 0 && levelIndex < tabs.Length)
        {
            tabs[levelIndex].AddToClassList("selected");
        }
    }

    private void OnDisable()
    {
        SmoothCameraLookAt.OnLevelChanged -= UpdateSelectedTab;
    }
}