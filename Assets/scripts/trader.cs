using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class trader : MonoBehaviour
{
    public Transform player;
    private PlayerController player_controller;

    
    //private bool is_trade_active = false;
    private bool is_player_close = false;

    void Start()
    {

        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }



    void Update()
    {
        is_player_close = IsPlayerClose();
    }


    public bool IsPlayerClose()
    {
        if (Vector3.Distance(transform.position, player.position) <= player_controller.action_radius)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
