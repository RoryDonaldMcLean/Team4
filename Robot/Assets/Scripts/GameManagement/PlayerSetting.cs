using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PlayerSetting
{
    public Level lastLevel = new Level();

    public KeyCode[] defaultButton = new KeyCode[28];

    public KeyCode[] currentButton = new KeyCode[28];

    public int volume;

    public bool player1Controller;

    public string versionInfo;
     
    public PlayerSetting()
    {
        defaultButton[0] = KeyCode.W;
        defaultButton[1] = KeyCode.A;
        defaultButton[2] = KeyCode.S;
        defaultButton[3] = KeyCode.D;
        defaultButton[4] = KeyCode.Alpha1;
        defaultButton[5] = KeyCode.Alpha2;
        defaultButton[6] = KeyCode.Alpha3;
        defaultButton[7] = KeyCode.Alpha4;
        defaultButton[8] = KeyCode.Space; // jump
        defaultButton[9] = KeyCode.E; // interact
        defaultButton[10] = KeyCode.F; // limbs panel
        defaultButton[11] = KeyCode.Z; // drop code
        defaultButton[12] = KeyCode.G; // player1 emote menu

        defaultButton[13] = KeyCode.UpArrow;
        defaultButton[14] = KeyCode.LeftArrow;
        defaultButton[15] = KeyCode.DownArrow;
        defaultButton[16] = KeyCode.RightArrow;
		defaultButton[17] = KeyCode.Alpha5;
		defaultButton[18] = KeyCode.Alpha6;
		defaultButton[19] = KeyCode.Alpha7;
		defaultButton[20] = KeyCode.Alpha8;
        defaultButton[21] = KeyCode.Keypad0;//jump
        defaultButton[22] = KeyCode.Keypad7; //interact
        defaultButton[23] = KeyCode.Keypad8; // limbs panel
        defaultButton[24] = KeyCode.Keypad9; //drop code
		defaultButton[25] = KeyCode.Keypad5; //player2 emote menu

		defaultButton [26] = KeyCode.R;
		defaultButton [27] = KeyCode.Keypad2;


        for (int i = 0; i < defaultButton.Length; i++)
        {
            currentButton[i] = defaultButton[i];
        }
        
        volume = 50;

        player1Controller = false;

        lastLevel = Level.None;

        versionInfo = "1.0";
    }

    public void sameButton(int number)
    {
        for (int i = 0; i < defaultButton.Length; i++)
        {
            if (number != i && currentButton[i] == currentButton[number])
            {
                currentButton[i] = KeyCode.None;
            }
        }
    }

}

/*
 * Player1:
 * 
 * 0 - up
 * 1 - left
 * 2 - down
 * 3 - right
 * 4 - emotion1
 * 5 - emotion2
 * 6 - emotion3
 * 7 - emotion4
 * 8 - jump
 * 9 - interact/reattach
 * 10 - lims panel
 * 11 - Another Button
 * 12 - Emote menu
 * 
 * Player2:
 * plus 13
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */
