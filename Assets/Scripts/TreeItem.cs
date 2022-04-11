using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TreeItem : MonoBehaviour
{
    public GameObject children;
    public GameObject arrow;
    public Text textComponent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddChild(TreeItem treeItem)
    {
        Debug.Log("Add child in tree item");

        treeItem.transform.SetParent(children.transform,false);
    }

    public void RemoveChild(int i)
    {
        if (i < children.transform.childCount)
            children.transform.GetChild(i).parent = null;
        else throw new Exception("This child doesn't exist.");
    }

    public void setText(string text)
    {
        textComponent.text = text;
    }

    public void RemoveAndDestroyChild(int i)
    {
        if (i < children.transform.childCount)
            Destroy(children.transform.GetChild(i));
        else throw new Exception("This child doesn't exist.");
    }

    public TreeItem GetChild(int i)
    {
        if (i < children.transform.childCount)
            return children.transform.GetChild(i).GetComponent<TreeItem>();
        else throw new Exception("This child doesn't exist.");
    }

    public void ChildrenVisible()
    {
        if (children.activeSelf)
            arrow.transform.rotation = new Quaternion(0, 0, 0.7f,0.7f);
        else arrow.transform.rotation = Quaternion.identity;
        Debug.Log(arrow.transform.rotation);
        children.SetActive(!children.activeSelf);
    }

}
