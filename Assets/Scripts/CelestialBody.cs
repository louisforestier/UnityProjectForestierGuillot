using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CelestialBody : MonoBehaviour
{
    public float speed = 5;
    public bool mustRotate = true;
    // Start is called before the first frame update


    private void FixedUpdate()
    {
        if (mustRotate)
        {
            transform.Rotate(0, speed * Time.fixedDeltaTime, 0);
        }
    }

    public void ToggleAnimation(bool animate)
    {
        mustRotate = animate;
    }

    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
    }

    public void ChooseTextureFile()
    {
        FileBrowser.ChooseFile(ChangeTexture);
    }

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
