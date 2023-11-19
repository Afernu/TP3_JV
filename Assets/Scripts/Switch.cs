using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;


public class Switch : MonoBehaviour, IInteractable
{
    private GameObject lever;
    private Quaternion targetRotation;
    private bool isRotating = false;
    private bool isUp = true;

    [SerializeField] private float rotationAngle = -90;
    [SerializeField] private float rotationSpeed = 8.0f;

    [SerializeField] private Transform arch;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float archMoveDuration = 2.0f;

    [SerializeField] private NavMeshSurface surface;

    void Start()
    {
        initialPosition = arch.position;
        targetPosition = initialPosition + Vector3.down * 5;
        lever = transform.Find("Lever").gameObject;
    }

    void Update()
    {
        if (isRotating)
        {
            lever.transform.rotation = Quaternion.Lerp(lever.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(lever.transform.rotation, targetRotation) < 0.1f)
            {
                lever.transform.rotation = targetRotation;
                isRotating = false;
                StartCoroutine(MoveArch());
            }
        }
    }

    public void Interact()
    {
        if (!isRotating)
        {
            isUp = !isUp;
            targetRotation = Quaternion.Euler(lever.transform.rotation.eulerAngles + Vector3.forward * (isUp ? rotationAngle : -rotationAngle));
            isRotating = true;
        }
    }

    IEnumerator MoveArch()
    {
        float elapsedTime = 0;
        Vector3 start = arch.position;
        Vector3 end = isUp ? initialPosition : targetPosition;

        while (elapsedTime < archMoveDuration)
        {
            arch.position = Vector3.Lerp(start, end, elapsedTime / archMoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        arch.position = end;
        //surface.BuildNavMesh();
    }

}
