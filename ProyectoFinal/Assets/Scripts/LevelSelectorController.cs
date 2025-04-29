using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelSelectorController : MonoBehaviour
{
    private UIDocument uiDocument;
    private VisualElement[] tabs;
    private VisualElement levelCard;
    private Label levelStatusLabel;
    private Button toggleCompleteButton;
    private Button backButton;


    private LevelProgressData progressData;
    private int currentLevelIndex = 0;
    [SerializeField ]private SmoothCameraLookAt cameraMovement;

    //cambio camara
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private UIDocument levelSelector;
    [SerializeField] private LevelSelectorController selector;
    [SerializeField] private FirstPersonAIO fpsController;

    private void OnEnable()
    {

        // Cargar progreso
        progressData = LevelProgressManager.LoadProgress(5);

        uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;


        backButton = root.Q<Button>("back-button");
        backButton.clicked += () =>
        {
            changeCamera();
        };

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


    private void UpdateSelectedTab(int levelIndex)
    {
        currentLevelIndex = levelIndex;

        for (int i = 0; i < tabs.Length; i++)
        {
            var tab = tabs[i];
            tab.RemoveFromClassList("selected");

            tab.Clear();

            // Crear elemento de imagen
            var image = new VisualElement();
            image.AddToClassList("level-tab-image");

            // Asignar imagen según el estado
            if (i == levelIndex)
            {
                image.style.backgroundImage = new StyleBackground(
                    Resources.Load<Texture2D>("Sprites/tartacolor")
                );
                tab.AddToClassList("selected");
            }
            else
            {
                image.style.backgroundImage = new StyleBackground(
                    Resources.Load<Texture2D>("Sprites/tartagris")
                );
            }

            tab.Add(image);
        }

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

    private void changeCamera()
    {
        // Cambiar cámaras
        mainCamera.enabled = false;
        firstPersonCamera.enabled = true;


        fpsController.enabled = true;


        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        // Activa el crosshair
        fpsController.autoCrosshair = true;

        if (fpsController.Crosshair != null)
        {
            // Busca el Canvas generado automáticamente
            Canvas crosshairCanvas = fpsController.playerCamera.GetComponentInChildren<Canvas>();
            if (crosshairCanvas != null)
            {
                crosshairCanvas.enabled = true;
            }
        }


        levelSelector.enabled = false;
        selector.enabled = false;
    }

    public void resetPosition()
    {
        UpdateSelectedTab(cameraMovement.getActualPosition());
    }
}