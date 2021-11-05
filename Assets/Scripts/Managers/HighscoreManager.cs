using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HighscoreManager : MonoBehaviour
{
    [SerializeField]
    bool postData = true;

    private static HighscoreManager _instance;
    public static HighscoreManager instance { get { return _instance; } }

    private const string privateCode = "";
    private const string publicCode = "";

    private const string url = "http://dreamlo.com/lb/";

    public Highscore[] highscores;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    private void Start()
    {
       // GetHighscores(3);
    }
    public void AddHighscore(string username, float newScore)
    {
        instance.StartCoroutine(instance.PostHighscore(username, newScore));
    }

    IEnumerator PostHighscore(string username, float newScore)
    {
        //TODO: Divide highscores depending on roundtype 
      //  int scoreVersion = (int)(newScore * 1000000);
        UnityWebRequest www = new UnityWebRequest();
        www = new UnityWebRequest(url + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + newScore);


        yield return www.SendWebRequest();
    }

    public void GetHighscores()
    {
        instance.StartCoroutine(instance.GetHighscoresRequest());
    }

    IEnumerator GetHighscoresRequest()
    {
        UnityWebRequest www = new UnityWebRequest();
        www = new UnityWebRequest(url + publicCode + "/pipe/");
        
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.downloadHandler.text);
           // MenuManager.instance.FetchedHighscore(highscores);
        }

    }

    void FormatHighscores(string data)
    {
        //TODO: Add different round types
        string[] scores = data.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscores = new Highscore[scores.Length];

        for (int i = 0; i < scores.Length; i++)
        {
            // string[] scoreInformation = scores[i].Split(new char[] { '|' });
            string[] scoreInformation = scores[i].Split('|');
            string username = scoreInformation[0];
           // float score = ((float)int.Parse(scoreInformation[1])) / 1000000;
            float score = ((float)int.Parse(scoreInformation[1]));
            highscores[i] = new Highscore(username, score);
        }

    }

}

public struct Highscore
{
    public string username;
    public float score;
    public Highscore(string _username, float _score)
    {
        username = _username;
        score = _score;
    }
}