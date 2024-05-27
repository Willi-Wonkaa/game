using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class dialogue : MonoBehaviour
{
    public GameObject dialogue_panel;
    public TextMeshProUGUI dialogue_text;
    public string[] dialogue_text_array;
    private int index = 0;

    public float typing_speed;
    public bool is_player_close;

    public Transform player;
    private PlayerController player_controller;

    void Start()
    {
        dialogue_text.text = "";
        //dialogue_panel.SetActive(false);
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    void Update()
    {
        is_player_close = IsPlayerClose();

        if (is_player_close)
        {
            if (!dialogue_panel.activeInHierarchy)
            {
                dialogue_panel.SetActive(true);
                StartCoroutine(Typing());
            }
            else if (dialogue_text.text == dialogue_text_array[index])
            {
                NextLine();
            }

        }
        if (Input.GetKeyDown(KeyCode.Q) && dialogue_panel.activeInHierarchy)
        {
            RemoveText();
        }
    }

    public void RemoveText()
    {
        dialogue_text.text = "";
        index = 0;
        dialogue_panel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue_text_array[index].ToCharArray())
        {
            dialogue_text.text += letter;
            yield return new WaitForSeconds(typing_speed);
        }
    }

    public void NextLine()
    {
        if (index < dialogue_text_array.Length - 1)
        {
            index++;
            dialogue_text.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
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