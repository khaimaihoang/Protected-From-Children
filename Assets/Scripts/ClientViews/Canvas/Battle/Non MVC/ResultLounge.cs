using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultLounge : MonoBehaviour
{
    private Button _leaveButton;
    private Text _scoreText, _winnerText;

    private GameObject _battleLounge, _waitingLounge, _resultLounge;
    
    public void Init()
    {
        _leaveButton = GameObject.Find("Leave Button").GetComponent<Button>();
        _scoreText = GameObject.Find("Current Score").GetComponent<Text>();
        _winnerText = GameObject.Find("Winner").GetComponent<Text>();

        _leaveButton.onClick.AddListener(OnControllerLeaveRoomClicked);

        this.OnControllerAnnounceResult();
    }

    public void Init(GameObject waitingLounge, GameObject battleLounge, GameObject resultLounge)
    {
        _waitingLounge = waitingLounge;
        _battleLounge = battleLounge;
        _resultLounge = resultLounge;
        this.Init();
    }

    #region Controller
    private void OnControllerAnnounceResult()
    {
        //Request Server get result
        this.OnViewAnnounceResult();
    }
    #endregion

    #region View
    private void OnViewAnnounceResult()
    {
        //Get winner text name
        //Get current player score
        _scoreText.text = "Your Score: " + BattleLounge.points;
        
    }

    private void OnControllerLeaveRoomClicked()
    {
        Debug.Log("Player left room");
        //Send request left room
    }
    #endregion 
}
