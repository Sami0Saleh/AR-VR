using System.Collections;
using UnityEngine;

public class HomingArrow : MonoBehaviour
{
    public float speed = 10f;
    public Transform tip;
    public float homingStrength = 20f; // Increased from 5f
    public float homingDuration = 3f;  // Increased from 2f

    private Rigidbody _rigidBody;
    private bool _inAir = false;
    private Vector3 _lastPosition = Vector3.zero;
    private float _homingTime = 0f;
    private Transform _target;

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

        _target = FindClosestTarget(); // Assign target when fired

        Vector3 force = transform.forward * value * speed;
        _rigidBody.velocity = force;
        _rigidBody.angularVelocity = Vector3.zero;

        StartCoroutine(RotateWithVelocity());
        _lastPosition = tip.position;
    }

    private IEnumerator RotateWithVelocity()
    {
        yield return new WaitForFixedUpdate();
        while (_inAir)
        {
            if (_target != null && _homingTime < homingDuration)
            {
                Vector3 direction = (_target.position - transform.position).normalized;
                _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, direction * speed, Time.deltaTime * homingStrength);
                transform.rotation = Quaternion.LookRotation(_rigidBody.velocity, Vector3.up);
                _homingTime += Time.deltaTime;
            }
            else if (_rigidBody.velocity.sqrMagnitude > 0.1f)
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
                Stick(hitInfo);
            }
        }
    }

    private void Stick(RaycastHit hitInfo)
    {
        _inAir = false;
        SetPhysics(false);
        transform.position = hitInfo.point;
        transform.parent = hitInfo.transform;
    }

    private Transform FindClosestTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 40f); // Increased from 10f
        Transform closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Target"))
            {
                float distance = Vector3.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = col.transform;
                }
            }
        }

        if (closestTarget != null)
            Debug.Log("Target Acquired: " + closestTarget.name);
        else
            Debug.Log("No Target Found");

        return closestTarget;
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
