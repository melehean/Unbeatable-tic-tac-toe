using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum elements_of_game { CIRCLE, CROSS, NOTHING, FULL};


public class board : MonoBehaviour
{
    private enum bar_position { VERTICAL, HORIZONTAL, DIAGONAL_DOWN, DIAGONAL_UP }
    public GameObject circle, cross, bar, duck;
    private GameObject help_bar;

    public Button play_again_button, cross_button, circle_button;

    private elements_of_game[,] matrix;
    
    private Vector3 mouse_position,board_position, spawn_position;
   
    private List<GameObject> active_elements;
    
    private elements_of_game turn = elements_of_game.NOTHING, winner;
    private elements_of_game human, robot;
    
    private AI Edward;

    private void Start()
    {
        matrix = new elements_of_game[3, 3];
        active_elements = new List<GameObject>();
        help_bar = null;
        int i, j;
        for (i = 0; i < 3; i++)
            for (j = 0; j < 3; j++)
                matrix[i, j] = elements_of_game.NOTHING;
        human = turn = elements_of_game.CROSS;
        robot = elements_of_game.CIRCLE;
        duck.GetComponent<Duck>().welcome();
        Edward = new AI(human, robot);
    }

    private Vector3 get_board_position(Vector3 mouse_position)
    {
        //middle middle tail
        if (mouse_position.x <= 1 && mouse_position.x >= -1 && mouse_position.y <= 1 && mouse_position.y >= -1)
            return new Vector3(1, 1, 0);
        //left up tail
        else if (mouse_position.x <= -2 && mouse_position.x >= -5 && mouse_position.y <= 5 && mouse_position.y >= 2)
            return new Vector3(0, 0, 0);
        //middle up tail
        else if (mouse_position.x <= 1 && mouse_position.x >= -1 && mouse_position.y <= 5 && mouse_position.y >= 2)
            return new Vector3(0, 1, 0);
        //right up tail
        else if (mouse_position.x <= 5 && mouse_position.x >= 2 && mouse_position.y <= 5 && mouse_position.y >= 2)
            return new Vector3(0, 2, 0);
        //middle left tail
        else if (mouse_position.x <= -2 && mouse_position.x >= -5 && mouse_position.y <= 1 && mouse_position.y >= -1)
            return new Vector3(1, 0, 0);
        //middle right tail
        else if (mouse_position.x <= 5 && mouse_position.x >= 2 && mouse_position.y <= 1 && mouse_position.y >= -1)
            return new Vector3(1, 2, 0);
        //left down tail
        else if (mouse_position.x <= -2 && mouse_position.x >= -5 && mouse_position.y <= -2 && mouse_position.y >= -5)
            return new Vector3(2, 0, 0);
        //middle down tail
        else if (mouse_position.x <= 1 && mouse_position.x >= -1 && mouse_position.y <= -2 && mouse_position.y >= -5)
            return new Vector3(2, 1, 0);
        //right down tail
        else if (mouse_position.x <= 5 && mouse_position.x >= 2 && mouse_position.y <= -2 && mouse_position.y >= -5)
            return new Vector3(2, 2, 0);
        else return new Vector3(-1, -1, 0);
    }

    private Vector3 get_spawn_position(Vector3 board_position)
    {
        Vector3 origin = Vector3.zero;
        if (board_position.x == 2) origin.y += -4;
        if (board_position.x == 0) origin.y += 4;
        if (board_position.y == 2) origin.x += 4;
        if (board_position.y == 0) origin.x += -4;
        return origin;
    }

    private void spawn_object(Vector3 spawn_position)
    {
        spawn_position = get_spawn_position(board_position);
        matrix[(int)board_position.x, (int)board_position.y] = turn;


        if (turn==elements_of_game.CROSS)
        {
            GameObject help = Instantiate(cross, spawn_position, Quaternion.identity);
            active_elements.Add(help);
            turn = elements_of_game.CIRCLE;
        }
        else if (turn == elements_of_game.CIRCLE)
        {
            GameObject help = Instantiate(circle, spawn_position, Quaternion.identity);
            active_elements.Add(help);
            turn=elements_of_game.CROSS;
        }
    }

