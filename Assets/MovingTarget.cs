using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;
    [SerializeField] float _speed = 2f;

    private bool movingToA = false;

    void Update()
    {
        // Move toward the target point
        Transform target = movingToA ? pointA : pointB;
        transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pointA"))
        {
            movingToA = false;
        }
        else if (other.CompareTag("pointB"))
        {
            movingToA = true;
        }
    }
}

