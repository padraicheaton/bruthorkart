using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static void PairCameraToImage(Camera cam, RawImage image, int width, int height)
    {
        RenderTexture renderTexture = new RenderTexture(width, height, 16);

        cam.targetTexture = renderTexture;
        image.texture = renderTexture;
    }

    public static void PairCameraToImage(Camera cam, RawImage image, float width, float height)
    {
        PairCameraToImage(cam, image, Mathf.RoundToInt(width), Mathf.RoundToInt(height));
    }

    public static void PairCameraToImage(Camera cam, RawImage image)
    {
        PairCameraToImage(cam, image, image.rectTransform.rect.width, image.rectTransform.rect.height);
    }
}
