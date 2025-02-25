using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class Fixed00 : MonoBehaviour
{
    //9x9 grid broken down into 4 dimensions: 3 lines of 3x3 squares and 3 lines of 1x3 tiles
    //Coord example: currPos for 73 is numGrid[2,0,1,1] (format is numGrid [currLnSq,currSq,currLn,currPos])
    public readonly int[,,,] numGrid = new int[,,,] { { { { 60,02,15 },{ 88,46,31 },{ 74,27,53 } },
                                                    { { 57,36,83 },{ 70,22,64 },{ 05,41,18 } },
                                                    { { 48,71,24 },{ 07,55,13 },{ 86,30,62 } } },
                                                    { { { 52,10,04 },{ 33,65,78 },{ 47,81,26 } },
                                                    { { 43,85,37 },{ 21,00,56 },{ 68,14,72 } },
                                                    { { 61,28,76 },{ 12,44,87 },{ 59,93,35 } } },
                                                    { { { 06,38,42 },{ 25,73,67 },{ 11,54,80 } },
                                                    { { 84,63,20 },{ 16,58,01 },{ 32,77,45 } },
                                                    { { 75,17,51 },{ 34,82,40 },{ 23,66,08 } } } };
    public int currPos;
    public int currLn;
    public int currSq;
    public int currLnSq;
    
    System.Random _rdm = new System.Random();

    bool obsfusticationDisabled = false;
    bool actionDisabled = false;

    //Determine functions' direction
    public bool svDefault;
    public bool shDefault;
    public bool lvDefault;
    public bool lhDefault;
    //Game objects
    public GameObject buttonSH;
    public GameObject buttonSV;
    public GameObject buttonLH;
    public GameObject buttonLV;
    public GameObject buttonSubmit;
    public GameObject buttonRotate;
    public TMP_InputField displayNumber;
    public AudioSource audioSource;
    public AudioClip jackpotAudioClip;
    public AudioClip sirenAudioClip;
    public AudioClip coinFailAudioClip;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttonRotate = GameObject.Find("Rotate").GetComponent<GameObject>();

        int _Min = 0;
        int _Max = 2;
        //Randomize starting position
        currLnSq = _rdm.Next(_Min, _Max);
        currSq = _rdm.Next(_Min, _Max);
        currLn = _rdm.Next(_Min, _Max);
        currPos = _rdm.Next(_Min, _Max);
        svDefault = _rdm.NextDouble() >= 0.5;
        shDefault = _rdm.NextDouble() >= 0.5;
        lvDefault = _rdm.NextDouble() >= 0.5;
        lhDefault = _rdm.NextDouble() >= 0.5;

        getNumberToDisplay(currLnSq, currSq, currLn, currPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (!obsfusticationDisabled)
        {
            if (getNumberInt(currLnSq, currSq, currLn, currPos) < 10)
            {
                displayNumber.text = String.Format("{0:00}", _rdm.Next(0, 9));
            }
        }
    }

    int getNumberInt(int currLnSq, int currSq, int currLn, int currPos)
    {
        return numGrid[currLnSq, currSq, currLn, currPos];
    }

    string getNumber(int currLnSq, int currSq, int currLn ,int currPos)
    {
        return String.Format("{0:00}", numGrid[currLnSq, currSq, currLn, currPos]);
    }

    void getNumberToDisplay(int currLnSq , int currSq, int currLn ,int currPos)
    {
        displayNumber.text = getNumber(currLnSq, currSq, currLn, currPos);
    }

    public void btnSmallVertical() 
    {
        if (!actionDisabled)
        {
            //Default (true) is down
            if (svDefault)
            {
                currLn += 1;
                if (currLn > numGrid.GetLength(2) - 1) currLn = 0;
                else if (currLn < 0) currLn = numGrid.GetLength(2) - 1;
            }
            else
            {
                currLn -= 1;
                if (currLn > numGrid.GetLength(2) - 1) currLn = 0;
                else if (currLn < 0) currLn = numGrid.GetLength(2) - 1;
            }
            getNumberToDisplay(currLnSq, currSq, currLn, currPos);
        }
    }
    public void btnSmallHorizontal() 
    {
        if (!actionDisabled)
        {
            //Default (true) is right
            if (shDefault)
            {
                currPos += 1;
                if (currPos > numGrid.GetLength(3) - 1) currPos = 0;
                else if (currPos < 0) currPos = numGrid.GetLength(3) - 1;
            }
            else
            {
                currPos -= 1;
                if (currPos > numGrid.GetLength(3) - 1) currPos = 0;
                else if (currPos < 0) currPos = numGrid.GetLength(3) - 1;
            }
            getNumberToDisplay(currLnSq, currSq, currLn, currPos);
        }
    }
    public void btnLargeVertical() 
    {
        if (!actionDisabled)
        {
            //Default true is down
            if (lvDefault)
            {
                currLnSq += 1;
                if (currLnSq > numGrid.GetLength(0) - 1) currLnSq = 0;
                else if (currLnSq < 0) currLnSq = numGrid.GetLength(0) - 1;
            }
            else
            {
                currLnSq -= 1;
                if (currLnSq > numGrid.GetLength(0) - 1) currLnSq = 0;
                else if (currLnSq < 0) currLnSq = numGrid.GetLength(0) - 1;
            }
            getNumberToDisplay(currLnSq, currSq, currLn, currPos);
        }
    }
    public void btnLargeHorizontal() 
    {
        if (!actionDisabled)
        {
            //Default true is right
            if (lhDefault)
            {
                currSq += 1;
                if (currSq > numGrid.GetLength(1) - 1) currSq = 0;
                else if (currSq < 0) currSq = numGrid.GetLength(1) - 1;
            }
            else
            {
                currSq -= 1;
                if (currSq > numGrid.GetLength(1) - 1) currSq = 0;
                else if (currSq < 0) currSq = numGrid.GetLength(1) - 1;
            }
            getNumberToDisplay(currLnSq, currSq, currLn, currPos);
        }
    }

    public async void btnSubmit() 
    {
        int checkNumber = getNumberInt(currLnSq, currSq, currLn, currPos);
        if (!actionDisabled)
        {
            if (checkNumber == 0)
            {
                displayNumber.text = "<color=#00FF00>YES</color>";
                audioSource.PlayOneShot(jackpotAudioClip, 1);
                actionDisabled = true;
                obsfusticationDisabled = true;
                await Task.Delay(5000);
                getNumberToDisplay(currLnSq, currSq, currLn, currPos);
                actionDisabled = false;
                obsfusticationDisabled = false;
            }
            else if (checkNumber < 10 && checkNumber != 0)
            {
                displayNumber.text = "<color=#FF0000>NO</color>";
                audioSource.PlayOneShot(sirenAudioClip, 1);
                actionDisabled = true;
                obsfusticationDisabled = true;
                await Task.Delay(5000);
                getNumberToDisplay(currLnSq, currSq, currLn, currPos);
                actionDisabled = false;
                obsfusticationDisabled = false;
            }
            else
            {
                displayNumber.text = "<color=#FFFFFF>???</color>";
                audioSource.PlayOneShot(coinFailAudioClip, 1);
                actionDisabled = true;
                await Task.Delay(1000);
                getNumberToDisplay(currLnSq, currSq, currLn, currPos);
                actionDisabled = false;
            }
        }
    }
    public void btnRotate() 
    {
        if (!actionDisabled)
        {
            svDefault = !svDefault;
            lvDefault = !lvDefault;
            shDefault = !shDefault;
            lhDefault = !lhDefault;
        }
    }
}
