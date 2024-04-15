using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;


public class UIDemo : MonoBehaviour
{
    public TextMeshProUGUI output;
    public TMP_InputField promptUser;

    public void ButtonDemo()
    {

        string model = "TheBloke/Mistral-7B-Instruct-v0.1-GGUF/mistral-7b-instruct-v0.1.Q3_K_L.gguf";
        string prompt = promptUser.text;
        int maxTokens = -1; 

        LlamaPromptResponse promptOutput = LlamaAPI.GetPromptResponse(model, prompt, maxTokens);
        output.text = promptOutput.choices[0].text;
        //output.text = promptUser.text;
    }

}