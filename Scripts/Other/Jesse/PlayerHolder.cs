using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHolder : MonoBehaviour
{

    PlayerFinder playerFinder;
    public GameObject player;

    void Awake()
    {
        playerFinder = new PlayerFinder();
        player = playerFinder.GetPlayer();
    }

}
