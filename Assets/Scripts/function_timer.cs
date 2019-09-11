using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class function_timer
{
    public static function_timer create(Action action, float time, string timer_name=null)
    {
        init_if_needed();
        GameObject game_object = new GameObject("funtion_timer", typeof(mono_behavior));
        function_timer timer = new function_timer(action, time, timer_name, game_object);
        game_object.GetComponent<mono_behavior>().on_update = timer.Update;
        active_timers.Add(timer);
        return timer;
    }
    private class mono_behavior : MonoBehaviour
    {
        public Action on_update;
        private void Update()
        {
            if(on_update!=null)on_update();
        }
    }
    private static List<function_timer> active_timers;
    private static GameObject init_game_object;
    private Action action;
    private float time;
    private bool is_destroy;
    private GameObject game_object;
    private string timer_name;

    private static void init_if_needed()
    {
        if(init_game_object == null)
        {
            init_game_object = new GameObject("funtion_timer_init_game_object");
            active_timers = new List<function_timer>();
        }
    }

    private static void remove_timer(function_timer timer)
    {
        init_if_needed();
        active_timers.Remove(timer);
    }

    public static void stop_timer(string timer_name)
    {
        int i;
        for(i=0;i<active_timers.Count;i++)
        {
            if(active_timers[i].timer_name == timer_name)
            {
                //Stop this timer
                active_timers[i].destroy_self();
                i--;
            }
        }
    }

    private function_timer(Action action,float time, string timer_name, GameObject game_object)
    {
        this.action = action;
        this.time = time;
        this.game_object = game_object;
        this.timer_name = timer_name;
        is_destroy = false;
    }

    public void Update()
    {
        if(is_destroy == false)
        {
            time -= Time.deltaTime;
            if (time < 0)
            {
                //Trigger action
                action();
                destroy_self();
            }
        }
    }

    private void destroy_self()
    {
        is_destroy = true;
        UnityEngine.Object.Destroy(game_object);
        remove_timer(this);
    }
}
