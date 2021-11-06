using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class InitialScript : MonoBehaviour
{

    public char[,] RealBoard = { { 'Z', 'Z', 'Z' }, { 'Z', 'Z', 'Z' }, { 'Z', 'Z', 'Z' } };
    public int PlayerTurn = 1;

    public Sprite SignO, SignX;

    public bool EndGame = false;

    public Image[] Position;


    public GameObject ResultPanel;
    public Text ResultText;

    public Text PlayerTurnText;

    int winType;

    public GameObject[] MatchImage;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerTurn % 2 == 1)
        {
            PlayerTurnText.text = "Player's Turn";
            PlayerTurnText.GetComponent<Text>().color = Color.green;
        }
        else
        {
            PlayerTurnText.text = "Bot's Turn";
            PlayerTurnText.GetComponent<Text>().color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (PlayerTurn % 2 == 0 && !EndGame)
        {
            PlayerTurn++;

            int[] result = new int[2];
            result = FindBestTurn(RealBoard);
            
            int ind = IndexToPos(result[0], result[1]);
            Position[ind].sprite = SignX;
        }
         * */

        /*
        for (int i = 0; i < 3; i++)
        {

            print(RealBoard[i, 0] + " " + RealBoard[i, 1] + " " + RealBoard[i, 2]);
        }
        */
    }

    public void SeeRealBoardValue()
    {
        
        SceneManager.LoadScene("MainGame");
        
    }

    IEnumerator PC_Move()
    {
        //Debug.Log("Getting PC_Move .... ");
        //yield return new WaitForSeconds(0.5f);
        yield return null;

        int[] result = new int[2];
        result = FindBestTurn(RealBoard);

        int ind = IndexToPos(result[0], result[1]);

        //Debug.Log(result[0] + " " + result[1]);

        RealBoard[result[0], result[1]] = 'X';
        
        Position[ind].sprite = SignX;

        PlayerTurn++;

        WinnerIS(RealBoard, true, PlayerTurn, 0);

        PlayerTurnText.text = "Player's Turn";
        PlayerTurnText.GetComponent<Text>().color = Color.green;
        //Debug.Log("Best Move is :  " + ind);
    }


    public int[] FindBestTurn(char[,] Board)
    {
        int posx = -1;
        int posy = -1;

        int bestResult = -999999999;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (Board[i, j] == 'Z')
                {
                    //int ind = IndexToPos(i,j);
                    //Position[ind].sprite = SignX;

                    Board[i, j] = 'X';

                    int CurResult = minAnalyzer(Board, 1, PlayerTurn + 1);

                    Board[i, j] = 'Z';

                    if (CurResult >= bestResult)
                    {
                        bestResult = CurResult;
                        posx = i;
                        posy = j;
                    }


                    //WinnerIS();
                    //return;
                }
            }
        }

        int[] result = new int[2];
        result[0] = posx;
        result[1] = posy;

        return result;

    }


    public int minAnalyzer(char[,] Board, int depth, int moveNo)
    {
        if (WinnerIS(Board, false, moveNo, depth) == 1)
        {
            int BestValue = 999999999;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j] == 'Z')
                    {
                        Board[i,j] = 'O';
                        BestValue = Mathf.Min(BestValue, maxAnalyzer(Board, depth + 1, moveNo + 1));
                        Board[i, j] = 'Z';
                    }
                }
            }
            return BestValue;
        }

        return WinnerIS(Board, false, moveNo, depth);
    }
    public int maxAnalyzer(char[,] Board, int depth, int moveNo)
    {
        if (WinnerIS(Board, false, moveNo, depth) == 1)
        {
            int BestValue = -999999999;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Board[i, j] == 'Z')
                    {
                        Board[i, j] = 'X';
                        BestValue = Mathf.Max(BestValue, minAnalyzer(Board, depth + 1, moveNo + 1));
                        Board[i, j] = 'Z';
                    }
                }
            }
            return BestValue;
        }

        return WinnerIS(Board, false, moveNo, depth);
    }

    public int WinnerIS(char[,] Board ,bool isRealBoard ,int PlayerTurnX, int depth)
    {
        char winner = WinCheck(Board);

        if (winner == 'Z' && PlayerTurnX == 10)
        {
            //Debug.Log("No win");
            if (isRealBoard)
            {
                EndGame = true;
                //Debug.Log("No win");
                winType = 0;
                StartCoroutine("WinResult", "Tie");
               
            }
            
            return 0;
        }
        else if (winner == 'X')
        {
            //Debug.Log("X win");
            if (isRealBoard)
            {
                EndGame = true;
                //Debug.Log("X win");

                StartCoroutine("WinResult", "Bot Is Winner");
                
            }

            return 100 - depth;
        }
        else if (winner == 'O')
        {
            //Debug.Log("O win");
            if (isRealBoard)
            {
                EndGame = true;
                //Debug.Log("O win");
                StartCoroutine("WinResult", "You Wins");
                
            }

            return -100;
        }

        return 1;
    }

    IEnumerator WinResult(string r)
    {
        yield return new WaitForSeconds(.5f);

        ResultText.text = r;
        
        if(winType != 0){
            MatchImage[winType-1].SetActive(true);
            StartCoroutine("CheckMateImageDraw", MatchImage[winType-1]);
        }
        else
        {
            yield return new WaitForSeconds(1f);
            ResultPanel.SetActive(true);
        }
        
    }
    IEnumerator CheckMateImageDraw(GameObject CheckImage)
    {
        yield return new WaitForSeconds(.01f);

        for (int i = 0; i < 100; i++)
        {
            CheckImage.GetComponent<Image>().fillAmount += 0.01f;
            yield return new WaitForSeconds(.005f);
        }

        yield return new WaitForSeconds(1.5f);
        ResultPanel.SetActive(true);
    }



    public char WinCheck(char[,] M)
    {
        char Winner = 'Z';

        if (M[0, 0] == M[0, 1] && M[0, 1] == M[0, 2])  //1
        {
            Winner = M[0, 0];
            winType = 1;
        }
        else if (M[1, 0] == M[1, 1] && M[1, 1] == M[1, 2]) //2
        {
            Winner = M[1, 0];
            winType = 2;
        }
        else if (M[2, 0] == M[2, 1] && M[2, 1] == M[2, 2]) //3
        {
            Winner = M[2, 0];
            winType = 3;
        }
        else if (M[0, 0] == M[1, 0] && M[1, 0] == M[2, 0]) //4
        {
            Winner = M[0, 0];
            winType = 4;
        }
        else if (M[0, 1] == M[1, 1] && M[1, 1] == M[2, 1]) //5
        {
            Winner = M[0, 1];
            winType = 5;
        }
        else if (M[0, 2] == M[1, 2] && M[1, 2] == M[2, 2]) //6
        {
            Winner = M[0, 2];
            winType = 6;
        }
        else if (M[0, 0] == M[1, 1] && M[1, 1] == M[2, 2]) //7
        {
            Winner = M[0, 0];
            winType = 7;
        }
        else if (M[0, 2] == M[1, 1] && M[1, 1] == M[2, 0]) //8
        {
            Winner = M[0, 2];
            winType = 8;
        }

        return Winner;

    }


    public int IndexToPos(int x, int y)
    {
        if (x == 0 && y == 0)
        {
            return 0;
        }
        else if (x == 0 && y == 1)
        {
            return 1;
        }
        else if (x == 0 && y == 2)
        {
            return 2;
        }

        else if (x == 1 && y == 0)
        {
            return 3;
        }
        else if (x == 1 && y == 1)
        {
            return 4;
        }
        else if (x == 1 && y == 2)
        {
            return 5;
        }

        else if (x == 2 && y == 0)
        {
            return 6;
        }
        else if (x == 2 && y == 1)
        {
            return 7;
        }
        else if (x == 2 && y == 2)
        {
            return 8;
        }
        return 9;
    }


    


    /*
    public int minimax(int[,] board, int depth,bool isMaximizing) {

    int result = WinnerIS();
  if (result != 1) {
    return scores[result];
  }

  if (isMaximizing) {
    int bestScore = -100000;
    for (int i = 0; i < 3; i++) {
      for (int j = 0; j < 3; j++) {
        // Is the spot available?
        if (board[i,j] == 'Z') {
          board[i,j] = 'X';
          int score = minimax(board, depth + 1, false);
          board[i,j] = 'Z';
            if(score > bestScore){
                bestScore = score;
            }
          //bestScore = Mathf.max(score, bestScore);
        }
      }
    }
    return bestScore;
  } else {
    int bestScore = 100000;
    for (int i = 0; i < 3; i++) {
      for (int j = 0; j < 3; j++) {
        // Is the spot available?
        if (board[i, j] == 'Z') {
          board[i, j] = 'O';
          int score = minimax(board, depth + 1, true);
          M[i,j] = 'Z';

            if(score < bestScore){
                bestScore = score;
            }
          //bestScore = min(score, bestScore);
        }
      }
    }
    return bestScore;
  }
}
    */

}
