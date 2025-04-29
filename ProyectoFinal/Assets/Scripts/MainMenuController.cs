using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset menuUXML;
    [SerializeField] private VisualTreeAsset settingsUXML;
    private UIDocument uiDocument;
    private VisualElement root;

    // Enum para los diferentes estados del menú
    private enum MenuState { Main, Settings }
    private MenuState currentState;


    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        ChangeState(MenuState.Main);
    }
    private void OnEnable()
    {
        uiDocument = GetComponent<UIDocument>();
        ChangeState(MenuState.Main);
    }

    private void ChangeState(MenuState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case MenuState.Main:
                ShowMainMenu();
                break;
            case MenuState.Settings:
                ShowSettingsMenu();
                break;
        }
    }

    private void ShowMainMenu()
    {
        uiDocument.visualTreeAsset = menuUXML;
        root = uiDocument.rootVisualElement;

        // Limpiar eventos previos para evitar duplicados
        root.Q<Button>("playButton").clicked -= () => HandleButtonClick("play");
        root.Q<Button>("settingsButton").clicked -= () => HandleButtonClick("settings");
        root.Q<Button>("quitButton").clicked -= () => HandleButtonClick("quit");

        // Configurar botones
        SetupButton(root.Q<Button>("playButton"), "play");
        SetupButton(root.Q<Button>("settingsButton"), "settings");
        SetupButton(root.Q<Button>("quitButton"), "quit");
    }

    private void ShowSettingsMenu()
    {
        uiDocument.visualTreeAsset = settingsUXML;
        root = uiDocument.rootVisualElement;

        // Limpiar evento previo
        root.Q<Button>("backButton").clicked -= () => ChangeState(MenuState.Main);

        // Configurar botón de retroceso
        SetupButton(root.Q<Button>("backButton"), "back");
    }

    private void SetupButton(Button button, string action)
    {
        if (button == null) return;

        button.RegisterCallback<MouseEnterEvent>(evt => {
            StartCoroutine(ShakeAnimation(button));
        });

        button.clicked += () => HandleButtonClick(action);
    }

    private IEnumerator ShakeAnimation(VisualElement element)
    {
        Vector3 originalPos = element.transform.position;
        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Mathf.Sin(elapsed * 50f) * 5f;
            element.transform.position = new Vector3(
                originalPos.x + offsetX,
                originalPos.y,
                originalPos.z
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        element.transform.position = originalPos;
    }

    private void HandleButtonClick(string action)
    {
        switch (action)
        {
            case "play":
                SceneManager.LoadScene("MainScene");
                break;
            case "settings":
                ChangeState(MenuState.Settings);
                break;
            case "back":
                ChangeState(MenuState.Main);
                break;
            case "quit":
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }
}