using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HippoManager : MonoBehaviour
{
    public static HippoManager current;

    public GameObject hippoGO;
    public List<GameObject> listOfHippos;

    public TMP_Text scoreUI;
    public TMP_Text hippoCountUI;

    public Button maleSpawnButton;
    public Button femaleSpawnButton;

    private int maxHippoNumber = 15;
    private int minHippoNumber = 5;

    private int currentScore;

	private void Awake()
	{
        current = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        StartCoroutine(Score_Coroutine());
        StartCoroutine(CheckMating());
    }

	private void Update()
	{
        if (currentScore <= 10)
        {
            Color mButtonColor = maleSpawnButton.GetComponent<Image>().color;
            mButtonColor.a = 0.5f;
            maleSpawnButton.GetComponent<Image>().color = mButtonColor;

            Color fButtonColor = femaleSpawnButton.GetComponent<Image>().color;
            fButtonColor.a = 0.5f;
            femaleSpawnButton.GetComponent<Image>().color = fButtonColor;
        }
        else
        {
            maleSpawnButton.GetComponent<Image>().color = Color.white;
            femaleSpawnButton.GetComponent<Image>().color = Color.white;
        }
	}

	public void Breed()
    {
        GameObject instantiatedHippo = Instantiate(hippoGO);
        instantiatedHippo.GetComponent<Hippo_AI>().SetRandomGender();
        listOfHippos.Add(instantiatedHippo);
    }

    public void HippoDeath(GameObject hippo)
    {
        listOfHippos.Remove(hippo);
        Destroy(hippo);
    }

    private void AssignMate()
    {
        GameObject hippo_1 = listOfHippos[Random.Range(0, listOfHippos.Count)];
        GameObject hippo_2 = listOfHippos[Random.Range(0, listOfHippos.Count)];

        if (hippo_1 == hippo_2) return; // If same hippo, skip

        Hippo_AI hippo1_Script = hippo_1.GetComponent<Hippo_AI>();
        Hippo_AI hippo2_Script = hippo_2.GetComponent<Hippo_AI>();

        if (hippo1_Script.hasMate || hippo2_Script.hasMate) return; // If either hippo has mate, skip

        if ((hippo1_Script.hippoGender == Hippo_AI.HIPPO_GENDER.M &&
            hippo2_Script.hippoGender == Hippo_AI.HIPPO_GENDER.F) || 
            (hippo2_Script.hippoGender == Hippo_AI.HIPPO_GENDER.M &&
            hippo1_Script.hippoGender == Hippo_AI.HIPPO_GENDER.F))
        {
            hippo1_Script.SetHasMate(true);
            hippo2_Script.SetHasMate(true);
            hippo2_Script.SetPregnant();
        }
    }

    private IEnumerator CheckMating()
    {
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        AssignMate();
        StartCoroutine(CheckMating());
    }

    private void HippoCount()
    {
        hippoCountUI.text = "Hippos Alive: " + listOfHippos.Count;
    }

    private void Score()
    {
        if (listOfHippos.Count > maxHippoNumber || listOfHippos.Count < minHippoNumber)
        {
            currentScore--;
            if (currentScore < 0) currentScore = 0;
        }
        else
        {
            currentScore++;
        }

        scoreUI.text = "Money: " + currentScore;
    }

    public void AddMaleHippo()
    {
        if (currentScore >= 10)
        {
            GameObject instantiatedHippo = Instantiate(hippoGO);
            instantiatedHippo.GetComponent<Hippo_AI>().hippoGender = Hippo_AI.HIPPO_GENDER.M;
            listOfHippos.Add(instantiatedHippo);

            currentScore -= 10;
        }        
    }

    public void AddFemaleHippo()
    {
        if (currentScore >= 10)
        {
            GameObject instantiatedHippo = Instantiate(hippoGO);
            instantiatedHippo.GetComponent<Hippo_AI>().hippoGender = Hippo_AI.HIPPO_GENDER.F;
            listOfHippos.Add(instantiatedHippo);

            currentScore -= 10;
        }
    }

    private IEnumerator Score_Coroutine()
    {
        yield return new WaitForSeconds(1f);
        Score();
        HippoCount();
        StartCoroutine(Score_Coroutine());
    }
}
