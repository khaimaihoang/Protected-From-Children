using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleLoungeView : MonoBehaviour
{
    private Button _AButton, _BButton, _CButton, _DButton;
    private Text _AAnswer, _BAnswer, _CAnswer, _DAnswer;
    private List<Text> _textList;
    private Text _currentQuestion;
    // Start is called before the first frame update
    private void Init()
    {
        _AButton = GameObject.Find("A Button").GetComponent<Button>();
        _BButton = GameObject.Find("B Button").GetComponent<Button>();
        _CButton = GameObject.Find("C Button").GetComponent<Button>();
        _DButton = GameObject.Find("D Button").GetComponent<Button>();

        _AAnswer = _AButton.GetComponentInChildren<Text>();
        _BAnswer = _BButton.GetComponentInChildren<Text>();
        _CAnswer = _CButton.GetComponentInChildren<Text>();
        _DAnswer = _DButton.GetComponentInChildren<Text>();

        _textList = new List<Text>(4);
        _textList.Add(_AAnswer);
        _textList.Add(_BAnswer);
        _textList.Add(_CAnswer);
        _textList.Add(_DAnswer);

        _currentQuestion = GameObject.Find("Question").GetComponentInChildren<Text>();
    }

    private void OnChooseAnswer(string choseAnswer, string rightAnswer)
    {

    }

    private void OnRenewQuestion(QuestionForm questionForm)
    {
        _currentQuestion.text = questionForm.question;
        _textList[0].text = "A. " + questionForm.answers[0];
        _textList[1].text = "B. " + questionForm.answers[1];
        _textList[2].text = "C. " + questionForm.answers[2];
        _textList[3].text = "D. " + questionForm.answers[3];
    }


    private void OnEnable()
    {
        BattleRoomController.Instance.OnInit += Init;
        BattleRoomController.Instance.OnChooseAnswer += OnChooseAnswer;
        BattleRoomController.Instance.OnRenewQuestion += OnRenewQuestion;
    }

    private void OnDisable()
    {
        BattleRoomController.Instance.OnInit -= Init;
        BattleRoomController.Instance.OnChooseAnswer -= OnChooseAnswer;
        BattleRoomController.Instance.OnRenewQuestion -= OnRenewQuestion;
    }
}
