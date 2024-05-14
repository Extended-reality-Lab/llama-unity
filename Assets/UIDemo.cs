using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class UIDemo : MonoBehaviour
{
    public TextMeshProUGUI output;
    public TMP_InputField promptUser;

    //void Start()
    //{
    //    
    //}
    public void ButtonDemo()
    {

        output.text = "";

        //GenerateNonStreamingResponse();

        //StartCoroutine(GenerateStreamingResponse());

        //AnythingLLMNonStreaming();
    }

    private void GenerateNonStreamingResponse()
    {
        //Non streaming - Plain LM Studio without RAG

        string model = "TheBloke/Mistral-7B-Instruct-v0.1-GGUF/mistral-7b-instruct-v0.1.Q3_K_L.gguf";
        string prompt = promptUser.text;
        int maxTokens = -1;
        bool stream = false;

        LlamaPromptResponse promptOutput = LlamaAPI.GetPromptResponseNonStreaming(model, prompt, maxTokens, stream);
        output.text = promptOutput.choices[0].text;
    }

    IEnumerator GenerateStreamingResponse()
    {
        //Streaming - Plain LM Studio without RAG

        string model = "TheBloke/Mistral-7B-Instruct-v0.1-GGUF/mistral-7b-instruct-v0.1.Q3_K_L.gguf";
        string prompt = promptUser.text;
        int maxTokens = -1;
        bool stream = true;

        LlamaAPI.GetPromptResponse(model, prompt, maxTokens, stream, (completionResponse) =>
        {
            // Loop through the choices and update output.text
            foreach (var choice in completionResponse.choices)
            {  
                output.text += choice.text;
            }
        });
        yield return null;
    }

    private void AnythingLLMNonStreaming()
    {
        //Streaming - LM Studio + AnythingLLM ie with RAG

        string message = promptUser.text;
        string mode = "query";
        AnythingLLMPromptResponse promptOutput = LlamaAPI.GetAnythingLLMNonStreaming(message, mode);

        output.text = promptOutput.textResponse;

    }



}
