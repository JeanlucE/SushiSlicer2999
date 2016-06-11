using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ScoreCanvas : MonoBehaviour {
    public Text CurrentPointsText;
    public Text ComboSumText;
    public Text ComboMultiplierText;
    public RectTransform ComboPanel;
    public float ComboListStart;

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

        List<EnemyData> myIngredients = cl.myComboList;

        if(ingredientsChanged)
        {
            DrawIcons(myIngredients);
            ingredientsChanged = false;
        }
	}

    private bool ingredientsChanged = false;
    public void IngredientsChanged(bool changed)
    {
        ingredientsChanged = changed;
    }

    private void DrawIcons(List<EnemyData> list)
    {
        
    }
}
