using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonScript : EventTrigger
{
    public InitialScript InitialScriptSc;
    public int x, y;
    public Image PlayerSign;
    void Start()
    {
        PlayerSign = this.GetComponent<Image>();
        InitialScriptSc = GameObject.FindObjectOfType<InitialScript>();
    }
    

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (InitialScriptSc.EndGame == false)
        {
            int Pos = Int32.Parse(String.Concat(this.name[5]));

            if (Pos == 1)
            {
                x = 0;
                y = 0;
            }
            else if (Pos == 2)
            {
                x = 0;
                y = 1;
            }
            else if (Pos == 3)
            {
                x = 0;
                y = 2;
            }
            else if (Pos == 4)
            {
                x = 1;
                y = 0;
            }
            else if (Pos == 5)
            {
                x = 1;
                y = 1;
            }
            else if (Pos == 6)
            {
                x = 1;
                y = 2;
            }
            else if (Pos == 7)
            {
                x = 2;
                y = 0;
            }
            else if (Pos == 8)
            {
                x = 2;
                y = 1;
            }
            else if (Pos == 9)
            {
                x = 2;
                y = 2;
            }


            if (InitialScriptSc.PlayerTurn % 2 == 0 && InitialScriptSc.RealBoard[x, y] == 'Z')
            {
                /*
                PlayerSign.sprite = InitialScriptSc.SignX;
                InitialScriptSc.RealBoard[x, y] = 'X';
                InitialScriptSc.PlayerTurn++;

                InitialScriptSc.WinnerIS(InitialScriptSc.RealBoard, true, InitialScriptSc.PlayerTurn);
                 * */
            }
            else if (InitialScriptSc.PlayerTurn % 2 == 1 && InitialScriptSc.RealBoard[x, y] == 'Z')
            {
                PlayerSign.sprite = InitialScriptSc.SignO;
                InitialScriptSc.RealBoard[x, y] = 'O';
                InitialScriptSc.PlayerTurn++;

                InitialScriptSc.WinnerIS(InitialScriptSc.RealBoard, true, InitialScriptSc.PlayerTurn,0);

                if (!InitialScriptSc.EndGame)
                {
                    InitialScriptSc.PlayerTurnText.text = "Bot's Turn";
                    InitialScriptSc.PlayerTurnText.GetComponent<Text>().color = Color.red;

                    InitialScriptSc.StartCoroutine("PC_Move");
                }
                
            }
        }
    }

}
