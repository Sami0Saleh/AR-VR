using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetArrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;
    public int maxRicochets = 3; // Number of allowed bounces
    public float ricochetDamping = 0.7f; // Speed reduction on each bounce

    private Rigidbody _rigidBody;
    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;
    private int _ricochetCount = 0;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        PullBow.PullActionReleased += Release;
        Stop();
    }

    private void OnDestroy()
    {
        PullBow.PullActionReleased -= Release;
    }

    private void Release(float value)
    {
        PullBow.PullActionReleased -= Release;
        gameObject.transform.parent = null;
        _inAir = true;
        SetPhysics(true);

        Vector3 force = transform.forward * value * speed;
        _rigidBody.velocity = force; // Ensure velocity is applied
        _rigidBody.angularVelocity = Vector3.zero; // Reset unwanted rotation

        StartCoroutine(RotateWithVelocity());

        _lastPosition = tip.position;
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir)
        {
            if (_rigidBody.velocity.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(_rigidBody.velocity, Vector3.up);
            }
            yield return null;
        }
    }

    private void FixedUpdate()
    {
        if (_inAir)
        {
            CheckCollision();
            _lastPosition = tip.position;
        }
    }

    private void CheckCollision()
    {
        if (Physics.Linecast(_lastPosition, tip.position, out RaycastHit hitInfo))
        {
            if (hitInfo.transform.gameObject.layer != 8)
            {
                if (_ricochetCount < maxRicochets)
                {
                    Ricochet(hitInfo);
                }
                else
                {
                    Stick(hitInfo);
                }
            }
        }
    }

    private void Ricochet(RaycastHit hitInfo)
    {
        _ricochetCount++;
        Vector3 reflectDir = Vector3.Reflect(_rigidBody.velocity.normalized, hitInfo.normal);
        _rigidBody.velocity = reflectDir * _rigidBody.velocity.magnitude * ricochetDamping;
    }

    private void Stick(RaycastHit hitInfo)
    {
        _inAir = false;
        SetPhysics(false);
        transform.position = hitInfo.point;
        transform.parent = hitInfo.transform;
    }

    private void Stop()
    {
        _inAir = false;
        SetPhysics(false);
    }

    private void SetPhysics(bool usePhysics)
    {
        _rigidBody.useGravity = usePhysics;
        _rigidBody.isKinematic = !usePhysics;
    }
}
