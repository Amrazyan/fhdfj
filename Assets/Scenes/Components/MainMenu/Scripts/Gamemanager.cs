using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager
{
    private static Gamemanager _instance;

    public static Gamemanager Instance 
    { 
        get 
        {
            if (_instance == null)
            {
                _instance = new Gamemanager();
            }

            return _instance;
        }
    
    }

    public string url = "http://localhost:53696/userdata";


}
