using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeView : MonoBehaviour
{
    public GameObject root;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(TreeItem treeItem)
    {
        treeItem.transform.SetParent(root.transform,false);
    }
}
