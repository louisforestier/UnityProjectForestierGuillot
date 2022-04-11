using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class TreeView : MonoBehaviour
{
    /// <summary>
    /// Root element of the TreeView.
    /// </summary>
    public GameObject root;

    /// <summary>
    /// Add a TreeItem in the TreeView by setting the TreeItem gameobject transform as a child of the root transform.
    /// </summary>
    /// <param name="treeItem"></param>
    public void AddItem(TreeItem treeItem)
    {
        treeItem.transform.SetParent(root.transform,false);
    }

    /// <summary>
    /// Remove the child at the index i. The GameObject is not destroyed.
    /// </summary>
    /// <param name="i"></param>
    public void RemoveChild(int i)
    {
        if (i < root.transform.childCount)
            root.transform.GetChild(i).parent = null;
        else throw new IndexOutOfRangeException("This child doesn't exist.");
    }

    /// <summary>
    /// Remove and destroy the child at index i.
    /// </summary>
    /// <param name="i"></param>
    public void RemoveAndDestroyChild(int i)
    {
        if (i < root.transform.childCount)
            Destroy(root.transform.GetChild(i));
        else throw new IndexOutOfRangeException("This child doesn't exist.");
    }

    /// <summary>
    /// Get the child at index i.
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public TreeItem GetChild(int i)
    {
        if (i < root.transform.childCount)
            return root.transform.GetChild(i).GetComponent<TreeItem>();
        else throw new IndexOutOfRangeException("This child doesn't exist.");
    }

    /// <summary>
    /// Return the number of children.
    /// </summary>
    /// <returns> number of children</returns>
    public int ChildrenCount()
    {
        return root.transform.childCount;
    }

}
