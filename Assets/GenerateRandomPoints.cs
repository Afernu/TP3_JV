using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateRandomPoints : MonoBehaviour
{
    public int numberOfPoints = 10;
    public float radius = 10f;

    void Awake()
    {
        GenerateRandomPatrolPoints();
    }

    void GenerateRandomPatrolPoints()
    {
        List<Vector3> validPoints = new List<Vector3>();

        int maxAttempts = numberOfPoints * 10;

        for (int i = 0; i < maxAttempts && validPoints.Count < numberOfPoints; i++)
        {
            Vector3 randomPoint = GetRandomPointInRadius(transform.position, radius);

            if (IsPointInNavMesh(randomPoint) && !IsTooCloseToExistingPoints(randomPoint, validPoints))
            {
                validPoints.Add(randomPoint);
            }
        }

        for (int i = 0; i < validPoints.Count; i++)
        {
            CreatePatrolPoint(validPoints[i], i);
        }
    }

    bool IsTooCloseToExistingPoints(Vector3 point, List<Vector3> existingPoints)
    {
        foreach (Vector3 existingPoint in existingPoints)
        {
            if (Vector3.Distance(point, existingPoint) < 2.0f)
            {
                return true;
            }
        }
        return false;
    }

    Vector3 GetRandomPointInRadius(Vector3 center, float r)
    {
        Vector2 randomPoint = Random.insideUnitCircle * r;
        return center + new Vector3(randomPoint.x, 0f, randomPoint.y);
    }

    bool IsPointInNavMesh(Vector3 point)
    {
        return NavMesh.SamplePosition(point, out _, 1.0f, NavMesh.AllAreas);
    }

    void CreatePatrolPoint(Vector3 position, int index)
    {
        GameObject patrolPoint = new GameObject("PatrolPoint " + index);
        patrolPoint.transform.position = position;
        patrolPoint.transform.parent = transform;
    }
}
