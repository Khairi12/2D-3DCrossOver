﻿using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float animationSpeed = 0.1f;
    public float movementSpeed = 0.1f;
    public float jumpHeight = 7.5f;

    private Rigidbody rigBody;
    private SpriteRenderer sprite;
    private float lastYposition = 0;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isIdling = true;

    private void Start() {
        // set gravity to be stronger
        Physics.gravity = new Vector3(0f, -15f, 0f);

        rigBody = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();
        StartCoroutine(IdleAnimation());
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Platform") {
            StopAllCoroutines();

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)) {
                StartCoroutine(RunAnimation());
            }
            else {
                StartCoroutine(IdleAnimation());
            }

            isJumping = false;
        }
    }

    // ---------------------------------------------------------------------------------
    // CHARACTER CONTROLS
    // ---------------------------------------------------------------------------------

    // Design flaw - player can walk off ledge, falling won't trigger until the player "jumps"
    // Rework required
    private IEnumerator Jump() {
        bool isFalling = false;
        
        // Start jump
        isJumping = true;
        yield return StartCoroutine(JumpAnimation());
        rigBody.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);

        // Wait until falling
        while (!isFalling) {
            yield return new WaitForSeconds(0.5f);

            if (!PlayerUtils.CharacterFalling(lastYposition, transform.position.y)) {
                isJumping = true;
                isFalling = true;
            }
        }

        StartCoroutine(FallAnimation());
    }

    private void MoveLeft() {
        isIdling = false;
        sprite.flipX = true;

        if (!isRunning && !isJumping) {
            isRunning = true;
            StopAllCoroutines();
            StartCoroutine(RunAnimation());
        }

        transform.Translate(Vector3.left.x * movementSpeed, 0f, 0f);
    }

    private void MoveRight() {
        isIdling = false;
        sprite.flipX = false;

        if (!isRunning && !isJumping) {
            isRunning = true;
            StopAllCoroutines();
            StartCoroutine(RunAnimation());
        }

        transform.Translate(Vector3.right.x * movementSpeed, 0f, 0f);
    }

    private void MoveUp() {
        isIdling = false;

        if (!isRunning && !isJumping) {
            isRunning = true;
            StopAllCoroutines();
            StartCoroutine(RunAnimation());
        }

        transform.Translate(0f, 0f, 1f * movementSpeed, Space.World);
    }

    private void MoveDown() {
        isIdling = false;

        if (!isRunning && !isJumping) {
            isRunning = true;
            StopAllCoroutines();
            StartCoroutine(RunAnimation());
        }

        transform.Translate(0f, 0f, -1f * movementSpeed, Space.World);
    }

    private void StopMove() {
        isRunning = false;

        if (!isIdling && !isJumping) {
            isIdling = true;
            StopAllCoroutines();
            StartCoroutine(IdleAnimation());
        }
    }

    // ---------------------------------------------------------------------------------
    // UPDATE PROCESS
    // ---------------------------------------------------------------------------------

    private void Update() {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping) {
            StopAllCoroutines();
            StartCoroutine(Jump());
        }

        // Directional Movement
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)) {
            StopMove();
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow)) {
            MoveLeft();
            MoveUp();
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow)) {
            MoveLeft();
            MoveDown();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow)) {
            MoveRight();
            MoveUp();
        }
        else if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow)) {
            MoveRight();
            MoveDown();
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            MoveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            MoveRight();
        }
        else if (Input.GetKey(KeyCode.UpArrow)) {
            MoveUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            MoveDown();
        }

        // Player stops moving
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
            StopMove();
        }

        // Save last yPosition
        lastYposition = transform.position.y;
    }

    // ---------------------------------------------------------------------------------
    // SPRITE ANIMATIONS
    // ---------------------------------------------------------------------------------

    private IEnumerator JumpAnimation() {
        float adjustedSpeed = animationSpeed / 3;

        for (int i = 0; i < 4; i++) {
            sprite.sprite = Resources.Load<Sprite>("Sprites/adventurer-jump-0" + i);
            yield return new WaitForSeconds(adjustedSpeed);
        }

        yield return null;
    }

    private IEnumerator FallAnimation() {
        while (true) {
            for (int i = 0; i < 2; i++) {
                sprite.sprite = Resources.Load<Sprite>("Sprites/adventurer-fall-0" + i);
                yield return new WaitForSeconds(animationSpeed);
            }
        }
    }

    private IEnumerator RunAnimation() {
        while (true) {
            for (int i = 0; i < 6; i++) {
                sprite.sprite = Resources.Load<Sprite>("Sprites/adventurer-run-0" + i);
                yield return new WaitForSeconds(animationSpeed);
            }
        }
    }

    private IEnumerator IdleAnimation() {
        while (true) {
            for (int i = 0; i < 3; i++) {
                sprite.sprite = Resources.Load<Sprite>("Sprites/adventurer-idle-0" + i);
                yield return new WaitForSeconds(animationSpeed);
            }
        }
    }
}
