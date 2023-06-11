using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingController : MonoBehaviour
{
    PlayerController playerController;
    PlayerFinder playerFinder;
    
    void Start()
    {
        playerFinder = new PlayerFinder();
        playerController = playerFinder.GetPlayer().GetComponent<PlayerController>();
        playerController.EndingEvent();
    }

    
    void Update()
    {
        
    }
}
