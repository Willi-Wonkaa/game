using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tradescript : MonoBehaviour
{
    public GameObject trade_panel;

    public Transform player;
    private PlayerController player_controller;

    private bool is_player_close = false;
    private bool is_trade_active = false;


    void Start()
    {
        trade_panel.SetActive(false);
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }



    void Update()
    {
        is_player_close = IsPlayerClose();

        if (is_trade_active == true && is_player_close && Input.GetKeyDown(KeyCode.Escape))
        {
            trade_panel.SetActive(false);
            is_trade_active = false;
        }
        if (is_trade_active == false && is_player_close && Input.GetKeyDown(KeyCode.T))
        {
            trade_panel.SetActive(true);
            is_trade_active = true;
        } 

        if (is_player_close == false)
        {
            trade_panel.SetActive(false);
            is_trade_active = false;
        }
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
