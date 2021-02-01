using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    [SerializeField] private Text idText;
    [SerializeField] private Text valueText;
    [SerializeField] private Text dateText;

    public void SetupScore(int id, uint value, string date)
    {
        idText.text = id + ":";
        valueText.text = value.ToString();
        dateText.text = date;
    }
}
