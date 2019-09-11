using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Duck : MonoBehaviour
{
    public GameObject cloud;
    bool is_cloud_on_board = false;
    public TextMeshProUGUI text;
    public string[] sentence;
    private int index=0;
    private const int index_allowed_to_play=7;
    public bool game_is_active = false;
    

    
    private void show_text()
    {
        cloud.transform.position = new Vector3(5.5f, 4, 0);
        cloud.GetComponent<Renderer>().sortingOrder = 3;
        if (is_cloud_on_board == false)
        {
            cloud = Instantiate(cloud);
            is_cloud_on_board = true;
        } 
        text.text += sentence[index];
    }

    private void clear_text()
    {
        cloud.GetComponent<Renderer>().sortingOrder = -1;
        text.text = "";
        index++;
        if (index == index_allowed_to_play)game_is_active = true;
        

    }

    public void welcome()
    {
        game_is_active = false;
        show_text();
        function_timer.create(clear_text, 2f);
        function_timer.create(show_text, 3f);
        function_timer.create(clear_text, 5f);
        function_timer.create(show_text, 6f);
        function_timer.create(clear_text, 7f);
        function_timer.create(show_text, 8f);
        function_timer.create(clear_text, 10f);
        function_timer.create(show_text, 11f);
        function_timer.create(clear_text, 13f);
        function_timer.create(show_text, 14f);
        function_timer.create(clear_text, 16f);
        function_timer.create(show_text, 17f);
        function_timer.create(clear_text, 19f);
    }

    public void game_over(elements_of_game winner)
    {
        //player wins
        if (winner == elements_of_game.CROSS) index = 9;

        //AI wins
        else if (winner == elements_of_game.CIRCLE) index = 7;

        //Draw
        else index = 8;
        show_text();
        function_timer.create(clear_text, 5f);
    }
    
    
   
}
