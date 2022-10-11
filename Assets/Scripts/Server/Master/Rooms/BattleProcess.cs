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
public class BattleProcess : MonoBehaviour
{
    public int numberOfQuestions = 2;
    public int scorePerQuestion = 1;
    private float _timeToWaitPerQuestion = 3f;
    private List<int> _questions = new List<int>();
    private Dictionary<int, string> _answers = new Dictionary<int, string>();

    [SerializeField] private int _state = (int)RoomState.Waiting;

    public List<int> playerList = new List<int>();
    private Dictionary<int, float> _playerAnswerTime = new Dictionary<int, float>();
    private Dictionary<int, int> _playerScores = new Dictionary<int, int>();
    



    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public BattleProcess(int numberOfQuestions, int state, List<int>userId)
    {
        this.numberOfQuestions = numberOfQuestions;
        this._state = state;
        this.playerList = userId;
    }

    private void Init()
    {
        LoadAnswer();
        GenerateQuestion();
        if (_state == (int)RoomState.Playing)
        {
            StartPlaying();
        }
        
    }

    private void SetValues(int numberOfQuestions, int state, List<int> userId)
    {
        this.numberOfQuestions = numberOfQuestions;
        this._state = state;
        this.playerList = userId;
    }

    private void SetState(int state)
    {
        this._state = state;
        if (_state == (int)RoomState.Playing)
        {
            StartPlaying();
        }
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

    private void GenerateQuestion()
    {
        int num = 0;
        for (int i = 0; i < numberOfQuestions; i++)
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

    public void AnswerReceived(int userId, string[] answers)
    {
        foreach (string answer in answers)
        {
            Debug.Log(userId + " answered: " + answer);
        }

        for (int i = 0; i < _questions.Count; i++)
        {
            if (answers[i] == _answers[_questions[i]])
            {
                _playerScores[userId] += scorePerQuestion;
            }
        }
    }

    public void SendQuestions()
    {
        SendReply.Instance.SendQuestions(playerList.ToArray(), _questions.ToArray());
        
    }

    IEnumerator WaitForAnswer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SendReply.Instance.SendScores(_playerScores.Keys.ToArray(), _playerScores.Values.ToArray());
    }

    private void StartPlaying()
    {
        SendQuestions();
        StartCoroutine(WaitForAnswer(numberOfQuestions * _timeToWaitPerQuestion));
    }
}
