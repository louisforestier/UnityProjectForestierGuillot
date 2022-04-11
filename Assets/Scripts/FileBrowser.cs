using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.IO;

public class FileBrowser : MonoBehaviour
{
    public delegate void OnClick(string path);

    public TreeView fileTree;
    public GameObject fileList;
    public GameObject treeItemPrefab;
    public GameObject fileItemPrefab;
    public InputField inputField;


    private OnClick onClick;

    public static void ChooseFile(OnClick onClick)
    {
        GameObject fileBrowserCanvas = Instantiate(Resources.Load<GameObject>("Prefabs/FileBrowserCanvas"));
        fileBrowserCanvas.GetComponentInChildren<FileBrowser>().onClick = onClick;
        if (!EventSystem.current)
        {
            fileBrowserCanvas.AddComponent<EventSystem>();
            fileBrowserCanvas.AddComponent<StandaloneInputModule>();
        }
        fileBrowserCanvas.GetComponentInChildren<FileBrowser>().CreateTree();

    }

    private void CreateTree()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        foreach (DriveInfo d in allDrives)
        {
            if (d.IsReady)
            {
                Debug.Log("Drive "+ d.RootDirectory.FullName);
                GameObject node = Instantiate(treeItemPrefab);
                TreeItem treeItem =  node.GetComponent<TreeItem>();
                treeItem.setText(d.RootDirectory.FullName);
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
                    treeItem.setText(subDirectory.Name);
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

    public void Quit()
    {
        GameObject canvas = transform.parent.gameObject;
        Destroy(canvas);
    }

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
