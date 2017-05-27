using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CurrencyCounterUI : MonoBehaviour {

    [SerializeField]
    private Text currencyText;

    // Use this for initialization
    void Awake() {
        currencyText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        currencyText.text = "ALIEN BUCKS: " + GameMaster.currency.ToString();
    }
}
