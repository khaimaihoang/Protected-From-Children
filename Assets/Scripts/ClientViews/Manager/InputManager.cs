using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum PlayerInput : int
{
    STOP,
    LEFT,
    UP,
    RIGHT,
    DOWN
}

public enum NetworkEvent : byte
{
    PositionEventCode,
    GetClientPositionEventCode,
    InputEventCode,
    GetWinnerEventCode,
    BattleRequestEventCode,
    GetBattleRequestEventCode,
    GetQuestionsEventCode,
    AnswerEventCode,
    StateChangeEventCode,
    GetScoresEventCode,
    GetReadyEventCode,
    AllInRoomEventCode,
    ExitEventCode,
    RequestNewUidEventCode,
    NewUidAcceptedEventCode,
    RequestCheckNewUidEventCode,
    ChangeNewUidEventCode,
    CreateNewRoomEventCode,
    GetCreatNewRoomEventCode,
    JoinRoomEventCode,
    GetJoinRoomEventCode
}

public class InputManager : MonoSingleton<InputManager>
{
    public string GeneralRoom = "GeneralRoom";

    public float verticalInput;
    public float horizontalInput;

    private float horizontalAxisRaw;
    private float verticalAxisRaw;

    private PlayerInput moveDirection = PlayerInput.STOP;

    public float cameraProjection;
    public float mouseScroll;

    private void HandleMovementInput(){
        verticalInput = Input.GetAxisRaw("Vertical");
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //Debug.Log(verticalInput + " " + horizontalInput);

        cameraProjection = Input.GetAxisRaw("Jump");
        mouseScroll = Input.mouseScrollDelta.y;
        SendInput();
    }
    public void HandleAllInput(){
        HandleMovementInput();
    }

    void SendInput()
    {
        horizontalAxisRaw = horizontalInput;
        verticalAxisRaw = verticalInput; 

         PlayerInput tempMoveDirection = moveDirection;
        if (horizontalAxisRaw == -1)
        {
            moveDirection = PlayerInput.LEFT;
        }
        if (verticalAxisRaw == 1)
        {
            moveDirection = PlayerInput.UP;
        }
        if (horizontalAxisRaw == 1)
        {
            moveDirection = PlayerInput.RIGHT;
        }
        if (verticalAxisRaw == -1)
        {
            moveDirection = PlayerInput.DOWN;
        }
        if (horizontalAxisRaw == 0 && verticalAxisRaw == 0)
        {
            moveDirection = PlayerInput.STOP;
        }

        if (tempMoveDirection != moveDirection)
        {
            GameObject.FindObjectOfType<SendRequest>()?.SendPlayerInputRequest((int)moveDirection);
        }

    }
}