    private elements_of_game who_wins()
    {
        int i, j;
        bool check;
        for(i=0;i<3;i++)
        {
            //Horizontally
            j=0;check=true;
            while(check==true&&j<2)
            {
                if (matrix[i, j] != matrix[i, j + 1] || matrix[i,j] == elements_of_game.NOTHING) check = false;
                j++;
            }
            if (check == true)
            {
                draw_bar(bar_position.HORIZONTAL,i);
                return matrix[i,0];
            } 
            //Vertically
            j = 0; check = true;
            while (check == true && j < 2)
            {
                if (matrix[j, i] != matrix[j + 1, i] || matrix[j, i] == elements_of_game.NOTHING) check = false;
                j++;
            }
            if (check == true)
            {
                draw_bar(bar_position.VERTICAL, i);
                return matrix[0,i];
            }
            
        }
        //Diagonal down
        j = 0; check = true;
        while(check==true&&j<2)
        {
            if (matrix[j, j] != matrix[j + 1, j + 1] || matrix[j, j] == elements_of_game.NOTHING) check = false;
            j++;
        }
        if (check == true)
        {
            draw_bar(bar_position.DIAGONAL_DOWN);
            return matrix[0,0];
        } 
        //Diagonal up
        j = 0; i = 2;check = true;
        while(check==true&&j<2)
        {
            if (matrix[j ,i] != matrix[j+1, i-1]||matrix[j,i]==elements_of_game.NOTHING) check = false;
            j++; i--;
        }
        if (check == true)
        {
            draw_bar(bar_position.DIAGONAL_UP);
            return matrix[0,2];
        }
        for (i = 0; i < 3; i++)
            for (j = 0; j < 3; j++)
                if (matrix[i, j] == elements_of_game.NOTHING) return elements_of_game.NOTHING;
        return elements_of_game.FULL;//Draw
    }

    private void endgame(elements_of_game winner)
    {
        duck.GetComponent<Duck>().game_is_active = false;
        duck.GetComponent<Duck>().game_over(winner);
        //foreach(GameObject it in active_elements)
        //{
          //  Destroy(it);
        //}
    }

    private void draw_bar(bar_position position, int index)
    {
        Vector3 vector = Vector3.zero;
        if(position==bar_position.VERTICAL)
        {
            vector.x+=1;
            vector.y += index;
            vector = get_spawn_position(vector);
            help_bar = Instantiate(bar, vector, Quaternion.identity);
            help_bar.transform.localScale = new Vector3(2, 4, 0);
        }
        else
        {
            vector.y+=1;
            vector.x += index;
            vector = get_spawn_position(vector);
            help_bar = Instantiate(bar, vector, Quaternion.Euler(0,0,90));
            help_bar.transform.localScale = new Vector3(2, 4, 0);
        }
    }

    private void draw_bar(bar_position position)
    {
        if(position == bar_position.DIAGONAL_UP)
        {
            help_bar = Instantiate(bar, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 135));
            help_bar.transform.localScale = new Vector3(2, 4, 0);
        }
        if (position == bar_position.DIAGONAL_DOWN)
        {
            help_bar = Instantiate(bar, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 45));
            help_bar.transform.localScale = new Vector3(2, 4, 0);
        }
    }

    private void Update()
    {
        if(duck.GetComponent<Duck>().game_is_active == true) 
        {
            if (turn == human)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mouse_position = Input.mousePosition;
                    mouse_position = Camera.main.ScreenToWorldPoint(mouse_position);
                    board_position = get_board_position(new Vector3((int)mouse_position.x, (int)mouse_position.y, 0));
                    if (board_position.x >= 0 && board_position.y >= 0 && matrix[(int)board_position.x, (int)board_position.y] == elements_of_game.NOTHING)
                    {
                        spawn_object(board_position);
                        winner = who_wins();
                        if (winner!=elements_of_game.NOTHING)
                        {
                            endgame(winner);
                        }
                    }
                }
            }
            else
            {
                board_position = Edward.I_am_smarter_than_you_human(matrix);
                if (board_position.x >= 0 && board_position.y >= 0 && matrix[(int)board_position.x, (int)board_position.y] == elements_of_game.NOTHING)
                {
                    spawn_object(board_position);
                    winner = who_wins();
                    if (winner!=elements_of_game.NOTHING)
                    {
                        endgame(winner);
                    }
                }
            }
        }
            
    }
}
