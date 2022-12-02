using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    new public string name = "New Item";
    public Sprite icon = null;
    public Sprite icon2 = null;
    public bool isFilled = false;
    public AudioClip audio = null;
    public AudioClip audio2 = null;

    public virtual void Use()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (name.Equals("Bucket"))
        {
            GameObject water = GameObject.FindGameObjectWithTag("Water");
            
            if (Vector3.Distance(player.transform.position, water.transform.position) < 5f)
            {
                isFilled = true;
                AudioSource.PlayClipAtPoint(audio, player.transform.position);
                GameController.instance.SetText("Filled Bucket with Water");

            } else
            {
                if (!isFilled) GameController.instance.SetText("Tried using bucket but there was nothing to fill");
                else GameController.instance.SetText("Used water but nothing happened");

                GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire2");
                foreach (GameObject fire in fires)
                {
                    if(Vector3.Distance(player.transform.position, fire.transform.position) < 3f)
                    {
                        ParticleSystem[] children = fire.GetComponentsInChildren<ParticleSystem>();
                        foreach (ParticleSystem r in children)
                        {
                            r.Stop();
                        }
                        if (isFilled) AudioSource.PlayClipAtPoint(audio2, player.transform.position);
                        isFilled = false;
                        GameController.instance.fireCount--;
                        GameController.instance.SetText("Extinguished Fire with Water");
                    }
                }

            }
            GameController.instance.onItemChangedCallback.Invoke();
        }

        if(name.Equals("Ax") && GameObject.FindGameObjectWithTag("Wood") != null)
        {
            GameController.instance.SetText("Used Ax, but nothing happened");
            AudioSource.PlayClipAtPoint(audio, player.transform.position);
            GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
            for (int i = 0; i < trees.Length; i++)
            {
                if(Vector3.Distance(player.transform.position, trees[i].transform.position) < 4.3f)
                {
                    GameController.instance.SetText("Used Ax to Obtain Wood");
                    GameObject.FindGameObjectWithTag("Wood").GetComponent<Renderer>().enabled = false;
                    Renderer[] children = GameObject.FindGameObjectWithTag("Wood").GetComponentsInChildren<Renderer>();
                    foreach (Renderer r in children)
                    {
                        r.enabled = true;
                    }
                }
            }
        }

        if (name.Equals("Wood"))
        {
            AudioSource.PlayClipAtPoint(audio, player.transform.position);
            GameObject[] g = GameObject.FindGameObjectsWithTag("Wood_Fire");
            foreach (GameObject r in g)
            {
                r.GetComponent<Renderer>().enabled = true;
            }
            GameController.instance.Remove(this);
            GameController.instance.SetText("Placed Wood");

        }

        if (name.Equals("Leaves"))
        {
            if (GameObject.FindGameObjectWithTag("Wood_Fire").GetComponent<Renderer>().enabled)
            {
                AudioSource.PlayClipAtPoint(audio, player.transform.position);
                GameObject[] l = GameObject.FindGameObjectsWithTag("Leaves_Fire");
                foreach (GameObject r in l)
                {
                    Renderer[] l2 = r.GetComponentsInChildren<Renderer>();
                    foreach (Renderer r2 in l2)
                    {
                        r2.enabled = true;
                    }

                }
                GameController.instance.Remove(this);
                GameController.instance.SetText("Placed Leaves");
            } else
            {
                GameController.instance.SetText("Tried Using Leaves but nothing happened");
            }
        }

        if (name.Equals("Rocks"))
        {
            if (GameObject.FindGameObjectWithTag("Leaves_Fire").GetComponent<Renderer>().enabled)
            {
                GameController.instance.SetText("Fire started");
                AudioSource.PlayClipAtPoint(audio, player.transform.position);
                AudioSource.PlayClipAtPoint(audio2, player.transform.position);
                ParticleSystem[] children = GameObject.FindGameObjectWithTag("Fire").GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem r in children)
                {
                    r.Play();
                }
                GameController.instance.Remove(this);

                GameController.instance.StartFire();

                GameObject.FindGameObjectWithTag("Bird").GetComponent<Renderer>().enabled = true;
            } else
            {
                GameController.instance.SetText("Tried Using rocks but nothing happened");
            }
        }

        if (name.Equals("Bird"))
        {
            GameController.instance.Remove(this);
            GameController.instance.SetText("Cooking bird meat...");
            AudioSource.PlayClipAtPoint(audio, player.transform.position);
            GameController.instance.CookMeat();
        }

        if (name.Equals("Meat"))
        {
            GameController.instance.Remove(this);
            GameController.instance.SetText("Yummy!");
            AudioSource.PlayClipAtPoint(audio, player.transform.position);
            GameController.instance.GameWin();
        }
    }
}
