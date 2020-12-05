using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateNames : MonoBehaviour
{
    public TextMesh x;
    List<TextMesh> texts;

    // Update is called once per frame

    private void Start()
    {
        texts = new List<TextMesh>(20);
        for (int i = 0; i < 20; i++)
        {
            texts.Add(Instantiate(x));

        }
    }

    void Update()
    {
        int index = 0;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("player"))
        {
            texts[index].transform.position = player.transform.position + new Vector3(0, 4, 0);
            texts[index].text = player.GetComponent<Dissonance.Integrations.MirrorIgnorance.Demo.MirrorIgnorancePlayerController>().namePlayer;
            index++;
            if (index > 20)
            {
                break;
            }
        }
        for (; index < 20; index++)
        {
            texts[index].transform.position = x.transform.position;
        }
    }
}
