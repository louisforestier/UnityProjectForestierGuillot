using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
