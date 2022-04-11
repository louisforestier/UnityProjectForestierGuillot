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

    /// <summary>
    /// Property to get and set the text of the TreeItem.
    /// </summary>
    public string Text
    {
        get => textComponent.text;
        set => textComponent.text = value;
    }

    /// <summary>
    /// Add a child to this TreeItem.
    /// </summary>
    /// <param name="treeItem"></param>
    public void AddChild(TreeItem treeItem)
    {
        treeItem.transform.SetParent(children.transform,false);
    }

    /// <summary>
    /// Remove the child at the index i. The GameObject is not destroyed.
    /// </summary>
    /// <param name="i"></param>
    public void RemoveChild(int i)
    {
        if (i < children.transform.childCount)
            children.transform.GetChild(i).parent = null;
        else throw new IndexOutOfRangeException("This child doesn't exist.");
    }

    /// <summary>
    /// Remove and destroy the child at index i.
    /// </summary>
    /// <param name="i"></param>
    public void RemoveAndDestroyChild(int i)
    {
        if (i < children.transform.childCount)
            Destroy(children.transform.GetChild(i));
        else throw new IndexOutOfRangeException("This child doesn't exist.");
    }

    /// <summary>
    /// Get the child at index i.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public TreeItem GetChild(int i)
    {
        if (i < children.transform.childCount)
            return children.transform.GetChild(i).GetComponent<TreeItem>();
        else throw new IndexOutOfRangeException("This child doesn't exist.");
    }

    /// <summary>
    /// Show/Hide the children and rotate the arrow accordingly.
    /// </summary>
    public void ChildrenVisible()
    {
        if (children.activeSelf)
            arrow.transform.rotation = new Quaternion(0, 0, 0.7f,0.7f);
        else arrow.transform.rotation = Quaternion.identity;
        children.SetActive(!children.activeSelf);
    }

    /// <summary>
    /// Return the number of children.
    /// </summary>
    /// <returns> number of children</returns>
    public int ChildrenCount()
    {
        return children.transform.childCount;
    }

}
