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
        GUI.Label(new Rect(10, 10, 100, 20), "Generation: " + generation, guiStyle);
        GUI.Label(new Rect(10, 10, 100, 20), "Trial Time: " + (int)elapsed, guiStyle);
    }
    // Use this for initialization
    void Start () {
		for(int i = 0; i < populationSize; i++)
		{
			Vector3 pos = new Vector3(Random.Range(-9,9), Random.Range(-4.5f,4.5f),0);
			GameObject go = Instantiate(personPrefab, pos, Quaternion.identity);
            go.GetComponent<DNA>().geneColor = new DNA.GeneColor(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
			population.Add(go);
		}
	}

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        Vector3 pos = new Vector3(Random.Range(-9, 9), Random.Range(-4.5f, 4.5f), 0);
        GameObject offspring = Instantiate(personPrefab, pos, Quaternion.identity);
        DNA dna1 = parent1.GetComponent<DNA>();
        DNA dna2 = parent2.GetComponent<DNA>();
        //swap the parents DNA
        offspring.GetComponent<DNA>().geneColor = new DNA.GeneColor(Random.Range(0, 10) < 5 ? dna1.geneColor.r : dna2.geneColor.r,
                                                                    Random.Range(0, 10) < 5 ? dna1.geneColor.g : dna2.geneColor.g,
                                                                    Random.Range(0, 10) < 5 ? dna1.geneColor.b : dna2.geneColor.b);

        return offspring;
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
