using UnityEngine;

public class SlashFadeAway : MonoBehaviour
{
    [SerializeField] SpriteRenderer slash;
    [SerializeField] SpriteRenderer glow;
    [SerializeField] Material glowMaterial;
    [SerializeField] float fadeTime = 5.0f;

    void Start()
    {
        glow.material = glowMaterial;
    }
    void Update()
    {
        Color color = slash.color;
        color.a = Mathf.Clamp01(color.a - Time.deltaTime / fadeTime);
        slash.color = color;


        Color glowColor = glow.material.color;
        glowColor.a = Mathf.Clamp01(color.a - Time.deltaTime / fadeTime);
        glow.material.color = glowColor; 
    } 
}
