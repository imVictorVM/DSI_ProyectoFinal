using UnityEngine;
using UnityEngine.UIElements;

public class LevelSelectorController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement[] tabs;
    private VisualElement levelCard;
    private Label levelStatusLabel;
    private Button toggleCompleteButton;

    private LevelProgressData progressData;
    private int currentLevelIndex = 0;

    private void OnEnable()
    {

        // Cargar progreso
        progressData = LevelProgressManager.LoadProgress(5);

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // Obtiene todas las pestañas.
        tabs = new VisualElement[5];
        for (int i = 0; i < 5; i++)
        {
            tabs[i] = root.Q<VisualElement>($"tab{i + 1}");
        }

        levelCard = root.Q<VisualElement>("level-card");
        levelStatusLabel = root.Q<Label>("level-status");
        toggleCompleteButton = root.Q<Button>("toggle-complete-btn");

        toggleCompleteButton.clicked += ToggleLevelCompletion;
        SmoothCameraLookAt.OnLevelChanged += UpdateSelectedTab;


        UpdateSelectedTab(0);

        // Suscribe al evento de cambio de nivel.
        SmoothCameraLookAt.OnLevelChanged += UpdateSelectedTab;
    }
    /*
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
    */

    private void UpdateSelectedTab(int levelIndex)
    {
        currentLevelIndex = levelIndex;

        // Actualizar pestañas
        foreach (var tab in tabs)
        {
            tab.RemoveFromClassList("selected");
        }
        tabs[levelIndex].AddToClassList("selected");

        // Actualizar tarjeta
        bool isCompleted = progressData.levelCompleted[levelIndex];
        levelStatusLabel.text = isCompleted ? "NIVEL COMPLETADO" : "NIVEL SIN COMPLETAR";
        toggleCompleteButton.text = isCompleted ? "Marcar incompleto" : "Marcar completo";
    }

    private void ToggleLevelCompletion()
    {
        progressData.levelCompleted[currentLevelIndex] = !progressData.levelCompleted[currentLevelIndex];
        LevelProgressManager.SaveProgress(progressData);
        UpdateSelectedTab(currentLevelIndex);
    }

    private void OnDisable()
    {
        SmoothCameraLookAt.OnLevelChanged -= UpdateSelectedTab;
    }
}