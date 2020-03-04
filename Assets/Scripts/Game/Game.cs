using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    private string _id;
    public string Id
    {
        get { return _id; }
        set { _id = value; }
    }

    private string _idplayer;
    public string IdPlayer
    {
        get { return _idplayer; }
        set { _idplayer = value; }
    }

    private int _remainingloot;
    public int RemainingLoot
    {
        get { return _remainingloot; }
        set { _remainingloot = value; }
    }

    private string _toploots;
    public string TopLoots
    {
        get { return _toploots; }
        set { _toploots = value; }
    }

    private string _lastloots;
    public string LastLoots
    {
        get { return _lastloots; }
        set { _lastloots = value; }
    }
}
