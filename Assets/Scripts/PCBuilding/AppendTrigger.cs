using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppendTrigger : MonoBehaviour
{
    [SerializeField] private PCComponent _component;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);

        Motherboard motherboard = other.GetComponentInChildren<Motherboard>();

        if(motherboard != null)
        {
            motherboard.TryAppend(_component);
            gameObject.SetActive(false);
        }
    }
}
