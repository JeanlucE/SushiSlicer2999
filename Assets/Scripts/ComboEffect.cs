using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class ComboEffect : MonoBehaviour 
{
    public Color[] comboColors;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetime;
    private ParticleSystem partSys;
    private ParticleSystemRenderer partRenderer;
    private ComboList combo;

    void Start()
    {
        partSys = GetComponent<ParticleSystem>();
        colorOverLifetime = partSys.colorOverLifetime;
        partRenderer = GetComponent<ParticleSystemRenderer>();

        combo = PlayerController.main.gameObject.GetComponent<ComboList>();
    }

    void Update()
    {
        Color col;
        int multiplier = (int)combo.GetMultiplier();

        partRenderer.enabled = multiplier > 1;//only render when active combo

        //render highest possible color
        if (comboColors.Length > multiplier)
        {
            col = comboColors[multiplier];
        }
        else
        {
            col = comboColors[comboColors.Length - 1];
        }

        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(Color.white, col);
    }
}
