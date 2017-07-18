using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	[SerializeField] private AudioSource audio1;
	[SerializeField] private AudioSource audio2;
	[SerializeField] private GameObject audiosource1;
	[SerializeField] private GameObject audiosource2;
	UnityMidi.MidiPlayer2 midiscript;
	AddAudioDimention script;


	//Audio1,2それぞれでStart・Stopが押されたときの挙動
	public void onAudio1PlayClick(){
		//おんげんからMidiPlayerスクリプトを取得
		midiscript = audiosource1.GetComponent<UnityMidi.MidiPlayer2> ();
		midiscript.LoadAndPlay ();
	}

	public void onAudio1StopClick(){
		midiscript = audiosource1.GetComponent<UnityMidi.MidiPlayer2> ();
		midiscript.Stop ();
	}

	public void onAudio2PlayClick(){
		midiscript = audiosource2.GetComponent<UnityMidi.MidiPlayer2> ();
		midiscript.LoadAndPlay ();
	}

	public void onAudio2StopClick(){
		midiscript = audiosource2.GetComponent<UnityMidi.MidiPlayer2> ();
		midiscript.Stop ();
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
