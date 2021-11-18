using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hippo_AI : MonoBehaviour
{
	private HippoManager hippoManager;

	public Material femaleHippoMat;
	public Material maleHippoMat;

	public enum HIPPO_GENDER
	{
		M,
		F,
	}
	public int hippoAge { get; private set; }
	public int hippoPregnantAge { get; private set; }
	public HIPPO_GENDER hippoGender;
	public bool hasMate { get; private set; }
	public bool isPregnant { get; private set; }

	private bool recurringPregnancy;
	private int hippoMaxAge;

	#region Movement
	public Vector3 minBounds;
	public Vector3 maxBounds;

	private Vector3 newPosition;
	private Vector3 startPosition;

	private float startTime;
	private float randomDuration;
	#endregion

	private void Start()
	{
		hippoManager = HippoManager.current;

		if(hippoGender == HIPPO_GENDER.M) GetComponent<Renderer>().material = maleHippoMat;
		if(hippoGender == HIPPO_GENDER.F) GetComponent<Renderer>().material = femaleHippoMat;

		hippoAge = 0;
		hippoMaxAge = Random.Range(10, 16);

		StartCoroutine(GoToNextPoint());
		StartCoroutine(GetOlder_Coroutine());
	}

	private void Update()
	{
		transform.position = Vector3.Lerp(startPosition, newPosition, (Time.time - startTime) / randomDuration);
		Debug.Log(isPregnant);
	}

	#region Hippo Stats
	public void SetRandomGender()
	{
		hippoGender = Random.Range(0, 2) == 1 ? HIPPO_GENDER.M : HIPPO_GENDER.F;
	}

	public void SetHasMate(bool hasMate)
	{
		this.hasMate = hasMate;
	}
	public void SetPregnant()
	{
		hippoPregnantAge = hippoAge;
		isPregnant = true;
		recurringPregnancy = true;
	}

	private void SetRecurringPregnancy()
	{
		if (!recurringPregnancy || isPregnant) return;

		if(Random.Range(0, 2) == 1) SetPregnant();
	}

	private void CheckGiveBirth()
	{
		if (hippoPregnantAge + 1 == hippoAge && isPregnant)
		{
			hippoManager.Breed();
			isPregnant = false;
		}
	}

	public void Die()
	{
		hippoManager.HippoDeath(gameObject);
	}

	private void CheckDeath()
	{
		if (hippoAge >= 10)
		{
			hippoManager.HippoDeath(gameObject);
		}
	}

	private void SetSize()
	{
		transform.localScale *= 1.1f;
	}

	private IEnumerator GetOlder_Coroutine()
	{
		yield return new WaitForSeconds(3f);
		
		hippoAge++;

		CheckDeath();
		SetSize();
		CheckGiveBirth();
		SetRecurringPregnancy();

		StartCoroutine(GetOlder_Coroutine());
	}
	#endregion

	#region Movement Functions
	private void SetNewPosition()
	{
		float newXPos = Random.Range(-10f, 10f);
		if (newXPos < minBounds.x) newXPos = minBounds.x;
		float newZPos = Random.Range(-10f, 10f);
		if (newZPos < minBounds.z) newXPos = minBounds.z;

		newPosition = new Vector3(newXPos, 0, newZPos);
		startPosition = transform.position;

		randomDuration = Random.Range(10f, 15f);
		startTime = Time.time;
	}

	private IEnumerator GoToNextPoint()
	{
		SetNewPosition();
		yield return new WaitForSeconds(Random.Range(10f, 30f));
		StartCoroutine(GoToNextPoint());
	}
	#endregion
}
