using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	[SerializeField] private AudioSource audio1;
	[SerializeField] private AudioSource audio2;
	[SerializeField] private GameObject audiosource1;
	[SerializeField] private GameObject audiosource2;
	AddAudioDimention script;


	//Audio1,2それぞれでStart・Stopが押されたときの挙動
	public void onAudio1PlayClick(){
		audio1.Play ();
	}

	public void onAudio1StopClick(){
		audio1.Stop ();
	}

	public void onAudio2PlayClick(){
		audio2.Play ();
	}

	public void onAudio2StopClick(){
		audio2.Stop();
	}
		
	//Audio1,2それぞれのmarkidが変わった時の挙動

	public void onValueChanged1(Dropdown dropdown){
		
		script = audiosource1.GetComponent<AddAudioDimention> ();

		switch (dropdown.value) {
		case 0:
			script.markid = -1;
			break;
		case 1:
			script.markid = 0;
			break;
		case 2:
			script.markid = 1;
			break;
		default:
			break;
		}
	}

	public void onValueChanged2(Dropdown dropdown){

		script = audiosource2.GetComponent<AddAudioDimention> ();

		switch (dropdown.value) {
		case 0:
			script.markid = -1;
			break;
		case 1:
			script.markid = 0;
			break;
		case 2:
			script.markid = 1;
			break;
		default:
			break;
		}
	}

}
