using UnityEngine;
using System.Collections.Generic;

public class SmoothCameraLookAt : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private List<Transform> cameraPoints;  // Puntos de movimiento.
    [SerializeField] private float moveSpeed = 5f;         // Velocidad base.
    [SerializeField] private float rotationSpeed = 3f;     // Velocidad de rotación.
    [SerializeField] private float smoothTime = 0.3f;      // Tiempo de suavizado (para SmoothDamp).

    [Header("Look At Target")]
    [SerializeField] private Transform lookAtTarget;       // Objetivo a mirar.
    [SerializeField] private AnimationCurve moveCurve;     // Curva de aceleración/desaceleración.

    private int currentIndex = 0;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;                       // Para SmoothDamp.
    private float rotationVelocity;                        // Para suavizar rotación.

    private void Start()
    {
        if (cameraPoints.Count > 0)
        {
            targetPosition = cameraPoints[currentIndex].position;
        }

        // Configuración inicial de la curva (si no está asignada).
        if (moveCurve == null || moveCurve.keys.Length == 0)
        {
            moveCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f); // Curva predeterminada suave.
        }
    }

    private void Update()
    {
        if (cameraPoints.Count == 0 || lookAtTarget == null) return;

        // Input: Teclas A/D.
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToPreviousPoint();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToNextPoint();
        }

        // Movimiento suavizado con SmoothDamp y curva de aceleración.
        float progress = Mathf.Clamp01(Vector3.Distance(transform.position, targetPosition) / 10f);
        float smoothedSpeed = moveSpeed * moveCurve.Evaluate(progress);

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            smoothTime,
            smoothedSpeed
        );

        // Rotación suavizada hacia el objetivo.
        Vector3 direction = (lookAtTarget.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float delta = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            delta
        );
    }

    private void MoveToNextPoint()
    {// Si NO está en el último punto, avanza.
        if (currentIndex < cameraPoints.Count - 1)
        {
            currentIndex++;
            targetPosition = cameraPoints[currentIndex].position;
            UpdateTargetPositionWithArc();
            ResetSmoothing();
        }
    }

    private void MoveToPreviousPoint()
    {
        // Si NO está en el primer punto, retrocede.
        if (currentIndex > 0)
        {
            currentIndex--;
            targetPosition = cameraPoints[currentIndex].position;
            UpdateTargetPositionWithArc();
            ResetSmoothing();
        }
    }

    private void ResetSmoothing()
    {
        currentVelocity = Vector3.zero; // Evita rebotes al cambiar de dirección.
    }

    private void UpdateTargetPositionWithArc()
    {
        Vector3 basePosition = cameraPoints[currentIndex].position;
        float arcHeight = 2f; // Altura del arco.
        float progress = Mathf.Clamp01(Vector3.Distance(transform.position, basePosition) / 10f);
        targetPosition = basePosition + Vector3.up * arcHeight * Mathf.Sin(progress * Mathf.PI);
    }
}