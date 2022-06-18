using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mPlayerMovement : MonoBehaviour
{

    public Rigidbody2D theRigidbody;
    public Camera theCamera;

    Vector2 movement;

    public Animator animator;

    void Update()
    {
        if (mPauseMenu.GameIsPaused == false)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Magnitude", movement.magnitude);
        }
    }

    void FixedUpdate()
    {
        if (mPauseMenu.GameIsPaused == false)
        {
            if (theRigidbody.bodyType == RigidbodyType2D.Dynamic)
            {
                theRigidbody.MovePosition(theRigidbody.position + movement * GetComponent<mPlayer>().getStats().Speed * Time.fixedUnscaledDeltaTime);
                if ((movement != Vector2.zero) && (!IsInvoking("footstepSound"))) Invoke("footstepSound", 0.12f);
            }
        }
    }

    private void footstepSound()
    {
        mAudioManager.Instance.PlaySFX("player_footstep_0" + Random.Range(0, 11).ToString(), 0.9f);
    }

}
