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
    public float slamCooldown;

    public GameObject thisCamera;
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
    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask Crates;

    float targetRadius;
    bool grounded;
    bool doSquash;
    bool doubleJump;
    string spriteState;
    float slamTime;
    string playerState = "grounded";
    private float ymov;
    private float crateMargin;

    // Start is called before the first frame update
    void Start()
    {
        crateMargin = groundMargin * 2;
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
        ymov = Input.GetAxis("Vertical") * Time.deltaTime;

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

        isGrounded();
        
        doParticles();

        spriteUpdate();

        movePlayer(xmov, ymov);

        //If moving above speed limit,
        if (player.velocity.x > speedLimit) player.velocity = new Vector2(speedLimit, player.velocity.y);
        if (player.velocity.x < -speedLimit) player.velocity = new Vector2(-speedLimit, player.velocity.y);
       
    }

    private bool groundDetect(LayerMask mask, float margin)
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - .4f, transform.position.y - .5f), new Vector2(transform.position.x + .4f, transform.position.y - margin), mask);
    }

    private void isGrounded()
    {
        bool grounded = groundDetect(Ground, groundMargin);
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

        //If grounded with no input, stop in place. In air, keep moving.
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
        if (playerState != "pound" && !grounded && Input.GetButtonDown("Slam"))
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

        if (grounded && Input.GetButtonDown("Jump"))
        {
            jump(ref ymov, jumpheight);
            slamTime = slamCooldown + Time.time;
            return;
        }

        if (!grounded && doubleJump == true && Input.GetButtonDown("Jump"))
        {
            jump(ref ymov, jumpheight * dJumpMod);
            slamTime = slamCooldown + Time.time;
            return;
        }
    }

    private void land()
    {
        playerState = "grounded";
        if (doSquash) squash.SetTrigger("Squash");
        doubleJump = true;
        doSquash = false;
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

    //This is supposed to let other s
    public string getState()
    {
        return playerState;
    }

    //This handles interactions with interactable objects. Really need to rework this because god just look at this code it's an embarrassment.
    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (playerState != "victory")   
        switch (trig.name)
        {
            case "LeftFireball(Clone)":
            case "Fireball(Clone)":
            case "Fireball":
            case "Spike":
                if (playerState != "dead") Sounder.PlaySound("death");
                    {
                        playerState = "dead";
                        thisCamera.SendMessage("deathShake"); 
                        squash.SetTrigger("Reset");
                    }
                break;
            case "ORB":
                if (playerState != "victory") Sounder.PlaySound("orb");
                playerState = "victory";
                scener.nextScene();
                break;
        }

    }

    //This literally just plays the landing sound when you hit something
    private void OnCollisionEnter2D()
    {
       if (Scener.transitionActive == false) Sounder.PlaySound("land");
    }

    //If the light radius isn't the target radius, shift the radius in the right direction
    private void shiftLight()
    {
        if (pointLight.pointLightOuterRadius < targetRadius) pointLight.pointLightOuterRadius += 1f;
        else pointLight.pointLightOuterRadius -= 1;
    }

    //Performs death actions and returns whether the player is dead
    private bool isDead()
    {
        if (playerState == "dead")
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
    
    //Adds slight delay to pound immediately after a jump to fix slowfall glitch
    IEnumerator bufferSlam()
    {
        yield return new WaitForSeconds(slamCooldown);
        squash.SetTrigger("Pound");
        Sounder.PlaySound("drop");
        player.velocity = new Vector2(player.velocity.x, -30);
        playerState = "pound";
    }

    private void crateBroken()
    {
        if (groundDetect(Crates,crateMargin))
        {
            player.velocity = new Vector2(player.velocity.x, 30);
        }
        else player.velocity = new Vector2(player.velocity.x, 20);
        playerState = "airborne";
        doubleJump = true;
    }

    private void wallbounce()
    {
        player.velocity = new Vector2(player.velocity.x * 1.5f, 30);
        playerState = "airborne";
        doubleJump = true;
    }
}
