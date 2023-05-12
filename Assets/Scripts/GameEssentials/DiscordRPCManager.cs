using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Discord;

public class DiscordRPCManager : MonoBehaviour
{
    public static DiscordRPCManager instance;

    const long applicationID = 1088485722706673734;

    [SerializeField] string details = "In the main menu.";
    [SerializeField] string state = "Solo";

    [SerializeField] string imageName = "logo";
    [SerializeField] string headerText = "Risk of Fail";
    private long launchTimestamp;
    
    
    private Discord.Discord discordRPC;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Initialize RPC Connection
        discordRPC = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
        
        launchTimestamp = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    private void Update()
    {
        try
        {
            discordRPC.RunCallbacks();
        }
        catch
        {
            Debug.LogWarning("Discord not detected.");
            Destroy(gameObject);
        }

        UpdateStatus();
    }

    private void OnApplicationQuit()
    {
        var activityManager = discordRPC.GetActivityManager();
        activityManager.ClearActivity(result =>
        {
            if (result == Discord.Result.Ok)
            {
                Console.WriteLine("Success!");
            }
            else
            {
                Console.WriteLine("Failed");
            }  
        });
    }

    void UpdateStatus()
    {
        try
        {
            var activityManager = discordRPC.GetActivityManager();

            var activity = new Discord.Activity
            {
                Details = details,
                State = state,
                Timestamps =
                {
                    Start = launchTimestamp
                },
                Assets =
                {
                    LargeImage = imageName,
                    LargeText = headerText
                }
            };

            activityManager.UpdateActivity(activity, (result =>
            {
                if (result != Discord.Result.Ok) Debug.LogWarning("Discord not connected.");
            }));
        }
        catch
        {
            Debug.LogWarning("Discord not detected.");
            Destroy(gameObject);
        }
        
    }

    public void ChangeDiscordState(string _details = "In the main menu.", string _state = "solo", string _imageName = "logo")
    {
        details = _details;
        state = _state;
        imageName = _imageName;
    }

    public void ChangeDiscordState(string _state)
    {
        state = _state;
    }

    public void ChangeDiscordDetails(string _details) => details = _details;
}
