using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothCameraLookAt : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private List<Transform> cameraPoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private AnimationCurve moveCurve;

    [Header("Look At Target")]
    [SerializeField] private Transform lookAtTarget;

    // Evento para notificar el cambio de nivel
    public delegate void LevelChangedHandler(int newLevelIndex);
    public static event LevelChangedHandler OnLevelChanged;

    private int currentIndex = 0;
    private Vector3 targetPosition;
    private Vector3 currentVelocity;
    private bool isMoving = false;
    private bool inputCooldown = false;


    private float movementStartTime; // Tiempo en que comenzó el movimiento
    private float estimatedMoveDuration = 1.0f; // Duración estimada del movimiento

    private void Start()
    {
        if (cameraPoints.Count > 0)
        {
            targetPosition = cameraPoints[currentIndex].position;
            OnLevelChanged?.Invoke(currentIndex); // Notificar nivel inicial
        }
    }

    private void Update()
    {
        if (cameraPoints.Count == 0 || lookAtTarget == null) return;

        // Solo procesar input si no hay cooldown
        if (!inputCooldown)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveToPreviousPoint();
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveToNextPoint();
            }
        }

      
        if (isMoving)
        {
            float progress = (Time.time - movementStartTime) / estimatedMoveDuration;
            progress = Mathf.Clamp01(progress);

            transform.position = Vector3.Lerp(
                transform.position,
                targetPosition,
                moveCurve.Evaluate(progress) * Time.deltaTime * moveSpeed
            );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(lookAtTarget.position - transform.position),
                rotationSpeed * Time.deltaTime
            );

            // Finaliza movimiento si está muy cerca Y ha pasado el tiempo mínimo
            if (progress >= 0.95f || Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                transform.position = targetPosition; // Asegura posición exacta
            }
        }
    }

    private void MoveToNextPoint()
    {
        if (currentIndex < cameraPoints.Count - 1)
        {
            currentIndex++;
            StartMovement();
        }
    }

    private void MoveToPreviousPoint()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            StartMovement();
        }
    }

    private void StartMovement()
    {
        targetPosition = cameraPoints[currentIndex].position;
        movementStartTime = Time.time;
        isMoving = true;
        inputCooldown = true;
        OnLevelChanged?.Invoke(currentIndex);

        // Cooldown comienza INMEDIATAMENTE al mover
        StartCoroutine(InputCooldown());
    }

    private IEnumerator InputCooldown()
    {
        // Cooldown total = duración movimiento (1s) + tiempo extra (0.3s)
        yield return new WaitForSeconds(estimatedMoveDuration + smoothTime);
        inputCooldown = false;
    }
}