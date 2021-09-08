using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveRenderTexture : MonoBehaviour {
    public RenderTexture rt;
    private IEnumerator CaptureDepth() {
        RenderTexture tmp = RenderTexture.GetTemporary(rt.width, rt.height, 24, RenderTextureFormat.ARGB32);
        Graphics.Blit(rt, tmp);

        //Wait until the next frame
        yield return null;

        //Store the last active render texture and set our depth copy as active
        RenderTexture lastActive = RenderTexture.active;
        RenderTexture.active = tmp;

        //Copy the active render texture into a normal Texture2D
        //Unfortunately readpixels doesn't work with single channel formats, so ARGB32 will have to do
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        //Restore the active render texture and release our temporary tex
        RenderTexture.active = lastActive;
        RenderTexture.ReleaseTemporary(tmp);

        //Wait another frame
        yield return null;

        //Encode the texture data into .png formatted bytes
        byte[] data = tex.EncodeToJPG(100);

        //Wait another frame
        yield return null;

        //Write the texture to a file
        File.WriteAllBytes(Application.dataPath + "/depthTexture.jpg", data);
        print("saved");
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(CaptureDepth());
    }
}
