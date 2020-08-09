using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{

    //public
    [SerializeField] public float speedLimit = 12f;
    public float dJumpMod;
    public float speed = 12f;
    public float jumpheight = 20f;
    public float gravity;
    public float groundMargin;
    public string playerState = "grounded";
    public float slamCooldown;

    public GameObject playerSprite;
    public TrailRenderer trail;
    public Light2D pointLight;
    public Scener scener;
    public Animator squash;

    SpriteRenderer playerSR;
    
    public Sprite poundSprite;
    public Sprite jumpSprite;
    public Sprite deadSprite;

    //private
    Rigidbody2D player;
    CapsuleCollider2D capsule;
    [SerializeField] private LayerMask maskG;

    float targetRadius;
    bool grounded;
    bool doSquash;
    bool doubleJump;
    bool alive = true;
    string spriteState;
    float slamTime;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D> ();
        capsule = GetComponent<CapsuleCollider2D>();  
        playerSR = playerSprite.GetComponent<SpriteRenderer>();
        player.gravityScale = gravity;
        Physics2D.IgnoreLayerCollision(9, 10);
    }

    // Update is called once per frame
    void Update()
    {

        float xmov = Input.GetAxis("Horizontal") * speed * 100 * Time.deltaTime;
        float ymov = Input.GetAxis("Vertical") * Time.deltaTime;

        if (pointLight.pointLightOuterRadius != targetRadius)
        {
            shiftLight();
        }

        if (isDead()) return;

        if (playerState == "pound") trail.enabled = false;
        else 
        {
            trail.enabled = true;
            if ((squash.GetCurrentAnimatorStateInfo(0)).IsName("Player_Pound")) squash.SetTrigger("Reset");
        }

        if (Input.anyKeyDown)
        {
            abilities(ref ymov);
        }

        //Recharge doublejump/groundpound when grounded
        isGrounded();
        

        //Handle particles
        doParticles();

        spriteUpdate();

        movePlayer(xmov, ymov);

        //If moving above speed limit,
        if (player.velocity.x > speedLimit) player.velocity = new Vector2(speedLimit, player.velocity.y);
        if (player.velocity.x < -speedLimit) player.velocity = new Vector2(-speedLimit, player.velocity.y);
       
    }

    private void isGrounded()
    {
        bool grounded = Physics2D.OverlapArea(new Vector2(transform.position.x - .4f, transform.position.y - .5f), new Vector2(transform.position.x + .4f, transform.position.y - groundMargin), maskG);
        if (grounded && playerState != "victory")
        {
            land();
        }
        else
        {
            if (playerState == "grounded")
            {
                playerState = "airborne";
                doSquash = true;
            }
            else if (playerState == "pound" && player.velocity.y > 0) land();
        }
        return;
    }

    private void movePlayer(float xmov, float ymov)
    {
        //Prepare to check movement
        Vector2 movement;

        //If grounded, stop in place. In air, keep moving.
        if (Input.GetAxis("Horizontal") < .5 && Input.GetAxis("Horizontal") > -.5 && grounded)
        {
            movement = new Vector2(-player.velocity.x * 2, ymov);
        }
        else movement = new Vector2(xmov, ymov);

        //Apply movement
        player.AddForce(movement);
    }

    private void doParticles()
    {
        //Particle scripts
        if (playerState == "pound") DJumpParticleScript.burstParticle(.6f, .5f, .5f, 6);
        else if (!doubleJump || grounded) DJumpParticleScript.hideParticle();
        else DJumpParticleScript.showParticle();
    }

    private void jump(ref float ymov, float jumpheight)
    {
        if (playerState != "pound")
        {
            squash.SetTrigger("Stretch");
            doSquash = true;
            DJumpParticleScript.burstParticle(1, 1, 1, 8);
            player.constraints = RigidbodyConstraints2D.FreezeRotation;
            Sounder.PlaySound("jump");
            ymov = (jumpheight * 100 + Mathf.Abs(player.velocity.y));
            player.velocity = new Vector2(player.velocity.x, 0);
            if (!grounded) doubleJump = false;
        }
    }

    private void abilities(ref float ymov)
    {
        if (playerState != "pound" && !grounded && Input.GetKeyDown("s"))
        {
            if (slamTime <= Time.time)
            {
                squash.SetTrigger("Pound");
                Sounder.PlaySound("drop");
                player.velocity = new Vector2(player.velocity.x, -30);
                playerState = "pound";
                return;
            }
            else
            {
                StartCoroutine("bufferSlam");
            }
        }

        if (grounded && Input.GetKeyDown("w"))
        {
            jump(ref ymov, jumpheight);
            slamTime = slamCooldown + Time.time;
            return;
        }

        if (!grounded && doubleJump == true && Input.GetKeyDown("w"))
        {
            jump(ref ymov, jumpheight * dJumpMod);
            slamTime = slamCooldown + Time.time;
            return;
        }
    }

    private void land()
    {
        if (playerState != "dead")
        {
            playerState = "grounded";
            if (doSquash) squash.SetTrigger("Squash");
            doubleJump = true;
            doSquash = false;
        }
    }

    private string getSpriteState()
    {
        if (playerState == "pound")
        {
            return "pound";
        }

        if (playerState == "airborne")
        {
            return "jump";
        }

        return "jump";
    }

    private void spriteUpdate()
    {
        switch(getSpriteState())
        {
            case "pound":
                playerSR.sprite = poundSprite;
                break;
            case "jump":
                playerSR.sprite = jumpSprite;
                break;
        }
    }

    public string getState()
    {
        return playerState;
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        Debug.Log("Hit!");
        if (playerState != "victory")
        switch (trig.name)
        {
            case "Fireball(Clone)":
            case "Fireball":
            case "Spike":
                if (playerState != "dead") Sounder.PlaySound("death");
                playerState = "dead";
                squash.SetTrigger("Reset");
                alive = false;
                break;
            case "ORB":
                if (playerState != "victory") Sounder.PlaySound("orb");
                playerState = "victory";
                scener.nextScene();
                break;
        }

    }

    private void OnCollisionEnter2D()
    {
       if (Scener.transitionActive == false) Sounder.PlaySound("land");
    }

    private void shiftLight()
    {
        if (pointLight.pointLightOuterRadius < targetRadius) pointLight.pointLightOuterRadius += 1f;
        else pointLight.pointLightOuterRadius -= 1;
    }

    private bool isDead()
    {
        if (!alive)
        {
            DJumpParticleScript.burstParticle(.25f, .1f, .3f, 2, 70);
            playerSR.sprite = deadSprite;
            targetRadius = 15f;
            return true;
        }
        else
        {
            targetRadius = 35f;
            pointLight.color = new Color(0.996164f, 0.8254717f, 1f);
            return false;
        }
    }
    IEnumerator bufferSlam()
    {
        yield return new WaitForSeconds(slamCooldown);
        squash.SetTrigger("Pound");
        Sounder.PlaySound("drop");
        player.velocity = new Vector2(player.velocity.x, -30);
        playerState = "pound";
    }
}
