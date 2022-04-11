using UnityEngine;

public class Breakable : MonoBehaviour
{
    //Object to be enable when broken
    [SerializeField]
    private GameObject broken;
    //Object to be disabled when broken
    [SerializeField]
    private GameObject not_broken;

	//Particle effect to play upon break
	[SerializeField]
	private ParticleSystem poof;

	[SerializeField]
	private AudioSource breakSfx;

    //The force at which pieces are flung when the object is broken
    [SerializeField]
    float explosionForce = 200f;
    //Higher the value the more the explosion goes up
    [SerializeField]
    float explosionUpwardsModifier = 1f;


    //SOUND
    [Range(0, 1)] [SerializeField] private float sfxLocalMultipler = 1;
    void Start()
    {
        not_broken.SetActive(true);
        broken.SetActive(false);

		if (breakSfx == null)
		{
			breakSfx = gameObject.GetComponent<AudioSource>();
		}
	}

    public void Break()
    {
		if (broken != null && not_broken != null)
		{
			//If not already broken
			if (!broken.activeSelf)
			{
				//Set broken to be in the same position as unbroken in case the un broken object moved before breaking
				broken.transform.position = not_broken.transform.position + broken.transform.localPosition;
				broken.transform.rotation = not_broken.transform.rotation;

				//Enable the broken object
				broken.SetActive(true);

				//Give every broken piece the same velocity as the unbroken object in case the object was falling while being broken
				//Also add an explosion force

				Rigidbody thisRb = not_broken.GetComponent<Rigidbody>();
				foreach (Rigidbody rb in broken.GetComponentsInChildren<Rigidbody>())
				{
					rb.velocity = thisRb.velocity;
					Vector3 randomVector = new Vector3(Random.Range(0, 0.5f), Random.Range(0, 0.5f), Random.Range(0, 0.5f));
					rb.AddExplosionForce(explosionForce, thisRb.position + randomVector, Mathf.Infinity, explosionUpwardsModifier, ForceMode.Force);
				}

				//Get rid of the unbroken object
				Destroy(not_broken.gameObject);
			}
		}

		//Play particle anim
		if (poof != null)
		{
			poof.Play();
		}
		
		//Play sfx
		if (breakSfx != null)
		{
            FindObjectOfType<SoundManager>().RequestPlaySound(breakSfx, sfxLocalMultipler);
		}
    }
}
