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
        CurrentPointsText.text = "" + cl.GetPoints();
        if(cl.GetSum() <= 0)
        {
            ComboSumText.text = "";
            ComboMultiplierText.text = "";
        }
        else
        {
            int index = Mathf.Clamp((int)cl.GetMultiplier(), 0, ce.comboColors.Length - 1);
            Color c = ce.comboColors[index];
            ComboMultiplierText.color = c;

            ComboSumText.text = "" + cl.GetSum();
            ComboMultiplierText.text = "x" + cl.GetMultiplier();
        }
	}

    public void IngredientsChanged(bool changed)
    {
        DrawIcons(cl.myComboList);
    }

    private void DrawIcons(List<EnemyData> list)
    {
        //clear panel
        for(int i = 0; i < icons.Count; i++)
        {
            Destroy(icons[i]);
        }
        icons.Clear();

        //populate panel
        for(int i = 0; i < list.Count; i++)
        {
            float horizontalPos = ComboListStart + i * 150;
            GameObject icon = Instantiate(list[i].Icon);
            icon.transform.SetParent(ComboPanel);
            RectTransform rt = icon.GetComponent<RectTransform>();
            rt.localPosition = new Vector3(horizontalPos, 0, 0);
            rt.localScale = new Vector3(1, 1, 1);

            icons.Add(icon);
        }
    }

    private List<GameObject> icons = new List<GameObject>();
}
