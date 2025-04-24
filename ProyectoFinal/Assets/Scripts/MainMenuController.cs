using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private VisualElement root;
    private Button playButton;
    private Button quitButton;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // Referencias a los botones
        playButton = root.Q<Button>("playButton");
        quitButton = root.Q<Button>("quitButton");

        // Eventos
        playButton.clicked += OnPlayButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;

        // Efecto hover (opcional)
        //  playButton.RegisterCallback<MouseEnterEvent>(e => AudioManager.PlaySound("hover"));
        //  quitButton.RegisterCallback<MouseEnterEvent>(e => AudioManager.PlaySound("hover"));
        
        
            // Desactiva hover programáticamente para pruebas
            playButton.RegisterCallback<MouseEnterEvent>(e => { });
            quitButton.RegisterCallback<MouseEnterEvent>(e => { });
        
    }

    private void OnPlayButtonClicked()
    {
    //    AudioManager.PlaySound("select");
        SceneManager.LoadScene("MainScene"); // Cambia al nombre de tu escena.
    }

    private void OnQuitButtonClicked()
    {
      //  AudioManager.PlaySound("select");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}