using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetPackController : MonoBehaviour
{
    public bool jetPackEnabled;
    public float jetpackDuration;
    public float jetpackRecharge;
    private float defaultJetDur;
    public float jetForce;
    public Vector3 moveDirection = Vector3.zero;
    public Rigidbody2D PlayerRG;
    bool Recharging;

    public AudioSource jetPackSound;

    public enum JetpackType
    {
        Activated,
        Not_Activated
    }


    private bool grounded;

    public JetpackType jetpackType;

    void Start()
    {
        defaultJetDur = jetpackDuration;
    }

    // Update is called once per frame
    void Update()
    {
        jetPackSound.volume = 0;

        jetpackDuration = Mathf.Clamp(jetpackDuration, 0, defaultJetDur);

        if (jetPackEnabled && jetpackDuration > 0)
        {
            if (jetpackType == JetpackType.Activated)
            {
                if (Input.GetKey(KeyCode.E) && grounded == false)
                {
                    if (jetPackEnabled)
                    {
                        ApplyJetForce();
                        jetPackSound.volume = 1;
                    }
                }
            }
        }

        if (!jetPackEnabled && (jetpackDuration < defaultJetDur) && !Recharging)
        {
            StartCoroutine(RechargeJetpack());
            Recharging = true;
        }
        else if (!jetPackEnabled && (jetpackDuration < defaultJetDur) && Recharging)
        {
            StopCoroutine(RechargeJetpack());
            Recharging = false;
        }

        if (jetpackType == JetpackType.Activated)
        {
            StartCoroutine(ActivateJetpack());
        }

        if (jetpackType == JetpackType.Activated)
        {
            StartCoroutine(ActivateJetpack());
        }
    }

    private IEnumerator ActivateJetpack()
    {
        yield return new WaitForSeconds(.3f);

        if (jetpackDuration > 0)
            jetPackEnabled = true;
    }



    private void ApplyJetForce()
    {
        PlayerRG.AddForce(PlayerRG.transform.up * jetForce);
        jetpackDuration -= Time.deltaTime;
    }

    private IEnumerator RechargeJetpack()
    {
        if (jetpackDuration <= 0f)
        {
            yield return new WaitForSeconds(3f);
            jetpackDuration = defaultJetDur;
            yield break;
        }

        yield return new WaitForSeconds(.05f);
        StartCoroutine(RechargeJetpack());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            grounded = true;
            jetPackEnabled = false;
        }
    }


    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            grounded = false;
        }
    }
}