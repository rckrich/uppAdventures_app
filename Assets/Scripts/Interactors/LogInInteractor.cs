using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogInInteractor : Interactor
{
    public override void PerformSearch(params object[] list)
    {

        if ((LogInMethods)list[0] == LogInMethods.PostLogIn)
        {
            List<KeyValuePair<string, string>> fields = new List<KeyValuePair<string, string>>();
            KeyValuePair<string, string> username = new KeyValuePair<string, string>("email", (string)list[2]);
            KeyValuePair<string, string> password = new KeyValuePair<string, string>("password", (string)list[3]);
            fields.Add(username);
            fields.Add(password);
            StartCoroutine(Post<LogInEntity>((string)list[1], LogInMethods.PostLogIn, null, fields));
        }

        if ((LogInMethods)list[0] == LogInMethods.GetUserData)
        {
            StartCoroutine(Get<GetUserEntity>((string)list[1], LogInMethods.GetUserData, (string)list[2]));
        }

    }
}
