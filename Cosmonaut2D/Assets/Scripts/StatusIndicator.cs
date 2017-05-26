using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]//show in the unity editor
    private RectTransform healthBarRect;

    [SerializeField]//show in the unity editor
    private Text healthText;

    void Start() {
        if(healthBarRect == null) {
            Debug.LogError("STATUS INDICATOR: No healthBarRect ref");
        }
        if(healthText == null) {
            Debug.LogError("STATUS INDICATOR: No healthText ref");
        }
    }

    public void setHealth(int _cur, int _max) {
        //calculate percentage of max health
        float _value = (float)_cur / _max;

        //TODO: change color of image component - equal to a gradient

        healthBarRect.localScale = new Vector3(_value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = (_cur + "/" + _max + " HP");
    }

}
