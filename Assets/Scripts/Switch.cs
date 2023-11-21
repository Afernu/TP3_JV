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
    [SerializeField] private float rotationSpeed = 1.0f;

    [SerializeField] private Transform obj;
    [SerializeField] private Vector3 initialPosition;
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float moveDuration = 2.0f;

    [SerializeField] private NavMeshSurface surface;

    void Start()
    {
        initialPosition = obj.position;
        targetPosition = new Vector3(initialPosition.x, initialPosition.y, initialPosition.z - 4);
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
                StartCoroutine(MoveObject());
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

    IEnumerator MoveObject()
    {
        float elapsedTime = 0;
        Vector3 start = obj.position;
        Vector3 end = isUp ? initialPosition : targetPosition;

        while (elapsedTime < moveDuration)
        {
            obj.position = Vector3.Lerp(start, end, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.position = end;
        surface.BuildNavMesh();
    }

}
