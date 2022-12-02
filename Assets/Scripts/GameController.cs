using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using TMPro;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameObject player;
    public float health = 100f;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public float healthRate = .008f;

    public int space = 8;
    public List<Item> inventory;

    public GameOverScreen GameOverScreen;
    public AudioClip pickUpAudio = null;
    public AudioClip gameOverAudio = null;
    public AudioClip gameWinAudio = null;

    private bool fireStarted = false;
    private TextMeshProUGUI text = null;
    public int fireCount = 6;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        text = GameObject.FindGameObjectWithTag("Text").GetComponent<TextMeshProUGUI>();
        inventory = new List<Item>();
        Reset();
    }

    void Update()
    {
        if (health <= 0) GameOver();
        if(fireStarted && fireCount <= 0)
        {
            GameController.instance.GetComponent<AudioSource>().Pause();
            fireStarted = false;
        }

        if (!fireStarted)
        {
            health -= healthRate;
        }
        else
        {
            health -= 2 * healthRate;
        }
    }

    private void Reset()
    {
        GameObject.FindGameObjectWithTag("Wood").GetComponent<Renderer>().enabled = false;
        Renderer[] children = GameObject.FindGameObjectWithTag("Wood").GetComponentsInChildren<Renderer>();
        foreach (Renderer r in children)
        {
            r.enabled = false;
        }

        GameObject[] g = GameObject.FindGameObjectsWithTag("Wood_Fire");
        foreach (GameObject r in g)
        {
            r.GetComponent<Renderer>().enabled = false;
        }

        GameObject[] l = GameObject.FindGameObjectsWithTag("Leaves_Fire");
        foreach (GameObject r in l)
        {
            Renderer[] l2 = r.GetComponentsInChildren<Renderer>();
            foreach (Renderer r2 in l2)
            {
                r2.enabled = false;
            }

        }

        ParticleSystem[] fireChildren = GameObject.FindGameObjectWithTag("Fire").GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem r in fireChildren)
        {
            r.Stop();
        }

        GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire2");
        foreach (GameObject fire in fires)
        {
            ParticleSystem[] pc = fire.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem r in pc)
            {
                r.Stop();
            }
        }

        GameObject.FindGameObjectWithTag("Bird").GetComponent<Renderer>().enabled = false;
        GameObject.FindGameObjectWithTag("Bird_Fire").GetComponent<Renderer>().enabled = false;
        Renderer[] meats = GameObject.FindGameObjectWithTag("Meat").GetComponentsInChildren<Renderer>();
        foreach (Renderer m in meats)
        {
            m.enabled = false;
        }
        GameObject.FindGameObjectWithTag("Meat").GetComponent<Renderer>().enabled = false;
    }

    public bool Add(Item item)
    {
        if (inventory.Count < space)
        {
            inventory.Add(item);
            GameController.instance.SetText("Obtained " + item.name);
            AudioSource.PlayClipAtPoint(pickUpAudio, player.transform.position);
            if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
            return true;
        }
        return false;
    }

    public void Remove(Item item)
    {
        inventory.Remove(item);
        if (onItemChangedCallback != null) onItemChangedCallback.Invoke();
    }

    public void StartFire()
    {
        StartCoroutine(FireRun());
    }

    public void CookMeat()
    {
        StartCoroutine(CookRun());
    }

    public void SetText(string s)
    {
        text.text = s;
    }

    IEnumerator FireRun()
    {
        yield return new WaitForSeconds(5);

        GameController.instance.SetText("Ahhh the fire is too strong and might burn the terrace down!");
        GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire2");
        foreach (GameObject fire in fires)
        {
            ParticleSystem[] pc = fire.GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem r in pc)
            {
                r.Play();
            }
        }

        GameController.instance.GetComponent<AudioSource>().Play();
        fireStarted = true;
    }

    IEnumerator CookRun()
    {
        GameObject.FindGameObjectWithTag("Bird_Fire").GetComponent<Renderer>().enabled = true;
        yield return new WaitForSeconds(5);

        GameObject.FindGameObjectWithTag("Bird_Fire").GetComponent<Renderer>().enabled = false;

        Renderer[] meats = GameObject.FindGameObjectWithTag("Meat").GetComponentsInChildren<Renderer>();
        foreach (Renderer m in meats)
        {
            m.enabled = true;
        }
        GameObject.FindGameObjectWithTag("Meat").GetComponent<Renderer>().enabled = true;
    }

    public void GameOver()
    {
        AudioSource.PlayClipAtPoint(gameOverAudio, player.transform.position);
        GameOverScreen.Setup("GAME OVER");
        GameController.instance.SetText("");
        player.gameObject.SetActive(false);

    }

    public void GameWin()
    {
        healthRate = 0;
        StartCoroutine(GameWinDisplay());

    }

    IEnumerator GameWinDisplay()
    {
        yield return new WaitForSeconds(5);
        AudioSource.PlayClipAtPoint(gameWinAudio, player.transform.position);
        GameOverScreen.Setup("CONGRATS. YOU SURVIVED.");
        GameController.instance.SetText("");
        player.gameObject.SetActive(false);
    }
}
