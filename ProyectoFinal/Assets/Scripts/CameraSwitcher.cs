using UnityEngine;
using UnityEngine.UIElements;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask clickableLayer;
    [SerializeField] private LayerMask antiRaycastLayer;

    [SerializeField] private UIDocument levelSelector;
    [SerializeField] private LevelSelectorController selector;
    private FirstPersonAIO fpsController;
    private void Start()
    {
        // Configurar cámaras al inicio
        firstPersonCamera.enabled = true;
        mainCamera.enabled = false;
    }

    private void Awake()
    {
        levelSelector.enabled = false;
        selector.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detecta clic izquierdo
        {
            Ray ray = firstPersonCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, antiRaycastLayer)) return;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayer))
            {
                if (hit.collider != null)
                {
                    // Cambiar cámaras
                    firstPersonCamera.enabled = false;
                    mainCamera.enabled = true;

                    // Desactivar el controlador FPS
                    fpsController = firstPersonCamera.GetComponentInParent<FirstPersonAIO>();
                    if (fpsController != null)
                    {
                        fpsController.enabled = false;
                    }

                    UnityEngine.Cursor.lockState = CursorLockMode.None;
                    UnityEngine.Cursor.visible = true;
                    // Desactiva el crosshair
                    fpsController.autoCrosshair = false;

                    // Si ya está creado, destruye el objeto del crosshair
                    if (fpsController.Crosshair != null)
                    {
                        // Busca el Canvas generado automáticamente
                        Canvas crosshairCanvas = fpsController.playerCamera.GetComponentInChildren<Canvas>();
                        if (crosshairCanvas != null)
                        {
                           crosshairCanvas.enabled = false;
                        }
                    }

                    
                    levelSelector.enabled = true;
                    selector.enabled = true;
                    selector.resetPosition();

                }
            }
        }
    }
}