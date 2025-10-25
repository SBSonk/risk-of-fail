using System;
using Discord;
using UnityEngine;

public class DiscordRPCManager : MonoBehaviour
{
    private const long applicationID = 1088485722706673734;
    public static DiscordRPCManager instance;

    [SerializeField] private string details = "In the main menu.";
    [SerializeField] private string state = "Solo";

    [SerializeField] private string imageName = "logo";
    [SerializeField] private string headerText = "Risk of Fail";


    private Discord.Discord discordRPC;
    private long launchTimestamp;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize RPC Connection
        try
        {
            discordRPC = new Discord.Discord(applicationID, (ulong)CreateFlags.NoRequireDiscord);

            launchTimestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
        catch
        {
            Debug.LogWarning("Discord not detected.");
        }
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
            if (result == Result.Ok)
                Console.WriteLine("Success!");
            else
                Console.WriteLine("Failed");
        });
    }

    private void UpdateStatus()
    {
        try
        {
            var activityManager = discordRPC.GetActivityManager();

            var activity = new Activity
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

            activityManager.UpdateActivity(activity, result =>
            {
                if (result != Result.Ok) Debug.LogWarning("Discord not connected.");
            });
        }
        catch
        {
            Debug.LogWarning("Discord not detected.");
            Destroy(gameObject);
        }
    }

    public void ChangeDiscordState(string _details = "In the main menu.", string _state = "solo",
        string _imageName = "logo")
    {
        details = _details;
        state = _state;
        imageName = _imageName;
    }

    public void ChangeDiscordState(string _state)
    {
        state = _state;
    }

    public void ChangeDiscordDetails(string _details)
    {
        details = _details;
    }
}