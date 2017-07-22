using UnityEngine;
using System.IO;
using System.Collections;
using AudioSynthesis;
using AudioSynthesis.Bank;
using AudioSynthesis.Synthesis;
using AudioSynthesis.Sequencer;
using AudioSynthesis.Midi;

namespace UnityMidi
{

	/* AudioSourceの格納は、1つ1つのオブジェクトによって内容を変える */
    [RequireComponent(typeof(AudioSource))]
    public class MidiPlayer2 : MonoBehaviour
    {
        [SerializeField] StreamingAssetResouce bankSource;
        [SerializeField] StreamingAssetResouce midiSource;
		//StreamingAssetResouce midiSource;
        //[SerializeField] bool loadOnAwake = true;	//これがfalseだったらbankとmidiを読み込まない
        //[SerializeField] bool playOnAwake = true;	//これがfalseだったら取り込んだ情報で演奏を始めない
		bool loadOnAwake = false;
        [SerializeField] int channel = 1;	//なぜか1だとエラーはいてスピードが早くなるのでインスペクターの方で2にしている
        [SerializeField] int sampleRate = 44100;
        [SerializeField] int bufferSize = 1024;
		[SerializeField] string MidiPath = "Assets/StreamingAssets/ExampleMidis/...";	//midiSourceをうまいことパスとしてつかえればいいんだけどな…
        PatchBank bank;
        MidiFile midi;
        Synthesizer synthesizer;
        AudioSource audioSource;
        MidiFileSequencer sequencer;
        int bufferHead;
        float[] currentBuffer;
		bool stopflag = true;

        public AudioSource AudioSource { get { return audioSource; } }

        public MidiFileSequencer Sequencer { get { return sequencer; } }

        public PatchBank Bank { get { return bank; } }

        public MidiFile MidiFile { get { return midi; } }

		public void Awake(){

			synthesizer = new Synthesizer(sampleRate, channel, bufferSize, 1);
			sequencer = new MidiFileSequencer(synthesizer);
			audioSource = GetComponent<AudioSource>();

		}

        public void Update()
        {
			//再生したいmidiファイルがあったらロードしていいトリガーをつくる
			if ((System.IO.File.Exists (MidiPath)) && !(loadOnAwake)) {
				loadOnAwake = true;
			}

			//pを押すとファイルがあれば再生される
			if (Input.GetKey(KeyCode.P) && loadOnAwake)
            {
				LoadBank(new PatchBank(bankSource));
				LoadMidi(new MidiFile(midiSource));
				Debug.Log ("play!");

				Play();
            }

			//sを押すとストップする
			if (Input.GetKey (KeyCode.S)) {
				Stop ();
			}

			//曲が終わったのときにもう一度流す
			if (!stopflag && !sequencer.IsPlaying) {
				Debug.Log ("Loop " + stopflag + " " + sequencer.IsPlaying);
				Play ();
			}

        }

        public void LoadBank(PatchBank bank)	//一度bankを読み込み直す
        {
            this.bank = bank;
            synthesizer.UnloadBank();
            synthesizer.LoadBank(bank);
        }

        public void LoadMidi(MidiFile midi)	//一度midiをストップして入れ直してる…？
        {
            this.midi = midi;
            sequencer.Stop();
            sequencer.UnloadMidi();
            sequencer.LoadMidi(midi);
        }

        public void Play()
        {
            sequencer.Play();
            audioSource.Play();	//これなくてもplayできちゃうんだが…
			stopflag = false;
        }

		public void Stop(){
			sequencer.Stop ();
			audioSource.Stop ();
			stopflag = true;
		}

		public void LoadAndPlay()
		{
			LoadBank(new PatchBank(bankSource));
			LoadMidi(new MidiFile(midiSource));
			Debug.Log ("play!");

			Play ();
		}

		//ミュート
		public void Mute(){
			sequencer.MuteAllChannels();
		}

		public void UnMute(){
			sequencer.UnMuteAllChannels ();
		}

		//こいつはこいつで勝手に呼び出されるっぽい
		//Mainとは別スレッド
        void OnAudioFilterRead(float[] data, int channel)
        {
            Debug.Assert(this.channel == channel);
            int count = 0;
			//データの長さになるまでまわしてるので、データがなくなったら最初に戻せばいいのでは？
            while (count < data.Length)
            {
				//このif文で何をしてるのか
                if (currentBuffer == null || bufferHead >= currentBuffer.Length)
                {
                    sequencer.FillMidiEventQueue();
                    synthesizer.GetNext();
                    currentBuffer = synthesizer.WorkingBuffer;
                    bufferHead = 0;
                }
                var length = Mathf.Min(currentBuffer.Length - bufferHead, data.Length - count);
                System.Array.Copy(currentBuffer, bufferHead, data, count, length);
                bufferHead += length;
                count += length;
            }
        }
    }
}
