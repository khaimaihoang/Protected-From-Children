using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleLounge : MonoBehaviour
{
    public static float points = 0; //delete after connect to server

    private Button _AButton, _BButton, _CButton, _DButton;
    private Text _AAnswer, _BAnswer, _CAnswer, _DAnswer;
    //private List<Text> _textList;
    private Text _currentQuestion, _timerText;
    private GameObject _waitingLounge, _battleLounge, _resultLounge;

    private List<QuestionForm> _questForms;
    public List<string> _playerAnswers;
    private int currentIdx;

    public float timerPerQuestion;
    private float _currentTimer;
    private bool _startTimer, _transitionTimer = false;

    private void Update()
    {
        if (_startTimer)
        {
            this.OnControllerTimer();
        }
    }
    public void Init()
    {
        _AButton = GameObject.Find("A Button").GetComponent<Button>();
        _BButton = GameObject.Find("B Button").GetComponent<Button>();
        _CButton = GameObject.Find("C Button").GetComponent<Button>();
        _DButton = GameObject.Find("D Button").GetComponent<Button>();

        _AAnswer = _AButton.GetComponentInChildren<Text>();
        _BAnswer = _BButton.GetComponentInChildren<Text>();
        _CAnswer = _CButton.GetComponentInChildren<Text>();
        _DAnswer = _DButton.GetComponentInChildren<Text>();

        //_textList = new List<Text>(4);
        //_textList.Add(_AAnswer);
        //_textList.Add(_BAnswer);
        //_textList.Add(_CAnswer);
        //_textList.Add(_DAnswer);

        _currentQuestion = GameObject.Find("Question").GetComponentInChildren<Text>();
        _timerText = GameObject.Find("Timer").GetComponentInChildren<Text>();

        currentIdx = 0;
        _questForms = QuestionLoader.LoadQuestion();
        _AButton.onClick.AddListener(() => { OnControllerAnswerClicked(_AButton); });
        _BButton.onClick.AddListener(() => { OnControllerAnswerClicked(_BButton); });
        _CButton.onClick.AddListener(() => { OnControllerAnswerClicked(_CButton); });
        _DButton.onClick.AddListener(() => { OnControllerAnswerClicked(_DButton); });

        _currentTimer = timerPerQuestion + 1;
        this.OnControllerRenewQuestion();

    }

    public void Init(GameObject waitingLounge, GameObject battleLounge, GameObject resultLounge)
    {
        _waitingLounge = waitingLounge;
        _battleLounge = battleLounge;
        _resultLounge = resultLounge;
        this.Init();
    }

    #region Controller
    private void OnControllerAnswerClicked(Button choseButton)
    {
        if (currentIdx < _questForms.Count)
        {
            if(choseButton != null)
            {
                _playerAnswers.Add(choseButton.GetComponentInChildren<Text>().text[0] + "");

                //delete after connect to server
                if(_playerAnswers[_playerAnswers.Count - 1] == _questForms[currentIdx - 1].correctAnswer)
                {
                    points++;
                }
            }
            else
            {
                _playerAnswers.Add("Null");
            }
            //StartCoroutine(WaitRenewQuestion());
            this.OnControllerRenewQuestion();
        }
        else
        {
            Debug.Log("End game");
            //foreach (var answer in _playerAnswers)
            //{
            //    Debug.Log(answer);
            //}
            this.OnControllerEndBattle();
        }
    }

    private void OnControllerRenewQuestion()
    {
        this.OnViewRenewQuestion(_questForms[currentIdx]);
        currentIdx++;

        _currentTimer = timerPerQuestion + 1;
        _startTimer = true;
        //this.OnControllerWaitTimerBetweenQuestion(1.5f);
    }

    private void OnControllerEndBattle()
    {
        _battleLounge.SetActive(false);
        //wait for others player or times up
        //if others not finished -> return wait for others
        //else
        //Load result
        if (false)
        {
            return;
        }
        else
        {
            _resultLounge.SetActive(true);
            GetComponent<ResultLounge>().Init(_waitingLounge, _battleLounge, _resultLounge);
        }
        
    }

    private void OnControllerTimer()
    {
        if(_currentTimer > 0)
        {
            _currentTimer -= Time.deltaTime;
        }
        else
        {
            _currentTimer = 0;
            _startTimer = false;
            this.OnControllerAnswerClicked(null);
        }
        this.OnViewTimer();
    }

    private void OnControllerWaitTimerBetweenQuestion(float waitTime)
    {
        
    }
    #endregion

    #region View
    private void OnViewRenewQuestion(QuestionForm questionForm)
    {
        _currentQuestion.text = questionForm.question;
        _AAnswer.text = "A. " + questionForm.answers[0];
        _BAnswer.text = "B. " + questionForm.answers[1];
        _CAnswer.text = "C. " + questionForm.answers[2];
        _DAnswer.text = "D. " + questionForm.answers[3];
    }

    private void OnViewTimer()
    {
        if(_currentTimer < 5)
        {
            _timerText.color = Color.red;
        }
        else if(!_timerText.color.Equals(Color.black))
        {
            _timerText.color = Color.black;
        }
        _timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(_currentTimer), (Mathf.Abs(_currentTimer - Mathf.FloorToInt(_currentTimer)) * 100));
    }
    #endregion
}
