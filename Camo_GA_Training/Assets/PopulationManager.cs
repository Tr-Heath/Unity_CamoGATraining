using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

	public GameObject personPrefab;
	public int populationSize = 10;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;
    int trialTime = 10;
    int generation = 1;

    GUIStyle guiStyle = new GUIStyle();
    private void OnGUI()
    {
        guiStyle.fontSize = 50;
        guiStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 100, 50), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 60, 100, 50), "Trial Time: " + (int)elapsed, guiStyle);
    }
    // Use this for initialization
    void Start () {
		for(int i = 0; i < populationSize; i++)
		{
			Vector3 pos = new Vector3(Random.Range(-9,9), Random.Range(-4.5f,4.5f),0);
		    GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
            // GameObject go = Instantiate()
            go.GetComponent<DNA>().geneColor = new DNA.GeneColor(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            go.GetComponent<DNA>().geneSize = new DNA.GeneSize(Random.Range(0.2f, 0.4f), Random.Range(0.2f, 0.4f));
			population.Add(go);
		}
	}

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);
        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();

        //swap the parent's gene information
        offspring.GetComponent<DNA>().geneColor = GetOffspringGeneColor(dna1, dna2);
        offspring.GetComponent<DNA>().geneSize = GetOffspringGeneSize(dna1, dna2);

        return offspring;
    }

    DNA.GeneColor GetOffspringGeneColor(DNA parent1, DNA parent2)
    {
        // 30% of the time, use the parent's gene's, else lets go for a random mutation.
        if(Random.Range(0,10) > 1)
        {
            return new DNA.GeneColor(Random.Range(0, 10) < 5 ? parent1.geneColor.r : parent2.geneColor.r,
                                     Random.Range(0, 10) < 5 ? parent1.geneColor.g : parent2.geneColor.g,
                                     Random.Range(0, 10) < 5 ? parent1.geneColor.b : parent2.geneColor.b);
        }
        return new DNA.GeneColor(Random.Range(0.0f, 1.0f),
                                 Random.Range(0.0f, 1.0f),
                                 Random.Range(0.0f, 1.0f));
    }

    DNA.GeneSize GetOffspringGeneSize(DNA parent1, DNA parent2)
    {
        // 70% of the time, use the parent's gene's, else lets go for a random mutation.
        if(Random.Range(0,10) > 1)
        {
            return new DNA.GeneSize(Random.Range(1,10) < 5 ? parent1.geneSize.x : parent2.geneSize.x,
                                    Random.Range(1,10) < 5 ? parent1.geneSize.y : parent2.geneSize.y);
        }
        return new DNA.GeneSize(Random.Range(0.2f, 0.4f), Random.Range(0.2f, 0.4f));
    }

    void BreedNewPopulation()
    {
        List<GameObject> newPopulation = new List<GameObject>();
        // make an ordered list of individuals based on "fitness" (how long they lived for).
        List<GameObject> sortedList = population.OrderBy(o => o.GetComponent<DNA>().timeTillDeath).ToList();

        population.Clear();
        for (int i = (int)(sortedList.Count / 2.0f) - 1; i < sortedList.Count - 1; i++){
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }

        for(int i = 0; i < sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }
	
	// Update is called once per frame
	void Update () {
        elapsed += Time.deltaTime;
        if(elapsed > trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
	}
}
