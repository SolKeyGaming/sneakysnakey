using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdibleManager : MonoBehaviour
{

    public GameObject Snake;
    public Vector2 SpecialSpawnIndexRange;

    Habilities habilities;
    int ediblesSinceLastPowerUp;
    EdibleFactory edibleFactory;
    //int powerUpPercentajes[]

    void Start()
    {
        ediblesSinceLastPowerUp = 0;
        edibleFactory = GetComponent<EdibleFactory>();
        habilities = Snake.GetComponent<Habilities>();
    }

    public void EdibleEaten(Edible edible)
    {
        ediblesSinceLastPowerUp += 1;
        edibleFactory.SpawnEdible(edible.EdibleType);
        // TODO: factorizar esta mierda? ya la arregle un poco xd
        GetComponentInParent<LevelManager>().Score += 1;

        int powerUpIndex = (int)Random.Range(SpecialSpawnIndexRange.x, SpecialSpawnIndexRange.y);

        if (ediblesSinceLastPowerUp >= powerUpIndex)
        {
            ediblesSinceLastPowerUp = 0;
            edibleFactory.SpawnEdible(EdibleType.PowerUp);
        }
    }

    public void EdibleEaten(PowerUp powerUp)
    {
        habilities.ActivateHability(powerUp.PowerUpType);
    }
}
