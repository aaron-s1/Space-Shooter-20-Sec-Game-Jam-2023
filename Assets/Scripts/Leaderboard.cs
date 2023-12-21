using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using Dan.Enums;

public class Leaderboard : MonoBehaviour
{
    public struct LeaderboardSearchQuery
    {
    public int Skip { get; set; } //amount of entries to skip
    public int Take { get; set; } //amount of entries to take
    public string Username { get; set; }
    public TimePeriodType TimePeriod { get; set; }
    }   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void GetLeaderboardEntries(string publicKey, )
    // {
    //     LeaderboardCreator.GetLeaderboard(string publicKey, Action<Entry[]> callback)
    // }

}
