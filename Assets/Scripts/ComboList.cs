using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ComboList : MonoBehaviour
{
    public static ComboList Instance;
   
    public void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public float CurrentComboSum = 0;
    public float CurrentComboMultiplier = 1;

    public List<EnemyData> myComboList = new List<EnemyData>();

    public List<Recipe> recipes = new List<Recipe>();
    [System.Serializable]
    public class Recipe
    {
        public EnemyData[] Ingredients;
        public float PointScore
        {
            get
            {
                float sum = 0;
                foreach (EnemyData e in Ingredients)
                {
                    sum += e.score;
                }

                return sum;
            }
        }

        private bool IsContainedIn(List<EnemyData> list)
        {
            if (list == null || Ingredients == null || Ingredients.Length > list.Count)
            {
                return false;
            }

            foreach (EnemyData e in Ingredients)
            {
                if (list.Contains(e))
                {
                    list.Remove(e);
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        //checks if recipe is in list fully and removes it, order is not important
        public bool RemoveFrom(List<EnemyData> list)
        {
            List<EnemyData> copy = new List<EnemyData>(list);
            if (IsContainedIn(copy))
            {
                foreach (EnemyData e in Ingredients)
                {
                    list.Remove(e);
                }
                return true;
            }
            else
            {
                return false;
            }


        }
    }

    //sort recipes descending
    private class RecipeSorter : IComparer<Recipe>
    {
        public int Compare(Recipe x, Recipe y)
        {
            float f = -(x.PointScore - y.PointScore);
            if (f < 0)
            {
                return -1;
            }
            else if (f == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }

    public void Start()
    {
        recipes.Sort(new RecipeSorter());
    }

    public void Update()
    {
        ResolveCombo();
    }

    public void AddIngredient(EnemyData enemyData)
    {
        myComboList.Add(enemyData);
        Debug.Log("Ingredient added!");
    }

    public float ResolveCombo()
    {
        bool comboResolved = false;
        List<EnemyData> usedIngredients = new List<EnemyData>(myComboList.Count);
        List<EnemyData> unusedIngredients = new List<EnemyData>(myComboList);
        List<EnemyData> lastIngredients = new List<EnemyData>(myComboList.Count);
        List<Recipe> combos = new List<Recipe>();

        //check each recipes ingredients
        foreach (Recipe r in recipes)
        {
            //remove recipe as many times as possible
            bool removed = false;
            do
            {
                removed = r.RemoveFrom(unusedIngredients);
                //if the recipe was in the list add it to the list of combos
                if (removed)
                {
                    combos.Add(r);
                    usedIngredients.AddRange(r.Ingredients);
                }
            } while (removed);
        }

        //calculate points
        float sum = 0;
        float multiplier = combos.Count;
        foreach (Recipe r in combos)
        {
            sum += r.PointScore;
        }

        CurrentComboSum = sum;
        CurrentComboMultiplier = multiplier;

        return sum * multiplier;
    }

    public void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 100, 50, 100, 100), CurrentComboSum + " x " + CurrentComboMultiplier);
    }
}
