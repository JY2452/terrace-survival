using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class TypeWriter: MonoBehaviour
{
	public float delay = 0.01f;
    public string[] fullText;
	private string currentText = "";

	void Start ()
    {
        StartCoroutine(ShowText());
    }

	IEnumerator ShowText()
    {
		for(int i = 0; i < fullText.Length; i++)
        {
            string displayText = fullText[i];
            currentText = "";
            for(int j = 0; j < displayText.Length; j++)
            {
                currentText = displayText.Substring(0, j);
                this.GetComponent<TextMeshProUGUI>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(delay * 10);

        }
        SceneManager.LoadScene("Game");
    }

}