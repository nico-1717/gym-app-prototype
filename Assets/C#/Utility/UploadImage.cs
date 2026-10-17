using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UploadImage : MonoBehaviour
{
    public void PickImage(Image Immagine)
    {
        int maxSize = 1024;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                byte[] imgByteArray = texture.EncodeToJPG();
                Globals.IMG_BYTE_INSERT = imgByteArray;

                //// Assign texture to a temporary quad and destroy it after 5 seconds
                //GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                //quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                //quad.transform.forward = Camera.main.transform.forward;
                //quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                //Material material = quad.GetComponent<Renderer>().material;
                //if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                //    material.shader = Shader.Find("Legacy Shaders/Diffuse");

                //material.mainTexture = texture;

                Sprite mySprite;
                mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                Immagine.sprite = mySprite; // apply the new sprite to the image
                Immagine.preserveAspect = true;

                Destroy(mySprite, 5f);

                // If a procedural texture is not destroyed manually, 
                // it will only be freed after a scene change
            }
        }, "Select a PNG image", "image/png");
    }

    public Image ConvertByteArrayToImage(byte[] byteArrayToConvert)
    {
        if (!gameObject.GetComponent<Image>())
            gameObject.AddComponent<Image>();
        Image m_Image = gameObject.GetComponent<Image>();
        Texture2D tex = new Texture2D(100, 100);
        tex.LoadImage(byteArrayToConvert);
        Sprite mySprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2());//set the Rect with
        m_Image.sprite = mySprite;
        Destroy(mySprite, 5f);
        Destroy(m_Image, 5f);

        return m_Image;
    }
}
