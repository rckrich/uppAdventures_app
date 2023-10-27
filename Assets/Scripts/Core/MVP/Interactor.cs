using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Reflection;
using System;

public abstract class Interactor : MonoBehaviour
{
    protected string _query;
    protected Presenter presenter;

    public virtual void Initialize<TPresenter>(Presenter presenter) where TPresenter : Presenter {
        if (this.presenter == null)
            this.presenter = (TPresenter)presenter;
    }
    public virtual void PerformSearch(params object[] list) { }
    protected string getCompleteRequestURL(string uri, string[] parameters = null)
    {
        string url = uri;
        if (parameters != null)
        {

            for (int i = 0; i < parameters.Length; i++)
            {
                uri = url;
                url = "";
                url = uri + parameters[i] + "/";
            }

        }
        return url;
    }
    protected void tryCatchServerError(long responseCode)
    {
        try
        {
            if (responseCode == 429)
                throw new Exception("Response code: " + responseCode + " .Many calls to the server.");
        }
        catch (Exception)
        {
            throw new Exception("Response code: " + responseCode);
        }
    }
    protected virtual IEnumerator Post<T>(string uri, string[] parameters = null, List<KeyValuePair<string, string>> fields = null)
    {
        string url = this.getCompleteRequestURL(uri, parameters);
        WWWForm form = new WWWForm();

        for (int i = 0; i < fields.Count; i++) {
            form.AddField(fields[i].Key, fields[i].Value);
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {

                //Catch id Error 401 Unauthorized
                if (webRequest.responseCode.Equals(401))
                {
                    tryCatchServerError(webRequest.responseCode);
                    presenter.OnServerError(webRequest.responseCode);
                    yield break;
                }

                //Catch if Error 400 Bad Request
                if (webRequest.responseCode.Equals(400))
                {
                    string jsonArray = webRequest.downloadHandler.text;
                    ErrorEntity errorEntity = JsonConvert.DeserializeObject<ErrorEntity>(jsonArray);
                    presenter.OnResult(errorEntity);
                    presenter.OnFailedResult(errorEntity);
                    yield break;
                }

                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);
                presenter.OnServerError(webRequest.responseCode);
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        string jsonArray = webRequest.downloadHandler.text;
                        T genericType = JsonConvert.DeserializeObject<T>(jsonArray);
                        presenter.OnResult(genericType);
                        yield break;
                    }
                }
            }

            presenter.OnFailedResult();
            yield break;

        }
    }
    protected virtual IEnumerator Post<T>(string uri, Enum method, string[] parameters = null, List<KeyValuePair<string, string>> fields = null)
    {
        string url = this.getCompleteRequestURL(uri, parameters);
        WWWForm form = new WWWForm();

        for (int i = 0; i < fields.Count; i++)
        {
            form.AddField(fields[i].Key, fields[i].Value);
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch id Error 401 Unauthorized
                if (webRequest.responseCode.Equals(401))
                {
                    tryCatchServerError(webRequest.responseCode);
                    presenter.OnServerError(webRequest.responseCode);
                    yield break;
                }

                //Catch if Error 400 Bad Request
                if (webRequest.responseCode.Equals(400))
                {
                    string jsonArray = webRequest.downloadHandler.text;
                    ErrorEntity errorEntity = JsonConvert.DeserializeObject<ErrorEntity>(jsonArray);
                    presenter.OnResult(method, errorEntity);
                    presenter.OnFailedResult(method, errorEntity);
                    yield break;
                }

                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);
                presenter.OnServerError(webRequest.responseCode);
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    //TODO atrapar si es mensaje de que necesita autorizar su correo
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        string jsonArray = webRequest.downloadHandler.text;
                        T genericType = JsonConvert.DeserializeObject<T>(jsonArray);
                        presenter.OnResult(method, genericType);
                        yield break;
                    }
                }
            }

            presenter.OnFailedResult(method);
            yield break;

        }
    }
    protected virtual IEnumerator Post<T>(string uri, string bearer, string[] parameters = null, List<KeyValuePair<string, string>> fields = null)
    {
        string url = this.getCompleteRequestURL(uri, parameters);
        WWWForm form = new WWWForm();

        for (int i = 0; i < fields.Count; i++)
        {
            form.AddField(fields[i].Key, fields[i].Value);
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + bearer);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch id Error 401 Unauthorized
                if (webRequest.responseCode.Equals(401))
                {
                    tryCatchServerError(webRequest.responseCode);
                    presenter.OnServerError(webRequest.responseCode);
                    yield break;
                }

                //Catch if Error 400 Bad Request
                if (webRequest.responseCode.Equals(400))
                {
                    string jsonArray = webRequest.downloadHandler.text;
                    ErrorEntity errorEntity = JsonConvert.DeserializeObject<ErrorEntity>(jsonArray);
                    presenter.OnResult(errorEntity);
                    presenter.OnFailedResult(errorEntity);
                    yield break;
                }

                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);
                presenter.OnServerError(webRequest.responseCode);
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        string jsonArray = webRequest.downloadHandler.text;
                        T genericType = JsonConvert.DeserializeObject<T>(jsonArray);
                        presenter.OnResult(genericType);
                        yield break;
                    }
                }
            }

            presenter.OnFailedResult();
            yield break;

        }
    }
    protected virtual IEnumerator Post<T>(string uri, Enum method, string bearer, string[] parameters = null, List<KeyValuePair<string, string>> fields = null)
    {
        string url = this.getCompleteRequestURL(uri, parameters);
        WWWForm form = new WWWForm();

        for (int i = 0; i < fields.Count; i++)
        {
            form.AddField(fields[i].Key, fields[i].Value);
        }

        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, form))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + bearer);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch id Error 401 Unauthorized
                if (webRequest.responseCode.Equals(401))
                {
                    tryCatchServerError(webRequest.responseCode);
                    presenter.OnServerError(webRequest.responseCode);
                    yield break;
                }

                //Catch if Error 400 Bad Request
                if (webRequest.responseCode.Equals(400))
                {
                    string jsonArray = webRequest.downloadHandler.text;
                    ErrorEntity errorEntity = JsonConvert.DeserializeObject<ErrorEntity>(jsonArray);
                    presenter.OnResult(method, errorEntity);
                    presenter.OnFailedResult(method, errorEntity);
                    yield break;
                }

                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);
                presenter.OnServerError(webRequest.responseCode);
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    //TODO atrapar si es mensaje de que necesita autorizar su correo
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        string jsonArray = webRequest.downloadHandler.text;
                        T genericType = JsonConvert.DeserializeObject<T>(jsonArray);
                        presenter.OnResult(method, genericType);
                        yield break;
                    }
                }
            }

            presenter.OnFailedResult(method);
            yield break;

        }
    }
    protected virtual IEnumerator Get<T>(string uri, string[] parameters = null)
    {
        string url = this.getCompleteRequestURL(uri, parameters);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);
                presenter.OnServerError();
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        string jsonArray = webRequest.downloadHandler.text;
                        T genericType = JsonConvert.DeserializeObject<T>(jsonArray);
                        presenter.OnResult(genericType);
                        yield break;
                    }
                }
            }

            presenter.OnFailedResult();
            yield break;

        }
    }
    protected virtual IEnumerator Get<T>(string uri, Enum method, string[] parameters = null)
    {
        string url = this.getCompleteRequestURL(uri, parameters);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);
                presenter.OnServerError();
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        string jsonArray = webRequest.downloadHandler.text;
                        T genericType = JsonConvert.DeserializeObject<T>(jsonArray);
                        presenter.OnResult(method, genericType);
                        yield break;
                    }
                }
            }

            presenter.OnFailedResult(method);
            yield break;

        }
    }
    protected virtual IEnumerator Get<T>(string uri, string bearer, string[] parameters = null)
    {
        string url = this.getCompleteRequestURL(uri, parameters);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + bearer);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);
                presenter.OnServerError();
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        string jsonArray = webRequest.downloadHandler.text;
                        T genericType = JsonConvert.DeserializeObject<T>(jsonArray);
                        presenter.OnResult(genericType);
                        yield break;
                    }
                }
            }

            presenter.OnFailedResult();
            yield break;

        }
    }
    protected virtual IEnumerator Get<T>(string uri, Enum method, string bearer, string[] parameters = null)
    {
        string url = this.getCompleteRequestURL(uri, parameters);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.SetRequestHeader("Authorization", "Bearer " + bearer);

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                //Catch response code for multiple requests to the server in a short timespan.
                tryCatchServerError(webRequest.responseCode);
                presenter.OnServerError(webRequest.responseCode);
                yield break;
            }
            else
            {
                if (webRequest.isDone)
                {
                    if (ErrorRequestManager.AnaliceResponseCode(webRequest.responseCode))
                    {
                        string jsonArray = webRequest.downloadHandler.text;
                        T genericType = JsonConvert.DeserializeObject<T>(jsonArray);
                        presenter.OnResult(method, genericType);
                        yield break;
                    }
                }
            }

            presenter.OnFailedResult(method, webRequest.responseCode);
            yield break;

        }
    }
}
