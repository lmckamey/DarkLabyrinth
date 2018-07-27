using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
	public Transform m_startTransform = null;
	public Transform m_endTransform = null;
	[SerializeField] Transform m_particleSystem = null;
	[SerializeField] Transform m_flare01 = null;
	[SerializeField] Transform m_flare02 = null;
	[SerializeField] [Range(2, 50)] int m_segmentsMax = 50;
	[SerializeField] [Range(0.0f,  5.0f)] float m_segmentLength = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_waveAmplitude = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_waveRate = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_waveStep = 1.0f;
	[SerializeField] [Range(0.0f, 10.0f)] float m_randomAmplitude = 1.0f;
	[SerializeField] AnimationCurve m_shape;

	LineRenderer m_line = null;
	List<Vector3> m_points = null;
	int m_segments = 0;
	float m_time = 0.0f;
	Vector3 m_wave = Vector3.zero;

	void Start()
	{
		m_line = GetComponent<LineRenderer>();
		m_line.positionCount = m_segmentsMax;
		m_points = new List<Vector3>(new Vector3[m_segmentsMax]);
		m_time = Random.value * 10000.0f;
	}

	void Update()
	{
		m_time = m_time + (Time.deltaTime * m_waveRate);
		
		Vector3 v3 = m_endTransform.position - m_startTransform.position;
		Vector3 fnv3 = v3.normalized;
		float length = v3.magnitude;
		int m_segments = (int)(length / m_segmentLength);
		m_segments = Mathf.Max(2, m_segments);
		float segmentLength = (m_segments > 2) ? length / (m_segments - 1) : length;

		m_line.positionCount = m_segments;

		Quaternion forward = Quaternion.LookRotation(fnv3, Vector3.up);
		for (int i = 0; i < m_segments; i++)
		{
			float t = i / (float)(m_segments - 1);

			m_wave.x = (Mathf.PerlinNoise(m_time, t * m_waveStep) * 2.0f) - 1.0f;
			m_wave.y = (Mathf.PerlinNoise(t * m_waveStep, m_time) * 2.0f) - 1.0f;
			m_wave.z = 0.0f;
			Vector3 wave = (forward * m_wave) * m_waveAmplitude;
			Vector3 random = Random.onUnitSphere * m_randomAmplitude;
			Vector3 offset = (wave + random) * m_shape.Evaluate(t);
			m_points[i] = m_startTransform.position + (fnv3 * (i * segmentLength)) + offset;
		}

		m_line.SetPositions(m_points.ToArray());
	
		if (m_particleSystem) m_particleSystem.position = m_points[m_segments - 1];
		if (m_flare01) m_flare01.position = m_points[0];
		if (m_flare02) m_flare02.position = m_points[m_segments - 1];
	}
}

