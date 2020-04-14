using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GenericTrigger : MonoBehaviour
{
    [SerializeField] UnityEvent[] events;

    private void Start()
    {
        if (GetComponent<MeshRenderer>() != null)
            GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            // [!] Run through each event and invoke it, [~] The event is called _event because without the underscore it's treated as a system variable or something
            foreach (UnityEvent _event in events)
                _event.Invoke();
            Destroy(this);
        }
    }
}
