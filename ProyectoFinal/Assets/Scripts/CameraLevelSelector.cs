using UnityEngine;
using System.Collections.Generic;

public class CameraLevelSelector : MonoBehaviour
{
    [SerializeField] private List<Transform> levelTransforms; // Lista de posiciones de tus tartas/niveles.
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float zoomSpeed = 2f;
    [SerializeField] private float zoomDistance = 3f; // Distancia adicional al acercarse.

    private int currentIndex = 0;
    private Vector3 targetPosition;
    private float originalDistance;

    private void Start()
    {
        if (levelTransforms.Count > 0)
        {
            originalDistance = Vector3.Distance(transform.position, levelTransforms[currentIndex].position);
            UpdateTargetPosition();
        }
    }

    private void Update()
    {
        if (levelTransforms.Count == 0) return;

        // Input: Teclas A/D o LeftArrow/RightArrow.
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToPrevious();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToNext();
        }

        // Movimiento suave hacia el objetivo.
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Zoom suave hacia el objetivo.
        Vector3 directionToTarget = (levelTransforms[currentIndex].position - transform.position).normalized;
        float targetDistance = originalDistance - zoomDistance;
        transform.position = Vector3.Lerp(
            transform.position,
            levelTransforms[currentIndex].position - directionToTarget * targetDistance,
            zoomSpeed * Time.deltaTime
        );
    }

    private void MoveToNext()
    {
        currentIndex = (currentIndex + 1) % levelTransforms.Count;
        UpdateTargetPosition();
    }

    private void MoveToPrevious()
    {
        currentIndex = (currentIndex - 1 + levelTransforms.Count) % levelTransforms.Count;
        UpdateTargetPosition();
    }

    private void UpdateTargetPosition()
    {
        if (levelTransforms.Count > 0)
        {
            // Posición objetivo detrás del objeto actual, manteniendo la altura/orientación original.
            Vector3 offset = transform.position - levelTransforms[currentIndex].position;
            offset.y = transform.position.y; // Mantén la altura de la cámara.
            targetPosition = levelTransforms[currentIndex].position + offset.normalized * originalDistance;
        }
    }
}