using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExitWall : MonoBehaviour
{
    bool playerOneValid, playerTwoValid = false;
    int playerNumber = 0;

    void OnTriggerEnter(Collider playerObject)
    {
        if ((this.GetComponent<BoxCollider>().isTrigger) && (playerObject.tag.Contains("Player")))
        {
            playerNumber++;
        }
    }

    void OnTriggerExit(Collider playerObject)
    {
        if ((this.GetComponent<BoxCollider>().isTrigger) && (playerObject.tag.Contains("Player")))
        {
            string playerTag = playerObject.tag;
            playerNumber--;

            if (ValidMovementTowardsPuzzle(ref playerObject))
            {
                PlayerWallStateChange(true, ref playerTag);

                if (BothPlayersInRightArea())
                {
                    //ToggleExitControl();
                    Destroy(this);
                }
            }
            else
            {
                PlayerWallStateChange(false, ref playerTag);
            }
        }
    }

    private void PlayerWallStateChange(bool state, ref string playerTag)
    {
        if (playerTag.Contains("Player1"))
        {
            playerOneValid = state;
        }
        else if (playerTag.Contains("Player2"))
        {
            playerTwoValid = state;
        }
    }

    private void ToggleExitControl()
    {
        bool wallState = this.GetComponent<MeshRenderer>().enabled;
        this.GetComponent<MeshRenderer>().enabled = !wallState;
        this.GetComponent<BoxCollider>().isTrigger = wallState;
        Destroy(this);
    }

    private bool BothPlayersInRightArea()
    {
        return (playerOneValid && playerTwoValid);
    }

    private bool ValidMovementTowardsPuzzle(ref Collider playerObject)
    {
        Vector3 wallPosition = this.transform.position;
        Vector3 collisionPoint = playerObject.ClosestPointOnBounds(wallPosition);

        float collidePoint = Vector3.Dot(collisionPoint, this.transform.forward);
        float wallPoint = Vector3.Dot(wallPosition, this.transform.forward);

        return (collidePoint > wallPoint);
    }
}
