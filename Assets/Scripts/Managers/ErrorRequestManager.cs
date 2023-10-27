using UnityEngine;

public class ErrorRequestManager
{
    public static bool AnaliceResponseCode(long responseCode)
    {
        switch (responseCode.ToString())
        {
            case "0":
                Debug.Log("Response Code: 0 (No wifi)");
                return false;
            case "200":
                Debug.Log("Response Code: 200 Ok (Satisfactory Response)");
                return true;
            case "201":
                Debug.Log("Response Code: 201 Created (Satisfactory Response)");
                return true;
            case "202":
                Debug.Log("Response Code: 201 Accepted (Satisfactory Response)");
                return true;
            case "204":
                Debug.Log("Response Code: 204 No Content (Satisfactory Response)");
                return true;
            case "400":
                Debug.Log("Response Code: 400 Bad Request (Client Error)");
                return false;
            case "401":
                Debug.Log("Response Code: 401 Unauthorized (Client Error)");
                return false;
            case "403":
                Debug.Log("Response Code: 403 Forbidden (Client Error)");
                return false;
            case "404":
                Debug.Log("Response Code: 404 Not Found (Client Error)");
                return false;
            case "429":
                Debug.Log("Response Code: 429 TOO MANY REQUESTS (Server Error)");
                return false;
            case "500":
                Debug.Log("Response Code: 500 Internal Server Error (Server Error)");
                return false;
            case "501":
                Debug.Log("Response Code: 501 Not Implemented (Server Error)");
                return false;
            case "502":
                Debug.Log("Response Code: 502 Bad Gateway (Server Error)");
                return false;
            case "503":
                Debug.Log("Response Code: 503 Service Unavailable (Server Error)");
                return false;
            case "504":
                Debug.Log("Response Code: 504 Gateway Timeout (Server Error)");
                return false;
            default:
                Debug.Log("Response Error" + responseCode + ", not registered");
                return false;
        }
    }
}
