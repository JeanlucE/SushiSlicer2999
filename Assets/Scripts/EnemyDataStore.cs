using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Sushi/EnemyDataStore")]
public class EnemyDataStore : ScriptableObject {

    public List<EnemyData> enemyTypes;

    public EnemyData getEnemy(string name)
    {
        return enemyTypes.Find((x) => x.name == name);
    }

}
