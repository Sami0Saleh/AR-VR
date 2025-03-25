using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ArrowSpawner : MonoBehaviour
{
    public GameObject arrow;
    public GameObject notch;

    private XRGrabInteractable _bow;
    private bool _arrowNotched = false;
    private GameObject _currentArrow = null;

    [SerializeField] private GameObject _ricochetArrow;
    [SerializeField] private GameObject _homingArrow;
    
    private void Start()
    {
        _bow = GetComponent<XRGrabInteractable>();
        PullBow.PullActionReleased += NotchEmpty;
    }

    private void OnDestroy()
    {
        PullBow.PullActionReleased -= NotchEmpty;
    }

    private void Update()
    {
        if (_bow.isSelected && _arrowNotched == false)
        {
            _arrowNotched = true;
            StartCoroutine("DelayedSpawn");
        }
        if (!_bow.isSelected && _currentArrow != null)
        {
            Destroy(_currentArrow);
            NotchEmpty(1f);
        }
    }


    private void NotchEmpty(float value)
    {
        _arrowNotched = false;
        _currentArrow = null;
    }

    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(1f);
        GameObject selectedArrow = SelectRandomArrow();
        _currentArrow = Instantiate(selectedArrow, notch.transform);
    }
    
    private GameObject SelectRandomArrow()
    {
        GameObject[] arrows = { arrow, _homingArrow, _ricochetArrow };
        return arrows[Random.Range(0, arrows.Length)];
    }
}

