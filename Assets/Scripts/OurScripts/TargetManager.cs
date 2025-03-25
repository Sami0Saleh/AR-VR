using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private GameObject lvl1;
    [SerializeField] private GameObject lvl2;
    [SerializeField] private GameObject lvl3;

    private int lvl1Destroyed = 0;
    private int lvl2Destroyed = 0;

    private void Start()
    {
        lvl2.SetActive(false);
        lvl3.SetActive(false);
    }

    public void TargetDestroyed(GameObject target)
    {
        if (target.transform.parent == lvl1.transform)
        {
            lvl1Destroyed++;
            if (lvl1Destroyed >= lvl1.transform.childCount)
            {
                lvl2.SetActive(true);
            }
        }
        else if (target.transform.parent == lvl2.transform)
        {
            lvl2Destroyed++;
            if (lvl2Destroyed >= lvl2.transform.childCount)
            {
                lvl3.SetActive(true);
            }
        }
    }
}