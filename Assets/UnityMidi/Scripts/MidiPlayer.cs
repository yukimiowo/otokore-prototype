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
    [RequireComponent(typeof(AudioSource))]
    public class MidiPlayer : MonoBehaviour
    {
        [SerializeField] StreamingAssetResouce bankSource;
        [SerializeField] StreamingAssetResouce midiSource;
        [SerializeField] bool loadOnAwake = true;	//これがfalseだったらbankとmidiを読み込まない
        [SerializeField] bool playOnAwake = true;	//これがfalseだったら取り込んだ情報で演奏を始めない
        [SerializeField] int channel = 1;
        [SerializeField] int sampleRate = 44100;
        [SerializeField] int bufferSize = 1024;
        PatchBank bank;
        MidiFile midi;
        Synthesizer synthesizer;
        AudioSource audioSource;
        MidiFileSequencer sequencer;
        int bufferHead;
        float[] currentBuffer;

        public AudioSource AudioSource { get { return audioSource; } }

        public MidiFileSequencer Sequencer { get { return sequencer; } }

        public PatchBank Bank { get { return bank; } }

        public MidiFile MidiFile { get { return midi; } }

        public void Awake()	//新しいシンセサイザ、シークエンサ、音源の確保
        {
            synthesizer = new Synthesizer(sampleRate, channel, bufferSize, 1);
            sequencer = new MidiFileSequencer(synthesizer);
            audioSource = GetComponent<AudioSource>();

            if (loadOnAwake)
            {
                LoadBank(new PatchBank(bankSource));
                LoadMidi(new MidiFile(midiSource));
            }

            if (playOnAwake)
            {
                Play();
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
            audioSource.Play();
        }

        void OnAudioFilterRead(float[] data, int channel)
        {
            Debug.Assert(this.channel == channel);
            int count = 0;
            while (count < data.Length)
            {
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
