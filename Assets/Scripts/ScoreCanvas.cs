using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreCanvas : MonoBehaviour {
    public Text CurrentPointsText;
    public Text ComboSumText;
    public Text ComboMultiplierText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        ComboList cl = PlayerController.main.gameObject.GetComponent<ComboList>();

        CurrentPointsText.text = "Points: " + cl.GetPoints();
        if(cl.GetSum() <= 0)
        {
            ComboSumText.text = "";
            ComboMultiplierText.text = "";
        }
        else
        {
            ComboSumText.text = "" + cl.GetSum();
            ComboMultiplierText.text = "x" + cl.GetMultiplier();
        }
        
	}
}
