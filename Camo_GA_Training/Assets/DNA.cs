using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA : MonoBehaviour {

    bool dead = false;
    public float timeTillDeath = 0;
    Collider2D sCollider;
    SpriteRenderer sRenderer;
    public GeneColor geneColor;

    //gene for color, floats for Red, Green, Blue
    public struct GeneColor {
		public float r; 
		public float g; 
		public float b;
		public GeneColor(float redValue, float greenValue, float blueValue){
			r = redValue;
			g = greenValue;
			b = blueValue;
		}
	};

	void OnMouseDown() {
		dead = true;
		timeTillDeath = PopulationManager.elapsed;
		// Debug.Log("Dead at: " + timeTillDeath);
		sRenderer.enabled = false;
		sCollider.enabled = false;
	}

	// Use this for initialization
	void Start () {
		sRenderer = GetComponent<SpriteRenderer>();
		sCollider = GetComponent<Collider2D>();
        sRenderer.color = new Color(geneColor.r, geneColor.g, geneColor.b);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
