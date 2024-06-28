using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplitScreenRend : MonoBehaviour
{
    private RawImage rawImage;
    private LayoutElement layoutElement;
    private PlayerConfiguration configuration;

    public void Setup(PlayerConfiguration playerConfiguration, int totalPlayers)
    {
        rawImage = GetComponent<RawImage>();
        layoutElement = GetComponent<LayoutElement>();
        configuration = playerConfiguration;

        // Setup size of split screen
        float width = Screen.currentResolution.width;
        float height = Screen.currentResolution.height;

        if (totalPlayers > 1)
            width /= 2f;
        if (totalPlayers > 2)
            height /= 2f;

        float borderWidth = totalPlayers == 1 ? 0f : 10f;

        layoutElement.minWidth = width - borderWidth;
        layoutElement.minHeight = height - borderWidth;

        // Pair camera to this image
        Extensions.PairCameraToImage(playerConfiguration.Camera, rawImage, width, height);
    }
}
