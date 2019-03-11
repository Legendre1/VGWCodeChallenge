using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalSystemManager : MonoBehaviour {

	private class QueuedModalInfo
	{
		//this class contains all the info to define a modal instance. it could have button texts, callbacks, images etc 
		public string content_text;
	}

	//Presentation details
	public float m_modal_transition_time;
	public Color m_blocker_color;


	//Element references
	public RectTransform m_modal_base_transform;
	public Text m_content_text;
	public Image m_blocker_image;


	private Vector2 m_modal_rest_position;
	private List<QueuedModalInfo> m_modal_queue;
	private bool m_modal_is_showing;
	private bool m_modal_dismissed;


	// Use this for initialization
	void Start () {
		m_modal_queue = new List<QueuedModalInfo>();
		m_modal_is_showing = false;
		m_modal_rest_position = m_modal_base_transform.anchoredPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_modal_queue.Count > 0)
		{
			if(!m_modal_is_showing)
			{
				popModal();
			}
		}
	}

	#region Public Access
	public void showSingleButtonModal(string content)
	{
		QueuedModalInfo new_modal_info = new QueuedModalInfo();
		new_modal_info.content_text = content;
		m_modal_queue.Add(new_modal_info);
	}

	#endregion

	public void onButtonPressed()
	{
		//this can handle button calbacks etc, but here it just dismisses the modal
		m_modal_dismissed = true;
	}

	private void popModal()
	{
		QueuedModalInfo next_modal_in_queue = m_modal_queue[0];
		StartCoroutine(showModal(next_modal_in_queue));
		m_modal_queue.RemoveAt(0);
	}


	#region Coroutines

	private IEnumerator showModal(QueuedModalInfo modal_info)
	{
		//this could be implemented to recieve various button texts, calbacks etc but I am keeping it simple here
		
		m_modal_is_showing = true;
		m_modal_dismissed = false;

		//setup the modal text etc
		m_content_text.text = modal_info.content_text;
		m_blocker_image.gameObject.SetActive(true);

		//transition the modal in
		float enter_lifetime = 0.0f;
		while(enter_lifetime < m_modal_transition_time)
		{
			float normalized_transition = enter_lifetime / m_modal_transition_time;
			Color blocker_color = Color.Lerp(Color.clear, m_blocker_color, normalized_transition);
			m_blocker_image.color = blocker_color;

			Vector2 modal_position = Vector2.Lerp(m_modal_rest_position, Vector2.zero, normalized_transition);
			m_modal_base_transform.anchoredPosition = modal_position;

			yield return 0;
			enter_lifetime += Time.deltaTime;
		}
		//force the final color, position
		m_blocker_image.color = m_blocker_color;
		m_modal_base_transform.anchoredPosition = Vector2.zero;

		//wait for a button press
		yield return new WaitUntil(() => m_modal_dismissed == true);

		//transition the modal out
		float exit_lifetime = 0.0f;
		while(exit_lifetime < m_modal_transition_time)
		{
			float normalized_transition = 1 - (exit_lifetime / m_modal_transition_time);
			Color blocker_color = Color.Lerp(Color.clear, m_blocker_color, normalized_transition);
			m_blocker_image.color = blocker_color;

			Vector2 modal_position = Vector2.Lerp(m_modal_rest_position, Vector2.zero, normalized_transition);
			m_modal_base_transform.anchoredPosition = modal_position;

			yield return 0;
			exit_lifetime += Time.deltaTime;
		}

		//wrap it up
		m_blocker_image.gameObject.SetActive(false);
		m_modal_is_showing = false;

	}

	#endregion
}
