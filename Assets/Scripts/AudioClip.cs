//ただいま変更前
//変更後はこのスクリプトをそれぞれのオブジェクトいくっつけて、指定したファイル名のフォルダができたら読み込むというところだけ
//MIDIの扱いはしらぬ


using UnityEngine;
using System.Collections;

public class AudioClip : MonoBehaviour {
	public UnityEngine.AudioClip audioClip1;
	public UnityEngine.AudioClip audioClip2;
	public UnityEngine.AudioClip audioClip3;
	private AudioSource audioSource;

	void Start(){
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = audioClip1;
	}

	void Update(){

		if (Input.GetKeyDown (KeyCode.R)) {
			Debug.Log ("RESET");
			/*音の停止*/
			StopAudio ();

		}

		if (Input.GetKeyDown (KeyCode.S)) {
			Debug.Log("START");
			/*音のなり始め*/
			StartAudio ();

		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log("OK");
		}

		/*音の変更*/
		if(Input.GetKeyDown("1")){
			audioSource.clip = audioClip1;
		}

		if(Input.GetKeyDown("2")){
			audioSource.clip = audioClip2;
		}

		if(Input.GetKeyDown("3")){
			audioSource.clip = audioClip3;
		}

	}

	public void StartAudio () {
		audioSource.Play ();
		//重ねて再生するばあい
		//audioSource.PlayOneShot(audioClip);
	}

	public void StopAudio(){
		audioSource.Stop ();
	}

}
