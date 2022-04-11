using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;

/// <summary>
/// Component to generate a FileChooser
/// </summary>
public class FileChooser : MonoBehaviour
{
    /// <summary>
    /// Delegate to referenced a method which take a string as parameter.
    /// </summary>
    /// <param name="path"></param>
    public delegate void OnClick(string path);

    /// <summary>
    /// The TreeView containing the file tree.
    /// </summary>
    public TreeView fileTree;

    /// <summary>
    /// The Content of the scroll view containing the list of file of the current directory.
    /// </summary>
    public GameObject fileList;

    /// <summary>
    /// The TreeItem prefab to instanciate.
    /// </summary>
    public GameObject treeItemPrefab;

    /// <summary>
    /// The FileItem prefab to instanciate.
    /// </summary>
    public GameObject fileItemPrefab;

    /// <summary>
    /// The InputField containing the path of the current directory.
    /// </summary>
    public InputField inputField;

    /// <summary>
    /// Method to be called by the FileChooser when clicking on a file.
    /// </summary>
    /// <param name="path"></param>
    private OnClick onClick;

    /// <summary>
    /// Static method to be called to instanciate a Canvas containing the FileChooser prefab, with the Component needed if no EventSystem are already in the Scene.
    /// </summary>
    /// <param name="onClick"></param>
    public static void ChooseFile(OnClick onClick)
    {
        GameObject fileChooserCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/FileChooserCanvas"));
        fileChooserCanvas.GetComponentInChildren<FileChooser>().onClick = onClick;
        if (!EventSystem.current)
        {
            fileChooserCanvas.AddComponent<EventSystem>();
            fileChooserCanvas.AddComponent<StandaloneInputModule>();
        }
        fileChooserCanvas.GetComponentInChildren<FileChooser>().CreateTree();

    }

