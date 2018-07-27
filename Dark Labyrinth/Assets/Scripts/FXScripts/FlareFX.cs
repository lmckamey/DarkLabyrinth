using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareFX : MonoBehaviour
{
	[SerializeField] [Range(0.0f, 10.0f)] float m_minScale = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_maxScale = 1.0f;
	[SerializeField] [Range(0.0f,  1.0f)] float m_minAlpha = 1.0f;
	[SerializeField] [Range(0.0f,  1.0f)] float m_maxAlpha = 1.0f;
	[SerializeField] [Range(0.0f, 1.0f)] float m_rate = 1.0f;

	float m_time = 0.0f;
	Material m_material;
	Color m_color;
	Vector3 m_scale;

	void Start()
	{
		m_material = GetComponent<Renderer>().material;
		m_color = m_material.GetColor("_TintColor");
		m_scale = transform.localScale;
	}

	void Update()
	{
		m_time = m_time - Time.deltaTime;
		if (m_time <= 0.0f)
		{
			m_time = m_rate;

			transform.localScale = m_scale * Mathf.Lerp(m_minScale, m_maxScale, Random.value);
			float a = Mathf.Lerp(m_minAlpha, m_maxAlpha, Random.value);
			m_color.a = a;
			m_material.SetColor("_TintColor", m_color);
		}
	}
}
