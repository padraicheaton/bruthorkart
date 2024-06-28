using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultsDataRow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI pointsTxt;

    public void Setup(string name, int points)
    {
        nameTxt.text = name;
        pointsTxt.text = points.ToString();
    }
}
