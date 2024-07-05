using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Script to keep track of the score and resetting the game
    private static bool won = false;
    private static bool lose = false;

    private static bool move = false;
    private static bool paused = false;

    //When player or the agent wins then set this and reset the game
    public static void setWin(bool val)
    {
        won = val;
    }

    public static bool getWin()
    {
        return won;
    }

    public static void setLose(bool val)
    {
        lose = val;
    }

    public static bool getLose()
    {
        return lose;
    }

    public static void setMove(bool val)
    {
        move = val;
    }

    public static void setPaused(bool val)
    {
        paused = val;
    }

    public static bool getMove()
    {
        return move;
    }

    public static bool getPaused()
    {
        return paused;
    }
}
