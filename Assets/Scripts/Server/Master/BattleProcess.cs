using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public enum PlayerState
{
    None,
    Ready,
}
public class BattleProcess : MonoSingleton<BattleProcess>
{
    public int numOfQuestions = 2;
    public int scorePerQuestion = 1;


    private Dictionary<int, int> _playerState = new Dictionary<int, int>();
    [SerializeField] private bool _isPlaying = false;
    private Dictionary<int, float> _playerAnswerTime = new Dictionary<int, float>();
    private Dictionary<int, int> _playerScores = new Dictionary<int, int>();
    public List<int> playerList = new List<int>();
    private List<int> _questions = new List<int>();
    private Dictionary<int, string> _answers = new Dictionary<int, string>();

    private float _timeToWaitPerSecond = 3f;
    // Start is called before the first frame update
    void Start()
    {
        LoadAnswer();
        GenerateQuestion();

        //Init Client's Battle View
        //GameObject.Find("BattleRoomManager").GetComponent<WaitingLounge>().Init();
    }

    private void LoadAnswer()
    {
        //TextAsset text = ;
        string[] lines = Resources.Load<TextAsset>("answers").text.Split('\n');
        string[] answer;
        for (int i = 1; i < lines.Length; i++)
        {
            answer = lines[i].Split(',');
            _answers.Add(int.Parse(answer[0]), answer[1]);
            Debug.Log(answer[0] + " - " + _answers[int.Parse(answer[0])]);
        }
    }

    void GenerateQuestion()
    {
        int num = 0;
        for (int i = 0; i < numOfQuestions; i++)
        {
            num = UnityEngine.Random.Range(0, _answers.Count);
            while (_questions.Contains(num))
            {
                num = UnityEngine.Random.Range(0, _answers.Count);
            }
            _questions.Add(num);
            Debug.Log(_questions[i]);
        }
    }

    public void AddNewPlayer(int viewId)
    {
        if (!playerList.Contains(viewId))
        {
            playerList.Add(viewId);
            _playerScores.Add(viewId, 0);
            _playerState.Add(viewId, (int)PlayerState.None);
        }

    }

    public void PlayerStateChange(int viewId, int state)
    {
        if (!_playerState.ContainsKey(viewId))
        {
            _playerState.Add(viewId, state);
        }
        else
        {
            _playerState[viewId] = state;
        }

        if (_playerState.Values.Distinct().Skip(1).Any())
        {
            if (_playerState.Values.First() == (int)PlayerState.Ready)
            {
                _isPlaying = true;
                Debug.Log("Battle start!");
                SendQuestions();

            }
        }
    }

    public void AnswerReceived(int viewId, string[] answers)
    {
        foreach (string answer in answers)
        {
            Debug.Log(viewId + " answered: " + answer);
        }

        for (int i = 0; i < _questions.Count; i++)
        {
            if (answers[i] == _answers[_questions[i]])
            {
                _playerScores[viewId] += scorePerQuestion;
            }
        }
    }

    public void SendQuestions()
    {
        SendReply.Instance.SendQuestions(_questions.ToArray());
        StartCoroutine(WaitForAnswer(numOfQuestions * _timeToWaitPerSecond));
    }

    public void StartWaitForReady()
    {
        StartCoroutine(WaitForReady(2));
    }

    IEnumerator WaitForAnswer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SendReply.Instance.SendScores(_playerScores.Keys.ToArray(), _playerScores.Values.ToArray());
    }

    IEnumerator WaitForReady(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (!_isPlaying) SendReply.Instance.SendReadyState(false);
    }

    public void ExitGame()
    {
        SendReply.Instance.SendExitReply();
    }
}
