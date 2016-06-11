using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreCanvas : MonoBehaviour {
    public Text CurrentPointsText;
    public Text ComboSumText;
    public Text ComboMultiplierText;

    private ComboList cl;
    private ComboEffect ce;
	// Use this for initialization
	void Start () {
        cl = PlayerController.main.gameObject.GetComponent<ComboList>();
        ce = PlayerController.main.gameObject.GetComponentInChildren<ComboEffect>();
    }
	
	// Update is called once per frame
	void Update () {
        CurrentPointsText.text = "Points: " + cl.GetPoints();
        if(cl.GetSum() <= 0)
        {
            ComboSumText.text = "";
            ComboMultiplierText.text = "";
        }
        else
        {
            int index = Mathf.Clamp((int)cl.GetMultiplier(), 0, ce.comboColors.Length);
            Color c = ce.comboColors[index];
            ComboMultiplierText.color = c;

            ComboSumText.text = "" + cl.GetSum();
            ComboMultiplierText.text = "x" + cl.GetMultiplier();
        }
	}
}
