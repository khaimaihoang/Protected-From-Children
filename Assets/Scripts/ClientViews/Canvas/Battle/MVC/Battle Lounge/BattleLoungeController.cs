using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleLoungeController : MonoBehaviour
{
    private Button _AButton, _BButton, _CButton, _DButton;
    private List<QuestionForm> _questForms;
    public List<string> _playerAnswers;
    private int currentIdx;
    // Start is called before the first frame update
    private void Start()
    {
        //BattleRoomController.Instance.RequestOnInit();
    }
    private void Init()
    {
        currentIdx = 0;
        _AButton = GameObject.Find("A Button").GetComponent<Button>();
        _BButton = GameObject.Find("B Button").GetComponent<Button>();
        _CButton = GameObject.Find("C Button").GetComponent<Button>();
        _DButton = GameObject.Find("D Button").GetComponent<Button>();

        _questForms = QuestionLoader.LoadQuestion();
        _AButton.onClick.AddListener(() => { OnAnswerClicked(_AButton); });
        _BButton.onClick.AddListener(() => { OnAnswerClicked(_BButton); });
        _CButton.onClick.AddListener(() => { OnAnswerClicked(_CButton); });
        _DButton.onClick.AddListener(() => { OnAnswerClicked(_DButton); });

    }

    private void OnAnswerClicked(Button choseButton)
    {
        if(currentIdx < _questForms.Count)
        {
            _playerAnswers.Add(choseButton.GetComponentInChildren<Text>().text[0] + "");
            BattleRoomController.Instance.RequestOnChooseAnswer(choseButton.GetComponentInChildren<Text>().text, _questForms[currentIdx].correctAnswer);
            this.OnRenewQuestion();
        }
        else
        {
            Debug.Log("End game");
            foreach(var answer in _playerAnswers)
            {
                Debug.Log(answer);
            }
        }
        
    }

    private void OnRenewQuestion()
    {
        BattleRoomController.Instance.RequestOnRenewQuestion(_questForms[currentIdx]);
        currentIdx++;
    }

    private void OnEnable()
    {
        BattleRoomController.Instance.OnInit += Init;
        BattleRoomController.Instance.OnStartBattle += OnRenewQuestion;
    }

    private void OnDisable()
    {
        BattleRoomController.Instance.OnInit -= Init;
        BattleRoomController.Instance.OnStartBattle -= OnRenewQuestion;
    }
}
