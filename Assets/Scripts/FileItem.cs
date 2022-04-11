using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FileItem : MonoBehaviour
{
    public Sprite Icon
    {
        get
        {
            return transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite;
        }
        set
        {
            transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = value;
        }
    }

    public string Text
    {
        get
        {
            return transform.GetChild(1).GetComponent<Text>().text;
        }
        set
        {
            transform.GetChild(1).GetComponent<Text>().text = value;
        }
    }

    public UnityEngine.UI.Button.ButtonClickedEvent Event
    {
        get
        {
            return GetComponent<UnityEngine.UI.Button>().onClick;
        }
    }

}
