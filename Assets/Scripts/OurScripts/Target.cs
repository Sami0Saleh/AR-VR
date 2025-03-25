using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            Destroy(gameObject);
            TargetCounter.instance.AddTargetHit();
        }
    }
}