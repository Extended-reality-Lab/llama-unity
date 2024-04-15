using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;

public static class LlamaAPI
{
    public static LlamaPromptResponse GetPromptResponse(string model, string prompt, int maxTokens)
    {
        ApiJsonPayload requestPayload = new ApiJsonPayload
        {
            model = model,
            prompt = prompt,
            max_tokens = maxTokens
        };

        // Serialize the payload to JSON
        string jsonPayload = JsonUtility.ToJson(requestPayload);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:1234/v1/completions");
        request.Method = "POST";
        request.ContentType = "application/json";

        // Write the JSON payload to the request body
        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(jsonPayload);
        }

        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        return JsonUtility.FromJson<LlamaPromptResponse>(json);


    }
   
}
