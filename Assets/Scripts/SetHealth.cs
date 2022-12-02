using UnityEngine;
using UnityEngine.UI;

public class SetHealth : MonoBehaviour
{
    public Image bar;
    float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        bar = GetComponent<Image>();
        currentHealth = GameController.instance.health;
    }

    void Update()
    {
        currentHealth = GameController.instance.health / 100f;
        bar.fillAmount = currentHealth;

        if (currentHealth > .5f) bar.color = new Color32(30, 130, 57, 255);
        else if(currentHealth > .25f) bar.color = new Color32(255, 221, 54, 255);
        else bar.color = new Color32(219, 37, 37, 255);
    }
}
