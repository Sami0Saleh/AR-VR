using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [SerializeField] Transform pointA;
    [SerializeField] Transform pointB;

    [SerializeField] float _speed;

    public bool movingToA;
    void Update()
    {
        if (movingToA)
        {
            transform.Translate(pointA.position * Time.deltaTime * _speed);
        }
        if (!movingToA)
        {
            transform.Translate(pointB.position * Time.deltaTime * _speed);
        }
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
