using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        catch (Exception)
        {
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

        return JsonConvert.DeserializeObject<T>(postRequest.downloadHandler.text);
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