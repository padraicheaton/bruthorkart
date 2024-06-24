using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private InputSystemUIInputModule inputSystemUIInputModule;
    [SerializeField] private Button firstSelect;

    [Header("Preview References")]
    [SerializeField] private Camera carCam;
    [SerializeField] private GameObject spinnerObj;
    [SerializeField] private RawImage camPreview;

    private int carIndex = 0;

    public void SetupPlayer(PlayerConfiguration config)
    {
        config.Input.uiInputModule = inputSystemUIInputModule;
        SetPlayerIndex(config.PlayerIndex);

        RenderTexture rend = new RenderTexture(600, 300, 16);
        carCam.targetTexture = rend;
        camPreview.texture = rend;

        SetDisplayedCar(CarDatabase.Instance.First());
    }

    private void SetDisplayedCar(CarSettings car)
    {
        for (int i = spinnerObj.transform.childCount - 1; i >= 0; i--)
            Destroy(spinnerObj.transform.GetChild(i).gameObject);

        Instantiate(car.carModel, spinnerObj.transform);

        SetCar(car);
    }

    public void PageLeft()
    {
        carIndex = CarDatabase.Instance.Previous(carIndex);

        SetDisplayedCar(CarDatabase.Instance.Get(carIndex));
    }

    public void PageRight()
    {
        carIndex = CarDatabase.Instance.Next(carIndex);

        SetDisplayedCar(CarDatabase.Instance.Get(carIndex));
    }

    public void SetPlayerIndex(int playerIndex)
    {
        PlayerIndex = playerIndex;
        titleText.text = $"Player {PlayerIndex + 1}";
    }

    public void SetCar(CarSettings carSettings)
    {
        PlayerConfigurationManager.Instance.SetPlayerCar(PlayerIndex, carSettings);
    }

    public void ReadyPlayer()
    {
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);

        readyPanel.SetActive(true);
    }
}
