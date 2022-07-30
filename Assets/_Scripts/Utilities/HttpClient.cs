using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public static class HttpClient
{
    public static async Task<T> Get<T>(string endpoint)
    {
        var getRequest = CreateRequest(endpoint, RequestType.GET);
        getRequest.SendWebRequest();

        while (!getRequest.isDone)
        {
            await Task.Delay(10);
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(getRequest.downloadHandler.text);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"HttpClient: GET from {endpoint}" +
                           $"\nSent object type: {typeof(T)}" +
                           $"\nError: {getRequest.error}" +
                           $"\n\n" +
                           $"See details in a exception logged below." +
                           $"\n\n");
            Debug.LogException(e);
            return default(T);
        }
    }

    public static async Task<T> Post<T>(string endpoint, object payload)
    {
        var postRequest = CreateRequest(endpoint, RequestType.POST, payload);
        postRequest.SendWebRequest();
        
        while (!postRequest.isDone)
        {
            await Task.Delay(10);
        }

        while (!postRequest.downloadHandler.isDone && postRequest.result != UnityWebRequest.Result.ProtocolError && postRequest.result != UnityWebRequest.Result.ConnectionError)
        {
            await Task.Delay(10);
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"HttpClient: POST to {endpoint}" +
                             $"\nSent object type: {payload.GetType()}" +
                             $"\nRetrieved object type: {typeof(T)}" +
                             $"\nError: {postRequest.error}" +
                             $"\n\n" +
                             $"See details in a exception logged below." +
                             $"\n\n");
            Debug.LogException(e);
            return default(T);
        }
    }

    private static UnityWebRequest CreateRequest(string path, RequestType type, object data = null)
    {
        var request = new UnityWebRequest(path, type.ToString());

        if (data != null)
        {
            var bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        if (SessionStore.Jwt != null)
        {
            request.SetRequestHeader("Authorization", $"Bearer {SessionStore.Jwt}");
        }

        request.certificateHandler = new CertificateWhore();

        return request;
    }

    private enum RequestType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}

public class CertificateWhore : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}