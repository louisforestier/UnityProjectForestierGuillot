using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FileItem : MonoBehaviour
{
    /// <summary>
    /// Property to get/set the sprite of the FileItem.
    /// </summary>
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

    /// <summary>
    /// Property to get/set the text of the FileItem.
    /// </summary>
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

    /// <summary>
    /// Propery to get the onClick event of the FileItem to add listeners.
    /// </summary>
    public UnityEngine.UI.Button.ButtonClickedEvent Event
    {
        get
        {
            return GetComponent<UnityEngine.UI.Button>().onClick;
        }
    }

}
