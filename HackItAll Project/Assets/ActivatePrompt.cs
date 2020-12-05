using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePrompt : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            other.GetComponent<Dissonance.Integrations.MirrorIgnorance.Demo.MirrorIgnorancePlayerController>().setAble(true);
            Debug.Log(other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "player")
        {
            other.GetComponent<Dissonance.Integrations.MirrorIgnorance.Demo.MirrorIgnorancePlayerController>().setAble(false);
            Debug.Log(other.name);
        }
    }
}
