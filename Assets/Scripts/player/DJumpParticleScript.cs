using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJumpParticleScript : MonoBehaviour
{

    private static ParticleSystem dJumpParticle;
    

    // Start is called before the first frame update
    void Start()
    {
        dJumpParticle = GetComponent<ParticleSystem>();
    }

    public static void hideParticle()
    {
        ParticleSystem.EmissionModule module = dJumpParticle.emission;
        module.rateOverTime = 0;
    }

    public static void showParticle()
    {
        ParticleSystem.EmissionModule module = dJumpParticle.emission;
        ParticleSystem.MainModule propertyMod = dJumpParticle.main;
        UnityEngine.Color newcolor = new Color(255, 255, 255, 1);
        propertyMod.startColor = new ParticleSystem.MinMaxGradient(newcolor);
        propertyMod.startSize = .4f;
        module.rateOverTime = 20;
    }

    public static void burstParticle(float a, float b, float c, float d)
    {
        ParticleSystem.EmissionModule emissionMod = dJumpParticle.emission;
        ParticleSystem.MainModule propertyMod = dJumpParticle.main;
        UnityEngine.Color newcolor = new Color(a, b, c, 1);
        propertyMod.startColor = new ParticleSystem.MinMaxGradient(newcolor);
        propertyMod.startSize = .1f * d;
        emissionMod.rateOverTime = 700;
    }

    public static void burstParticle(float a, float b, float c, float d, int f)
    {
        ParticleSystem.EmissionModule emissionMod = dJumpParticle.emission;
        ParticleSystem.MainModule propertyMod = dJumpParticle.main;
        UnityEngine.Color newcolor = new Color(a, b, c, 1);
        propertyMod.startColor = new ParticleSystem.MinMaxGradient(newcolor);
        propertyMod.startSize = .1f * d;
        emissionMod.rateOverTime = f*10;
    }
}
