using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class NPC_script : MonoBehaviour
{
    public InventoryManager inventory_manager;
    public quest_test current_quest_GO;

    public string npc_name;
    public int npc_id;


    public GameObject dialogue_panel;
    public TextMeshProUGUI dialogue_text;
    public Image dialogue_speaker;

    public List<GameObject> NPS_content;




    [SerializeField] private int current_speech_len;
    [SerializeField] private int current_dialogue_moment = 0;

    private int total_quest_count;
    [SerializeField] private int number_of_current_quest;
    private bool is_npc_content_end = false;

    private bool is_all_quest_conditions_are_met = false;

    public Transform player;
    private PlayerController player_controller;
    public bool is_player_close = false;
    public bool is_dialogue_in_action = false;
    private bool is_just_talk = false;



    public bool is_quest_done = false;


    [SerializeField] private bool is_first_phase = true;
    [SerializeField] private bool is_second_phase = false;
    [SerializeField] private bool is_third_phase = false;

    void Start()
    {
        inventory_manager = GameObject.FindGameObjectWithTag("test_UI").GetComponent<InventoryManager>();
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player").GetComponent<Transform>();


        dialogue_panel.SetActive(false);
        current_quest_GO = NPS_content[number_of_current_quest].GetComponent<quest_test>();

        current_speech_len = current_quest_GO.start_speech.Count;
        total_quest_count = NPS_content.Count;
    }


    void Update()
    {
        if (is_npc_content_end == false)
        {
            is_player_close = IsPlayerClose();
            is_just_talk = IsJustTalk();

            // игрок начинает диалог
            if ((is_dialogue_in_action == false) && is_player_close && Input.GetKeyDown(KeyCode.Q))
            {
                is_dialogue_in_action = true;
            }


            if (is_dialogue_in_action && is_player_close)
            {
                dialogue_panel.SetActive(true);

                // игрок выполнил задание - закрывающий квест диалог
                if (is_third_phase)
                {
                    dialogue_text.text = current_quest_GO.end_speech[current_dialogue_moment];
                    dialogue_speaker.sprite = current_quest_GO.end_speacer_img[current_dialogue_moment];

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        ++current_dialogue_moment;
                        if (current_dialogue_moment >= current_speech_len)
                        {
                            is_dialogue_in_action = false;
                            is_quest_done = true;
                            dialogue_panel.SetActive(false);
                        }
                    }
                    if (is_quest_done)
                    {
                        ConsumeItems();
                        AwardIssuance();

                        // переход к новому квесту или "отключение" НПС
                        ++number_of_current_quest;
                        if (number_of_current_quest >= total_quest_count)
                        {
                            is_npc_content_end = true;
                        }
                        else
                        {
                            current_quest_GO = NPS_content[number_of_current_quest].GetComponent<quest_test>();
                        }

                    }

                }
                
                // диалог во время выполнения задания
                if (is_second_phase)
                {
                    is_all_quest_conditions_are_met = IsAllQuestConditionsAreMet();
                    // Debug.Log("is_all_quest_conditions_are_met: " + is_all_quest_conditions_are_met);

                    // если задание выполнено то мы сразу переходим к следующей фазе без вывода цикличной фразы
                    if (is_all_quest_conditions_are_met)
                    {
                        is_second_phase = false;
                        is_third_phase = true;

                    } else {
                        is_dialogue_in_action = true;
                        dialogue_panel.SetActive(true);
                        dialogue_text.text = current_quest_GO.speech_during_active;
                        dialogue_speaker.sprite = current_quest_GO.sprite_during_active;

                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            is_dialogue_in_action = false;
                            dialogue_panel.SetActive(false);
                        }
                    }
                }

                // первая активация диалога - выдача задания
                if (is_first_phase)
                {
                    dialogue_text.text = current_quest_GO.start_speech[current_dialogue_moment];
                    dialogue_speaker.sprite = current_quest_GO.start_speacer_img[current_dialogue_moment];

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("space is down" + current_dialogue_moment);
                        ++current_dialogue_moment;
                        // если мы закончили диалог
                        if (current_dialogue_moment >= current_speech_len)
                        {
                            is_dialogue_in_action = false;
                            current_dialogue_moment = 0;
                            current_speech_len = current_quest_GO.end_speech.Count;

                            is_first_phase = false;
                            is_second_phase = true;
                            dialogue_panel.SetActive(false);
                            Debug.Log("вроде как тут квест должен начаться");

                            if (is_just_talk)
                            {
                                AwardIssuance();
                                // переход к новому квесту или "отключение" НПС
                                ++number_of_current_quest;
                                if (number_of_current_quest >= total_quest_count)
                                {
                                    is_npc_content_end = true;
                                }
                                else
                                {
                                    current_quest_GO = NPS_content[number_of_current_quest].GetComponent<quest_test>();
                                }
                            }
                        }
                    }

                }

            }

            if(is_dialogue_in_action && !is_player_close)
            {
                RestartDialogueAndClosePannel(); 
            }
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

    public bool IsAllQuestConditionsAreMet()
    {
        for (int i = 0; i < current_quest_GO.conditions_items.Count; ++i) {
            if ( !inventory_manager.InventoryItemChecer(current_quest_GO.conditions_items[i], current_quest_GO.conditions_items_count[i]))
                return false;
        } 
        return true;
    }


    public void ConsumeItems()
    {
        for (int i = 0; i < current_quest_GO.conditions_items.Count; ++i)
        {
            inventory_manager.DeleteItemFromPlayerInventory(current_quest_GO.conditions_items[i], current_quest_GO.conditions_items_count[i]);
        }
    }

    public void AwardIssuance()
    {
        for (int i = 0; i < current_quest_GO.rewards.Count; ++i)
        {
            for (int j = 0; j < current_quest_GO.rewards_items_count[i]; ++j)
            {
                Instantiate(current_quest_GO.rewards[i], new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), Quaternion.identity);
            }
        }
    }

    public void RestartDialogueAndClosePannel()
    {
        current_dialogue_moment = 0;
        dialogue_panel.SetActive(false);
        is_dialogue_in_action = false;
    }


    public bool IsJustTalk()
    {
        if (current_quest_GO.conditions_items_count[0] == -1)
        {
            return true;
        } else
        {
            return false;
        }
    }

}


/*   
    структура квеста (current_quest_GO)

    public int quest_id;


    public List<string> start_speech;
    public List<Sprite> start_speacer_img;

    public string speech_during_active;
    public Sprite sprite_during_active;

    public List<GameObject> conditions_items;
    public List<int> conditions_items_count;

    public List<GameObject> rewards;
    public List<int> rewards_items_count;

    public List<string> end_speech;
    public List<Sprite> end_speacer_img;

*/