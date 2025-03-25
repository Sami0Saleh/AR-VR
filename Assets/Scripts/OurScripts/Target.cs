using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private TargetManager targetManager;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            targetManager.TargetDestroyed(gameObject);
            Destroy(gameObject);
            TargetCounter.instance.AddTargetHit();
        }
    }
}