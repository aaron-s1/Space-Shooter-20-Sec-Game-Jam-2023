using UnityEngine;
using System.Collections;

public class PSDestroy : MonoBehaviour {

	void Start () =>
		Destroy(gameObject, GetComponent<ParticleSystem>().main.duration);
}
