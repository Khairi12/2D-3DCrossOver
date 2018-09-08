using UnityEngine;

public class PlayerUtils : MonoBehaviour {

    public static bool CharacterFalling(float lastY, float curY) {
        return lastY > curY ? true : false;
    }

    public static bool CharacterGrounded() {
        // Check if player is touching ground
        return false;
    }
}
