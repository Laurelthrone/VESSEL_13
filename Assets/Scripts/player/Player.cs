using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Player : MonoBehaviour
{

    //publi
    public string playerState = "grounded";

    public GameObject thisCamera, playerSprite, orb, face;
    public TrailRenderer trail;
    public Light2D pointLight;
    public Scener scener;
    public Animator squash;
    public Sprite faceLeft, faceRight, faceWin, faceDead, facePound;

    SpriteRenderer playerSR, playerFace;

    //private
    Rigidbody2D player;
    CapsuleCollider2D capsule;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask Crates;

    [SerializeField] float speedLimit = 12f;
    [SerializeField] float dJumpMod;
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpheight = 20f;
    [SerializeField] float gravity;
    [SerializeField] float groundMargin;
    [SerializeField] float slamCooldown;

    bool grounded;
    bool doSquash;
    bool doubleJump;

    string spriteState;

    float targetRadius;
    float slamTime;
    float ymov;
    float crateMargin;
    
    private Vector2 targetPos;
    
    Animator spriteAnimator;
    Dictionary<string, Color> playerColors = new Dictionary<string, Color>();

    Color Pound, Normal, Dead;

    const int playerLayer = 10, alwaysIgnoreLayer = 9, deathwallLayer = 14;

    // Start is called before the first frame update
    void Start()    
    {
        crateMargin = groundMargin * 2;
        player = GetComponent<Rigidbody2D> ();
        capsule = GetComponent<CapsuleCollider2D>();  
        playerSR = playerSprite.GetComponent<SpriteRenderer>();
        playerFace = face.GetComponent<SpriteRenderer>();
        spriteAnimator = playerSprite.GetComponent<Animator>();
        player.gravityScale = gravity;
        Physics2D.IgnoreLayerCollision(alwaysIgnoreLayer, playerLayer);
        Physics2D.IgnoreLayerCollision(deathwallLayer, playerLayer, true);

        ColorUtility.TryParseHtmlString("#CF616D", out Pound);
        ColorUtility.TryParseHtmlString("#FFCEF8", out Normal);
        ColorUtility.TryParseHtmlString("#87639A", out Dead);

        playerColors.Add("pound", Pound);
        playerColors.Add("airborne", Normal);
        playerColors.Add("victory", Normal);
        playerColors.Add("grounded", Normal);
        playerColors.Add("dead", Dead);
    }

    // Update is called once per frame
    void Update()
    {

        if(playerState == "victory")
        {
            spriteUpdate();
            targetPos = new Vector2(orb.transform.position.x, orb.transform.position.y);
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), targetPos, 7 * Time.deltaTime);
            playerFace.sprite = faceWin;
            return;
        }

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

        isGrounded();
        
        doParticles();

        spriteUpdate();

        float xmov = Input.GetAxis("Horizontal") * speed * 100 * Time.deltaTime;
        ymov = Input.GetAxis("Vertical") * Time.deltaTime;
        if (Input.anyKeyDown)
        {
            abilities(ref ymov);
        }

        movePlayer(xmov, ymov);

        //Cap velocity at speed limit
        if (player.velocity.x > speedLimit) player.velocity = new Vector2(speedLimit, player.velocity.y);
        if (player.velocity.x < -speedLimit) player.velocity = new Vector2(-speedLimit, player.velocity.y);

        if (playerState != "pound")
        {
            spriteAnimator.SetFloat("Speed", player.velocity.x / 3);
        }
        else spriteAnimator.SetFloat("Speed", 6 * Math.Sign(player.velocity.x));
       
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
                thisCamera.SendMessage("slamShake");
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
        if (playerState == "pound") thisCamera.SendMessage("land");
        playerState = "grounded";
        if (doSquash) squash.SetTrigger("Squash");
        doubleJump = true;
        doSquash = false;
    }

    private void spriteUpdate()
    {
        playerSR.color = playerColors[playerState];
        if (playerState == "dead") playerFace.sprite = faceDead;
        else if (playerState == "pound") playerFace.sprite = facePound;
        else if (player.velocity.x < 0) playerFace.sprite = faceLeft;
        else playerFace.sprite = faceRight;
    }

    public string getState()
    {
        return playerState;
    }

    private void OnTriggerEnter2D(Collider2D trig)
    {
        if (playerState != "victory")   
            switch (trig.tag)
            {
                case "Hazard":
                    die();
                    break;
                case "Victory":
                    win();
                    break;
                case "Revive":
                    revive();
                    break;
            }
    }

    void die()
    {
        if (playerState != "dead")
        {
            Sounder.PlaySound("death");
            playerState = "dead";
            thisCamera.SendMessage("deathShake");
            squash.SetTrigger("Reset");
        }
    }

    void win()
    {
        Sounder.PlaySound("orb");
        playerState = "victory";
        scener.nextScene();
    }

    void revive()
    {
        if (playerState == "dead")
        {
            Sounder.PlaySound("revive");
            playerState = "grounded";
            thisCamera.SendMessage("deathShake");
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
            spriteAnimator.SetFloat("Speed", .99f * spriteAnimator.GetFloat("Speed"));
            Physics2D.IgnoreLayerCollision(deathwallLayer, playerLayer, true);
            DJumpParticleScript.burstParticle(.25f, .1f, .3f, 2, 70);
            spriteUpdate();
            targetRadius = 15f;
            return true;
        }
        else
        {
            Physics2D.IgnoreLayerCollision(deathwallLayer, playerLayer, false);
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

    //Bounce off side of crates
    private void crateBroken()
    {
        thisCamera.SendMessage("land");
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
        thisCamera.SendMessage("land");
        player.velocity = new Vector2(player.velocity.x * 1.5f, 30);
        playerState = "airborne";
        doubleJump = true;
    }
}
