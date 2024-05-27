using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class indicators : MonoBehaviour
{ 
    private PlayerController player_controller;

    public Image player_exp_bar;
    public Image player_hp_bar;


    // насколько заполнена шкала в % 
    public float player_exp_amount = 1f;
    public float player_hp_amount = 1f;


    void Start()
    {
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        // stats_for_show = GameObject.FindGameObjectWithTag""
    }
    
    void Update()
    {
        player_hp_amount = ((float)player_controller.player_current_hp / (float)player_controller.player_max_hp);
        player_exp_amount = ((float)player_controller.player_exp / (float)(int)(10 + Math.Round(player_controller.player_lvl * Math.Log(Math.Pow(6.9, player_controller.player_lvl)))));

        player_exp_bar.fillAmount = player_exp_amount;
        player_hp_bar.fillAmount = player_hp_amount;
        /*
        Debug.Log(player_hp_amount);
        Debug.Log(player_exp_amount);
        */

    } 
}
