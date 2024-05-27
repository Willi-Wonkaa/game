using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class quest_test : MonoBehaviour
{
    public int quest_id;


    public List<string> start_speech;
    public List<Sprite> start_speacer_img;

    public string speech_during_active;
    public Sprite sprite_during_active;

    public List<ItemScriptableObject> conditions_items;
    public List<int> conditions_items_count;

    public List<GameObject> rewards;
    public List<int> rewards_items_count;

    public List<string> end_speech;
    public List<Sprite> end_speacer_img;

}
