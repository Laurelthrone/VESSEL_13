using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{

    //publi
    public static string playerState = "grounded";

    public GameObject thisCamera, playerSprite, face;
    public TrailRenderer trail;
    public Light2D pointLight;
    public Scener scener;
    public Animator squash;
    public Sprite faceLeft, faceRight, faceWin, faceDead, faceSlam;

    SpriteRenderer playerSR, playerFace;

    //private
    Rigidbody2D player;
    CapsuleCollider2D capsule;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private LayerMask Crates;

    [SerializeField] float speedLimit;
    [SerializeField] float dJumpMod;
    [SerializeField] float speed;
    [SerializeField] float jumpheight;
    [SerializeField] float gravity;
    [SerializeField] float groundMargin;
    [SerializeField] float slamCooldown;
    [SerializeField] float wallbounceWindow;

    bool grounded;
    bool doSquash;
    bool doubleJump;
    bool initialized = false;
    bool canWallbounce = false;

    string spriteState;

    float targetRadius;
    float slamTime;
    float ymov;
    float crateMargin;
    float storeXvel;
    float gamespeed;
    
    
    private Vector2 targetPos;
    
    Animator spriteAnimator, chromaticAberration;
    Dictionary<string, Color> playerColors = new Dictionary<string, Color>();

    Color Slam, Normal, Dead;

    const int playerLayer = 10, alwaysIgnoreLayer = 9, deathwallLayer = 14;

    // Start is called before the first frame update
    void Start()    
    {
        gamespeed = Time.timeScale;
        Time.timeScale = 0;
        playerState = "grounded";
        player = GetComponent<Rigidbody2D>();
        player.bodyType = RigidbodyType2D.Static;
        ColorUtility.TryParseHtmlString("#CF616D", out Slam);
        ColorUtility.TryParseHtmlString("#FFCEF8", out Normal);
        ColorUtility.TryParseHtmlString("#87639A", out Dead);

        playerColors.Add("slam", Slam);
        playerColors.Add("airborne", Normal);
        playerColors.Add("victory", Normal);
        playerColors.Add("grounded", Normal);
        playerColors.Add("dead", Dead);

        crateMargin = groundMargin * 2;
        Physics2D.IgnoreLayerCollision(alwaysIgnoreLayer, playerLayer);
        Physics2D.IgnoreLayerCollision(deathwallLayer, playerLayer, true);

        
        player.gravityScale = gravity;
        capsule = GetComponent<CapsuleCollider2D>();
        playerSR = playerSprite.GetComponent<SpriteRenderer>();
        playerFace = face.GetComponent<SpriteRenderer>();
        spriteAnimator = playerSprite.GetComponent<Animator>();
        chromaticAberration = FindObjectOfType<Volume>().GetComponent<Animator>();
        chromaticAberration.SetFloat("Speed", -1f);
        initialized = true;
        StartCoroutine(LoadTime());
        player.bodyType = RigidbodyType2D.Dynamic;
    }

    IEnumerator LoadTime()
    {
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = gamespeed;
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(Globals.shakeEnabled);

        if (!initialized || playerState == "victory") return;

        if (pointLight.pointLightOuterRadius != targetRadius)
        {
            shiftLight();
        }

        if (isDead()) return;

        if (playerState == "slam")
        {
            spriteAnimator.SetFloat("Speed", 6 * Math.Sign(player.velocity.x));
            trail.enabled = false;
            if (canWallbounce) wallbounce();
        }
        else
        {
            spriteAnimator.SetFloat("Speed", player.velocity.x / 3);
            trail.enabled = true;
            if ((squash.GetCurrentAnimatorStateInfo(0)).IsName("Player_slam")) squash.SetTrigger("Reset");
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
    }

    private bool groundDetect(LayerMask mask, float margin)
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x - .4f, transform.position.y - .5f), new Vector2(transform.position.x + .4f, transform.position.y - margin), mask);
    }
            
    private void isGrounded()
    {
        if (groundDetect(Ground, groundMargin) && playerState != "victory")
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
            else if (playerState == "slam" && player.velocity.y > 0) land();
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
        if (playerState == "slam") DJumpParticleScript.burstParticle(.7f, .5f, .5f, 6);
        else if (!doubleJump || grounded) DJumpParticleScript.hideParticle();
        else DJumpParticleScript.showParticle();
    }

    private void jump(ref float ymov, float jumpheight)
    {
        if (playerState != "slam")
        {
            squash.SetTrigger("Stretch");
            doSquash = true;
            DJumpParticleScript.burstParticle(1, 1, 1, 8);
            player.constraints = RigidbodyConstraints2D.FreezeRotation;
            Sounder.PlaySound("jump");
            ymov = (jumpheight * 100 + Mathf.Abs(player.velocity.y));
            player.velocity = new Vector2(player.velocity.x, 0);
            if (!grounded) doubleJump = false;
            slamTime = slamCooldown + Time.time;
        }
    }

    private void abilities(ref float ymov)
    {
        if (playerState != "slam" && !grounded && Input.GetButtonDown("Slam"))
        {
            //Check if slam is available
            if (slamTime <= Time.time)
            {
                slam();
                return;
            }
            else
            {
                StartCoroutine("bufferSlam");
            }
        }

        if (Input.GetButtonDown("Jump"))
        {

            if (grounded)
            {
                jump(ref ymov, jumpheight);
                return;
            }

            if (!grounded && doubleJump == true)
            {
                jump(ref ymov, jumpheight * dJumpMod);
                return;
            }
        }
    }

    void slam()
    {
        squash.SetTrigger("Slam");
        thisCamera.SendMessage("slamShake");
        Sounder.PlaySound("drop");
        player.velocity = new Vector2(player.velocity.x, -30);
        playerState = "slam";
    }

    private void land()
    {
        if (playerState == "slam") thisCamera.SendMessage("land");
        playerState = "grounded";
        if (doSquash) squash.SetTrigger("Squash");
        doubleJump = true;
        doSquash = false;
    }

    private void spriteUpdate()
    {
        playerSR.color = playerColors[playerState];
        if (playerState == "dead") playerFace.sprite = faceDead;
        else if (playerState == "slam") playerFace.sprite = faceSlam;
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
                case "Beast":
                    goto case "Hazard";
                case "Hazard":
                    die();
                    break;
                case "Victory":
                    StartCoroutine(pullTo(trig.transform.position));
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
            chromaticAberration.SetFloat("Speed", 1f);
            Sounder.PlaySound("death");
            playerState = "dead";
            thisCamera.SendMessage("deathShake");
            squash.SetTrigger("Reset");
        }
    }

    IEnumerator pullTo(Vector2 point)
    {
        while(true)
        {
            float xvel = player.velocity.x;
            float yvel = player.velocity.y;
            xvel = 0;
            yvel = 0;
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), point, 7 * Time.deltaTime);
            playerFace.sprite = faceWin;
            yield return new WaitForEndOfFrame();
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
            chromaticAberration.SetFloat("Speed", -1f);
            Sounder.PlaySound("revive");
            playerState = "grounded";
            thisCamera.SendMessage("deathShake");
        }
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
    
    //Adds slight delay to slam immediately after a jump to fix slowfall glitch
    IEnumerator bufferSlam()
    {
        yield return new WaitForSeconds(slamCooldown);
        slam();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Store velocity on contact with wall to apply when wallbounce occurs
        storeXvel = player.velocity.x;
        if (Scener.transitionActive == false) Sounder.PlaySound("land");
        if (collision.collider.tag == "Wallbouncer") StartCoroutine(allowWallbounce());
    }

    IEnumerator allowWallbounce()
    {
        /*Main update function contains 
            "if(canWallbounce && playerState == "slam") wallbounce();
          This function enables wallbouncing for a short period after touching a wallbouncer
        */
        canWallbounce = true;
        yield return new WaitForSeconds(wallbounceWindow);
        canWallbounce = false;
    }

    private void wallbounce()
    {
        //Don't wallbounce if player didn't have enough velocity
        if (Math.Abs(storeXvel) < 5) return;
        
        //Disable the slam zoom
        thisCamera.SendMessage("land");
        
        //Apply the bounce velocity
        player.velocity = new Vector2(storeXvel * 1.5f, 30);

        //Set player state
        playerState = "airborne";
        doubleJump = true;
        canWallbounce = false;
    }
}
