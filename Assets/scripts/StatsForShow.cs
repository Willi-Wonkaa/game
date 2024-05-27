using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatsForShow : MonoBehaviour
{
    private PlayerController player_controller;

    public TMP_Text hp;
    // public TMP_Text energy;
    public TMP_Text money;
    public TMP_Text lvl;
    public TMP_Text exp;


    void Start()
    {
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        hp.text = Math.Round(player_controller.player_current_hp).ToString() + " / " + Math.Round(player_controller.player_max_hp).ToString();
        hp = transform.GetChild(0).GetComponent<TMP_Text>();

        exp.text = player_controller.player_exp.ToString() + " / " + (int)(10 + Math.Round(player_controller.player_lvl * Math.Log(Math.Pow(6.9, player_controller.player_lvl))));
        exp = transform.GetChild(1).GetComponent<TMP_Text>();

        money.text = player_controller.player_money.ToString();
        money = transform.GetChild(2).GetComponent<TMP_Text>();

        lvl.text = player_controller.player_lvl.ToString();
        lvl = transform.GetChild(3).GetComponent<TMP_Text>();


    }

}
