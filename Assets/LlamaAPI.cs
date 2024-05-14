using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Net.Http;
using System;
using static LlamaPromptResponse;
using Unity.VisualScripting;
using UnityEditor.VersionControl;

public static class LlamaAPI
{
    
    public static void GetPromptResponse(string model, string prompt, int maxTokens, bool stream, Action<LlamaPromptResponse> callback)
    {
        //Constructing JSON payload
        ApiJsonPayload requestPayload = new ApiJsonPayload
        {
            model = model,
            prompt = prompt,
            max_tokens = maxTokens,
            stream  = stream
        };

        //Serialize the payload to JSON
        string jsonPayload = JsonUtility.ToJson(requestPayload);

        //Creat HTTP POST request 
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:1234/v1/completions");
       
        request.Method = "POST";
        request.ContentType = "application/json";

        // Write the JSON payload to the request body
        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(jsonPayload);
        }



        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            // Read the response stream
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    //to ignore the empty JSONS
                    if (line.Length == 0)
                    {
                        continue;
                    }
                    if (line == "data: [DONE]")
                    {
                        //Debug.Log("Done parsing!");
                        break;
                    }

                    //Deserialize JSON to LlamaPromptResponse object type
                    LlamaPromptResponse completionResponse = JsonUtility.FromJson<LlamaPromptResponse>(line.Substring("data:".Length));
                    //Debug.Log(completionResponse.choices[0].text);
                    
                    callback.Invoke(completionResponse);

                }

            }
        }

    }


    public static LlamaPromptResponse GetPromptResponseNonStreaming(string model, string prompt, int maxTokens, bool stream)
    {
        //Constructing JSON payload
        ApiJsonPayload requestPayload = new ApiJsonPayload
        {
            model = model,
            prompt = prompt,
            max_tokens = maxTokens,
            stream = stream
        };

        //Serialize the payload to JSON
        string jsonPayload = JsonUtility.ToJson(requestPayload);

        //Creat HTTP POST request 
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:1234/v1/completions");

        request.Method = "POST";
        request.ContentType = "application/json";
       

        // Write the JSON payload to the request body
        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(jsonPayload);
        }

        //Non streaming
        //send request and get response
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //read JSON response body
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        //Deserialize JSON to LlamaPromptResponse object type
        return JsonUtility.FromJson<LlamaPromptResponse>(json);

    }

    public static AnythingLLMPromptResponse GetAnythingLLMNonStreaming(string message, string mode)
    {
        //Constructing JSON payload
        AnythingLLMJSONPayload requestPayload = new AnythingLLMJSONPayload
        {
            message = message,
            mode = mode
        };

        string apikey  = "DYWMF1W - 4BA4KJR - G3RQEHM - 00G0KHF";
        //Serialize the payload to JSON
        string jsonPayload = JsonUtility.ToJson(requestPayload);

        //Creat HTTP POST request 
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost:3001/api/v1/workspace/llamaragtest/chat");

        request.Method = "POST";
        request.ContentType = "application/json";
        request.Headers["Authorization"] = "Bearer " + apikey;
        request.UseDefaultCredentials = true;

        // Write the JSON payload to the request body
        using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
        {
            writer.Write(jsonPayload);
        }

        //Non streaming
        //send request and get response
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //read JSON response body
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();
        //Deserialize JSON to LlamaPromptResponse object type
        return JsonUtility.FromJson<AnythingLLMPromptResponse>(json);

    }






}
