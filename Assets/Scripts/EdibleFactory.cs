using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EdibleType
{
    BasicEdible,
    PowerUp,
    Super,
    Debuff
}

[System.Serializable]
public class PowerUpPercentajePair
{
    public GameObject PowerUp;
    public int Percentaje;
}

public class EdibleFactory : MonoBehaviour
{

    public List<PowerUpPercentajePair> PowerUps;
    public GameObject BasicEdiblePrefab;
    //public GameObject PowerUp1;
    //public GameObject PowerUp2;
    public GameObject Super1;

    void Start()
    {
        // check at start if probabilities of powerups doesn't sum 100%
        int percent = 0;
        foreach (PowerUpPercentajePair pair in PowerUps)
        {
            percent += pair.Percentaje;
        }
        if (percent != 100)
        {
            Debug.LogError("percentajes of powerups doesn't sum 100%," +
                " you are vulnerable to errors. Check EdibleManager settings");
        }

        // Spawn first edible at the start of the game
        SpawnEdible(EdibleType.BasicEdible);
    }

    public void SpawnEdible(EdibleType type)
    {
        GameObject go;

        switch (type)
        {
            case EdibleType.BasicEdible:
                go = BasicEdiblePrefab;
                break;
            case EdibleType.PowerUp:
                go = getPowerUpByProbability();
                break;
            default:
                go = BasicEdiblePrefab; // asd
                Debug.Log("EdibleFactory.SpawnEdible(): No edible found of type = " + type);
                return;
        }

        go.transform.position = getRandomPos();
        Instantiate(go);
    }

    Vector2 getRandomPos()
    {
        float x = Random.Range(-3, 3);
        float y = Random.Range(-4, 4);

        x -= x % 0.64f;
        y -= y % 0.64f;

        return new Vector2(x, y);
    }

    GameObject getPowerUpByProbability()
    {

        System.Random random = new System.Random();
        int roll = random.Next(0, 101);
        int cumulative = 0;
        foreach (PowerUpPercentajePair pair in PowerUps)
        {
            cumulative += pair.Percentaje;
            if (roll < cumulative)
            {
                Debug.Log(roll);
                return pair.PowerUp;
            }
        }
        // if we are here, no powerup was selected
        Debug.LogError("no powerup selected to spawn, check propabilities, rolled " + roll);
        return null;
    }

}
