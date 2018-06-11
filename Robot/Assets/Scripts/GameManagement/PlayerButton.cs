using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerButton
{
    public KeyCode[] defaultButton = new KeyCode[11];

    public KeyCode[] currentButton = new KeyCode[11];

    public PlayerButton()
    {
        defaultButton[0] = KeyCode.W;
        defaultButton[1] = KeyCode.A;
        defaultButton[2] = KeyCode.S;
        defaultButton[3] = KeyCode.D;
        defaultButton[4] = KeyCode.Alpha1;
        defaultButton[5] = KeyCode.Alpha2;
        defaultButton[6] = KeyCode.Alpha3;
        defaultButton[7] = KeyCode.Alpha4;
        defaultButton[8] = KeyCode.Space;
        defaultButton[9] = KeyCode.E;
        defaultButton[10] = KeyCode.F;

        for (int i = 0; i <= 10; i++)
        {
            currentButton[i] = defaultButton[i];
        }
    }     
    
      
}

/*
 * 0 - forward
 * 1 - left
 * 2 - backward
 * 3 - right
 * 4 - emotion1
 * 5 - emotion2
 * 6 - emotion3
 * 7 - emotion4
 * 8 - jump
 * 9 - interact/reattach
 * 10 - lims panel
 * 
 * 
 * 
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
