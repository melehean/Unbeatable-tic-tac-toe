using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AI
{
    private elements_of_game human, me;

    public AI(elements_of_game human_player, elements_of_game AI_player)
    {
        this.human = human_player;
        this.me = AI_player;
    }

    private elements_of_game who_win(elements_of_game[,] board)
    {
        int i, j;
        bool check;
        for (i = 0; i < 3; i++)
        {
            //Horizontally
            j = 0; check = true;
            while (check == true && j < 2)
            {
                if (board[i, j] != board[i, j + 1] || board[i, j] == elements_of_game.NOTHING) check = false;
                j++;
            }
            if (check == true)return board[i, 0];
            //Vertically
            j = 0; check = true;
            while (check == true && j < 2)
            {
                if (board[j, i] != board[j + 1, i] || board[j, i] == elements_of_game.NOTHING) check = false;
                j++;
            }
            if (check == true)return board[0,i];
        }
        //Diagonal down
        j = 0; check = true;
        while (check == true && j < 2)
        {
            if (board[j, j] != board[j + 1, j + 1] || board[j, j] == elements_of_game.NOTHING) check = false;
            j++;
        }
        if (check == true)return board[0,0];
        //Diagonal up
        j = 0; i = 2; check = true;
        while (check == true && j < 2)
        {
            if (board[j, i] != board[j + 1, i - 1] || board[j, i] == elements_of_game.NOTHING) check = false;
            j++; i--;
        }
        if (check == true)return board[0,2];
        return elements_of_game.NOTHING;
    }

    private bool is_board_full(elements_of_game[,] board)
    {
        int i, j;
        for (i = 0; i < 3; i++) 
            for (j = 0; j < 3; j++)
                if (board[i, j] == elements_of_game.NOTHING) return false;
        return true;
    }

    int minimax(elements_of_game[,] board, int depth, elements_of_game player)
    {
	    elements_of_game victory = who_win(board);

	    if (victory == me)
		    return 10-depth;

	    if (victory == human)
		    return -10+depth;

	    if (is_board_full(board) == true)
		    return 0;

	    if (player == me)
	    {
		    int best = -1000;

		    for (int i = 0; i < 3; i++)
		    {
			    for (int j = 0; j < 3; j++)
			    {
				    if (board[i,j] == elements_of_game.NOTHING)
				    {
					    board[i,j] = me;

					    best = Math.Max(best,
						    minimax(board, depth + 1, human));

					    board[i,j] = elements_of_game.NOTHING;
				    }
			    }
		    }
		    return best;
	    }
	    else
	    {
		    int best = 1000;

		    for (int i = 0; i < 3; i++)
		    {
			    for (int j = 0; j < 3; j++)
			    {

				    if (board[i,j] == elements_of_game.NOTHING)
				    {
					    board[i,j] = human;

					    best = Math.Min(best,
						    minimax(board, depth + 1, me));

					    board[i,j] = elements_of_game.NOTHING;
				    }
			    }
		    }
		    return best;
        }
    }

    public Vector3 I_am_smarter_than_you_human(elements_of_game[,] board)
    {
	    int best_value = -1000;
        int move_value,i,j;
	    Vector3 best_move;
	    best_move.x = -1;
	    best_move.y = -1;
        best_move.z = -1;

	    for (i = 0; i < 3; i++)
	    {
		    for (j = 0; j < 3; j++)
		    {
			    if (board[i,j] == elements_of_game.NOTHING)
			    {
				    board[i,j] = me;

				    move_value = minimax(board, 0, human);

				    board[i,j] = elements_of_game.NOTHING;

				    if (move_value > best_value)
				    {
					    best_move.x = i;
					    best_move.y = j;
					    best_value = move_value;
				    }
			    }
		    }
	    }

	    return best_move;
    }
}