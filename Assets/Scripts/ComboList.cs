using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ComboList: MonoBehaviour {
    public float CurrentComboSum = 0;
    public float CurrentComboMultiplier = 1;

    public List<EnemyType> myComboList = new List<EnemyType>();

    public List<Recipe> recipes = new List<Recipe>();
    [System.Serializable]
    public class Recipe
    {
        public EnemyType[] Ingredients;
        public float PointScore
        {
            get
            {
                return 1;
            }
        }

        private bool IsContainedIn(List<EnemyType> list)
        {
            if(list == null || Ingredients == null || Ingredients.Length > list.Count)
            {
                return false;
            }

            foreach(EnemyType e in Ingredients)
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
        public bool RemoveFrom(List<EnemyType> list)
        {
            List<EnemyType> copy = new List<EnemyType>(list);
            if (IsContainedIn(copy))
            {
                foreach (EnemyType e in Ingredients)
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
            if(f < 0)
            {
                return -1;
            }
            else if(f == 0)
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
        //ResolveCombo();
    }

    public void AddIngredient(EnemyType enemyType)
    {
        myComboList.Add(enemyType);
    }

    public float ResolveCombo()
    {
        bool comboResolved = false;
        List<EnemyType> usedIngredients = new List<EnemyType>(myComboList.Count);
        List<EnemyType> unusedIngredients = new List<EnemyType>(myComboList);
        List<EnemyType> lastIngredients = new List<EnemyType>(myComboList.Count);
        List<Recipe> combos = new List<Recipe>();

        while(!comboResolved)
        {
            //check each recipes ingredients
            foreach(Recipe r in recipes)
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

            //chech anything has changed
            if(lastIngredients.Equals(usedIngredients))
            {
                comboResolved = true;
            }
            else
            {
                lastIngredients.Clear();
                lastIngredients.AddRange(usedIngredients); 
            }
            Debug.Log("Blubb!");
        }

        //calculate points
        float sum = 0;
        float multiplier = combos.Count;
        foreach(Recipe r in combos)
        {
            sum += r.PointScore;
        }

        CurrentComboSum = sum;
        CurrentComboMultiplier = multiplier;

        return sum * multiplier;
    }
}
