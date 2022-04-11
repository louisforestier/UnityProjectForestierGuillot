using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CelestialBody : MonoBehaviour
{
    /// <summary>
    /// Speed of the rotation
    /// </summary>
    public float speed = 5;

    /// <summary>
    /// True if the object should rotate, false if you want it to be still.
    /// </summary>
    public bool mustRotate = true;

    /// <summary>
    /// Rotate the Celestial body on the y axis
    /// </summary>
    private void FixedUpdate()
    {
        if (mustRotate)
        {
            transform.Rotate(0, speed * Time.fixedDeltaTime, 0);
        }
    }

    /// <summary>
    /// Toggle the rotation.
    /// </summary>
    /// <param name="animate"></param>
    public void ToggleAnimation(bool animate)
    {
        mustRotate = animate;
    }

    /// <summary>
    /// Change the rotation speed.
    /// </summary>
    /// <param name="speed"></param>
    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
    }

    /// <summary>
    /// Call the <see cref="FileBrowser.ChooseFile(FileBrowser.OnClick)"/> method with <see cref="ChangeTexture(string)"/> to modify the Texture at runtime.
    /// </summary>
    public void ChooseTextureFile()
    {
        FileBrowser.ChooseFile(ChangeTexture);
    }

    /// <summary>
    /// If the path corresponds to a file with a .jpg extension, it changes the texture of the object.
    /// </summary>
    /// <param name="path"></param>
    private void ChangeTexture(string path)
    {
        string extension = Path.GetExtension(path).ToLowerInvariant();
        if (String.Equals(extension, ".jpg"))
        {
            byte[] bytes = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(900, 900);
            texture.LoadImage(bytes);
            texture.Apply();
            GetComponent<Renderer>().material.mainTexture = texture;
        }
    }
}
