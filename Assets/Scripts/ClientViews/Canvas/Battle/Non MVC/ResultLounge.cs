using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultLounge : MonoBehaviour
{
    private Button _leaveButton;
    private Text _scoreText, _winnerText;
    private int _winnerID, _myScore;
    private GameObject _battleLounge, _waitingLounge, _resultLounge;
    
    public void Init()
    {
        _leaveButton = GameObject.Find("Leave Button").GetComponent<Button>();
        _scoreText = GameObject.Find("Current Score").GetComponent<Text>();
        _winnerText = GameObject.Find("Winner").GetComponent<Text>();

        _leaveButton.onClick.AddListener(OnControllerLeaveRoomClicked);
    }

    public void Init(GameObject waitingLounge, GameObject battleLounge, GameObject resultLounge)
    {
        _waitingLounge = waitingLounge;
        _battleLounge = battleLounge;
        _resultLounge = resultLounge;
        this.Init();
    }

    #region Controller
    private void OnControllerAnnounceResult(int[] viewIDs, int[] scores)
    {
        int maxScore = scores[0];
        int winnerID = viewIDs[0];
        for(int i = 0; i < viewIDs.Length; i++)
        {
            if(maxScore < scores[i])
            {
                maxScore = scores[i];
                winnerID = viewIDs[i];
            }

            //if (PlayerManager.IsMine(viewIds[i]))
            //{
            //    _myScore = scores[i];
            //}
            
        }
        this._winnerID = winnerID;
        this.OnViewAnnounceResult();
    }
    #endregion

    #region View
    private void OnViewAnnounceResult()
    {
        //Get winner text name
        //Get current player score
        _winnerText.text = "WINNER: " + _winnerID;
        _scoreText.text = "Your Score: " + _myScore; //+ BattleLounge.points;
        
    }

    private void OnControllerLeaveRoomClicked()
    {
        Debug.Log("Player left room");
        //Send request left room
    }
    #endregion 

    #region Event Registers
    private void OnEnable()
    {
        BattleRoomManager.Instance.OnAnnounceWinner += OnControllerAnnounceResult;
    }

    private void OnDisable()
    {
        BattleRoomManager.Instance.OnAnnounceWinner -= OnControllerAnnounceResult;
    }
    #endregion
}
