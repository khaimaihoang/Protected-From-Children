using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleLounge : MonoBehaviour
{
    private Button _AButton, _BButton, _CButton, _DButton;
    private Text _AAnswer, _BAnswer, _CAnswer, _DAnswer;
    //private List<Text> _textList;
    private Text _currentQuestion, _timerText;
    private GameObject _waitingLounge, _battleLounge, _resultLounge;

    private List<QuestionForm> _questForms;
    public List<string> _playerAnswers;
    private int currentIdx;
    private int[] _questionIds;

    public float timerPerQuestion;
    private float _currentTimer;
    private bool _startTimer, _doneWaiting = false;

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
        timerPerQuestion = 3;
        _playerAnswers = new List<string>();
        _questForms = QuestionLoader.LoadQuestion();
        _AButton.onClick.AddListener(() => { OnControllerAnswerClicked(_AButton); });
        _BButton.onClick.AddListener(() => { OnControllerAnswerClicked(_BButton); });
        _CButton.onClick.AddListener(() => { OnControllerAnswerClicked(_CButton); });
        _DButton.onClick.AddListener(() => { OnControllerAnswerClicked(_DButton); });

        _currentTimer = timerPerQuestion;
        this.OnControllerRenewQuestion();

    }

    public void Init(GameObject waitingLounge, GameObject battleLounge, GameObject resultLounge, int[] questions)
    {
        _waitingLounge = waitingLounge;
        _battleLounge = battleLounge;
        _resultLounge = resultLounge;

        _questionIds = questions;
        this.Init();
    }

    #region Controller
    private void OnControllerAnswerClicked(Button choseButton)
    {
        if (choseButton != null)
        {
            _playerAnswers.Add(choseButton.GetComponentInChildren<Text>().text[0] + "");
        }
        else
        {
            _playerAnswers.Add("Null");
        }

        if (currentIdx >= _questionIds.Length)
        {
            Debug.Log("End battle");
            _battleLounge.SetActive(false);
            BattleRoomManager.Instance.RequestOnSendAnswers(_playerAnswers.ToArray());
            //this.OnControllerEndBattle();
            StartCoroutine(WaitForOtherToFinish());
            return;
        }
        this.OnControllerRenewQuestion();
    }

    private void OnControllerRenewQuestion()
    {
        QuestionForm myQuestion = _questForms.Find(x => x.id == _questionIds[currentIdx]);
        if(myQuestion != null)
        {
            this.OnViewRenewQuestion(myQuestion);
        }
        currentIdx++;

        _currentTimer = timerPerQuestion;
        _startTimer = true;
    }

    private void OnControllerEndBattle()
    {
        _doneWaiting = true;
        //_battleLounge.SetActive(false);
        _resultLounge.SetActive(true);
        this.GetComponent<ResultLounge>().Init(_waitingLounge, _battleLounge, _resultLounge);  
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
        if(_currentTimer < timerPerQuestion / 2)
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

    #region Event Registers
    private void OnEnable()
    {
        BattleRoomManager.Instance.OnEndBattle += OnControllerEndBattle;
    }

    private void OnDisable()
    {
        BattleRoomManager.Instance.OnEndBattle -= OnControllerEndBattle;
    }
    #endregion

    #region Coroutines
    IEnumerator WaitForOtherToFinish()
    {
        Debug.Log("Waiting...");
        yield return new WaitUntil(WaitForScores);
        Debug.Log("Wait Done!");
    }

    private bool WaitForScores()
    {
        return _doneWaiting;
    }
    #endregion
}