    /// <summary>
    /// Method called at the end of ChooseFile, to generated the first nodes of the file tree and the drives directories in the File List by instanciating the prefabs. <br/>
    /// Adds listeners to the treeItems to add the subDirectories as children when the arrow is clicked (because building the entire tree takes too long), by calling <see cref="AddSubDirectories(DirectoryInfo, TreeItem)"/> 
    /// and show the directories and files in the selected folder in the FileList, by calling <see cref="OpenDirectory(DirectoryInfo)"/>. <br/>
    /// Adds listener to the FileItem in the FileList to show the directories and files in the selected folder in the FileList.<br/>
    /// Sets the text of the FileItems and TreeItems, as well as the icon of the FileItems.<br/>
    /// </summary>
    public void CreateTree()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        foreach (DriveInfo d in allDrives)
        {
            if (d.IsReady && IsAccessibleDirectory(d.RootDirectory))
            {
                Debug.Log("Drive "+ d.RootDirectory.FullName);
                GameObject node = Instantiate(treeItemPrefab);
                TreeItem treeItem =  node.GetComponent<TreeItem>();
                treeItem.Text = d.RootDirectory.FullName;
                fileTree.AddItem(treeItem);
                treeItem.arrow.GetComponent<Button>().onClick.AddListener(() => {
                    if(treeItem.children.transform.childCount == 0)
                        AddSubDirectories(d.RootDirectory, treeItem);
                });
                treeItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate {OpenDirectory(d.RootDirectory);});

                GameObject item = Instantiate(fileItemPrefab, fileList.transform);
                item.GetComponent<FileItem>().Icon = Resources.Load<Sprite>("Sprites/directory_icon");
                item.GetComponent<FileItem>().Text = d.RootDirectory.FullName;
                item.GetComponent<FileItem>().Event.AddListener(delegate { OpenDirectory(d.RootDirectory); });

            }
        }
    }

    /// <summary>
    /// Adds the accessible subdirectories in the TreeItem root as TreeItem child by instanciating the prefab.<br/>
    /// Adds listeners to the treeItems subdirectories to call this method when the arrow is clicked
    /// and show the directories and files in the selected folder in the FileList, by calling <see cref="OpenDirectory(DirectoryInfo)"/>.<br/>
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="root"></param>
    private void AddSubDirectories(DirectoryInfo directory, TreeItem root)
    {
        if (IsAccessibleDirectory(directory))
        {
            foreach (var subDirectory in directory.GetDirectories())
            {
                if (IsAccessibleDirectory(subDirectory))
                {
                    GameObject node = Instantiate(treeItemPrefab);
                    TreeItem treeItem = node.GetComponent<TreeItem>();
                    treeItem.Text = subDirectory.Name;
                    root.AddChild(treeItem);
                    treeItem.arrow.GetComponent<Button>().onClick.AddListener(() =>
                    {
                        AddSubDirectories(subDirectory, treeItem);
                    });
                    treeItem.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(delegate { OpenDirectory(subDirectory); });
                }
            }
        }
    }

    /// <summary>
    /// Clears the FileList then adds all the accessible subdirectories and the files contained in directory.
    /// Set the name and icon for each item.
    /// Adds a listener to the subdirectories item to call this method and a listener to the files item to call the <see cref="onClick"/> method.
    /// </summary>
    /// <param name="directory"></param>
    private void OpenDirectory(DirectoryInfo directory)
    {
        foreach (Transform child in fileList.transform)
        {
            Destroy(child.gameObject);
        }
        inputField.text = directory.FullName;
        DirectoryInfo[] directoryEntries = directory.GetDirectories();
        FileInfo[] fileEntries = directory.GetFiles();

        foreach (DirectoryInfo subDirectory in directoryEntries)
        {
            if (IsAccessibleDirectory(subDirectory))
            {
                GameObject item = Instantiate(fileItemPrefab, fileList.transform);
                item.GetComponent<FileItem>().Icon = Resources.Load<Sprite>("Sprites/directory_icon");
                item.GetComponent<FileItem>().Text = subDirectory.Name;
                item.GetComponent<FileItem>().Event.AddListener(delegate { OpenDirectory(subDirectory); });
            }
        }

        foreach (FileInfo file in fileEntries)
        {
            GameObject item = Instantiate(fileItemPrefab, fileList.transform);
            item.GetComponent<FileItem>().Icon = Resources.Load<Sprite>("Sprites/file_icon");
            item.GetComponent<FileItem>().Text = file.Name;
            item.GetComponent<FileItem>().Event.AddListener(() => onClick(file.FullName));
        }
    }

    /// <summary>
    /// Go to the parent directory of the current ones, if it exists, by calling <see cref="OpenDirectory(DirectoryInfo)"/>. Else it displays the drives.
    /// </summary>
    public void Return()
    {
        string current_directory = inputField.text;
        if (!String.IsNullOrWhiteSpace(current_directory))
        {
            foreach (Transform child in fileList.transform)
            {
                Destroy(child.gameObject);
            }
            DirectoryInfo parent_directory = Directory.GetParent(current_directory);
            if (parent_directory is object)
                OpenDirectory(parent_directory);
            else
            {
                inputField.text = "";
                DriveInfo[] allDrives = DriveInfo.GetDrives();

                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady)
                    {
                        GameObject item = Instantiate(fileItemPrefab, fileList.transform);
                        item.GetComponent<FileItem>().Icon = Resources.Load<Sprite>("Sprites/directory_icon");
                        item.GetComponent<FileItem>().Text = d.RootDirectory.FullName;
                        item.GetComponent<FileItem>().Event.AddListener(delegate { OpenDirectory(d.RootDirectory); });
                    }
                }
            }
        }
    }

    /// <summary>
    /// Quit the FileChooser by asking for the whole canvas to be destroyed.
    /// </summary>
    public void Quit()
    {
        Destroy(transform.parent.gameObject);
    }

    /// <summary>
    /// Check if a directory is accessible.
    /// </summary>
    /// <param name="directory"></param>
    /// <returns></returns>
    private bool IsAccessibleDirectory(DirectoryInfo directory)
    {
        try
        {
            directory.GetDirectories();
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        return true;
    }

}
