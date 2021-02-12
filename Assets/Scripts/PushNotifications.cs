using UnityEngine;
using Unity.Notifications.Android;

public class PushNotifications : MonoBehaviour
{
    public double timeNoteInHours = 8;

    private void Awake()
    {
        AndroidNotificationChannel channel = new AndroidNotificationChannel()
        {
            Name = "Return to game",
            Description = "Return to game description",
            Id = "return",
            Importance = Importance.High
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }


    public void SendNotification()
    {
        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Great comeback!",
            Text = "Return to game, please",
            SmallIcon = "small_icon",
            LargeIcon = "large_icon",
            FireTime = System.DateTime.Now.AddHours(timeNoteInHours)
        };

        AndroidNotificationCenter.CancelAllNotifications();
        AndroidNotificationCenter.SendNotification(notification, "Return to game");        
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SendNotification();
        }
    }

    private void OnApplicationQuit()
    {
        SendNotification();
    }
}
