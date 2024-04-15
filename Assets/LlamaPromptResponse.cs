[System.Serializable]
public class LlamaPromptResponse
{
    public string id;
    public string @object;
    public long created;
    public string model;
    public Choice[] choices;
    public Usage usage;

    [System.Serializable]
    public class Choice
    {
        public int index;
        public string text;
        public object logprobs; 
        public string finish_reason;
    }

    [System.Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }


}
