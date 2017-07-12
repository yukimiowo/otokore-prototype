﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AddAudioDimention : MonoBehaviour {
	private TextAsset csvFile;
	private List<string[]> posdata = new List<string[]>();
	//名前を読み込みたい場所ごとに切り替えられたらいいね　別のスクリプトでファイル名を値として受け渡すのもいいかもね
	//ためしにGUIでつくってみてもいいかもね A B C とかってマーカの名前を決めて
	string filename = "te";
	string audioname = "st";
	//Audioもここで読み取れるとなおよし　とりあえずwavで
	//AudioはAudioClip.csで読み込む　とりあえず　midiになったらしらん
	public int markid;
	private int linenum = 0;
	private bool lineflag = false;
	private int lineend = 0;

	public AudioSource AudioSource;


	// Use this for initialization
	void Start () {

		// ただ1回ファイルを読み込みたいだけならここ 
//		csvFile = Resources.Load ("CSV/" + filename + audioname) as TextAsset;
//		StringReader reader = new StringReader (csvFile.text);
//
//		//行がなくなるまでおこなう
//		while(reader.Peek() > -1) {
//		string line = reader.ReadLine ();
//		posdata.Add (line.Split (','));
//			}
//
//		this.transform.position = new Vector3 (float.Parse (posdata [0] [0]), float.Parse (posdata [0] [1]), float.Parse (posdata [0] [2]));

		StartCoroutine ("PositionLoad");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	private IEnumerator PositionLoad(){
		while (true) {
			
			//もしファイルがなかったら一旦停止してその後のロードを行わせない
			string csvPath = "Assets/Resources/CSV/" + filename + audioname + ".csv";
			while(System.IO.File.Exists(csvPath) == false ) {
				Debug.Log ("File doesn't exist");
				float rantime = Random.Range(0.1f, 0.4f);
				yield return new WaitForSeconds (rantime);
			}

			csvFile = Resources.Load ("CSV/" + filename + audioname) as TextAsset;
			StringReader reader = new StringReader (csvFile.text);

			linenum = 0;
			lineend = 0;
			lineflag = false;
			//行がなくなるまでおこなう
			while (reader.Peek () > -1) {
				string line = reader.ReadLine ();
				posdata.Add (line.Split (','));
				if (int.Parse(posdata [linenum] [0]) == markid) {
					lineflag = true;
				} else if (!lineflag) {
					linenum++;
				}
				lineend++;
			}

			if(linenum == lineend){
				AudioSource.mute = true;
				Debug.Log ("mute");
			} else{
				//音を鳴らし始める
				AudioSource.mute = false;
				this.transform.position = new Vector3 (float.Parse (posdata [linenum] [1]), float.Parse (posdata [linenum] [2]), float.Parse (posdata [linenum] [3]));
				//音のぶれ確認用↓
	//			var x = 5 * Mathf.Sin(Time.time);
	//			var z = 5 * Mathf.Cos(Time.time);
	//			transform.position = new Vector3(x, 0.5f, z);
				Debug.Log (posdata [linenum] [1]);
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	/* サーバにアクセスするようにするにはきっとこんな感じ */
	
/*	private TextAsset csvFile;
	
	string filename = "te";
	string audioname = "st";
    string url = "http://127.0.0.1:5000/";	//csvファイルの置き場所
    float timeoutsec = 5f;

    public int markid = 0;
	private int linenum = 0;
	private bool lineflag = false;

	// Use this for initialization
	void Start () {

		StartCoroutine ("PositionLoad");
	}

	// Update is called once per frame
	void Update () {

	}

	private IEnumerator PositionLoad(){
		while (true) {

			WWW www = new WWW(url);

            yield return StartCoroutine(CheckTimeOut(www, timeoutsec)); //ダウンロードが完了するのをまつ
            //yield return www;

            if (www.error != null)
            {
                Debug.Log("ERROR:" + www.error);
            }
            else if (www.isDone)
            {
                Debug.Log("Success:" + www.text);
                
                StringReader reader = new StringReader(www.text);
                List<string[]> posdata = new List<string[]>();

				linenum = 0;
				lineend = 0;
                //行がなくなるまでおこなう
				//指定されたidの行を探し出してそれを取得する
				while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();
                    posdata.Add(line.Split(','));
					if (int.Parse(posdata [linenum] [0]) == markid) {
						lineflag = true;
					} else if (!lineflag) {
						linenum++;
					}
                }

				//idの行がなかったら音をストップする
				if(linenum == lineend){
					AudioSource.mute = true;
					Debug.Log("mute");
				} else{
					AudioSource.mute = false;
	                this.transform.position = new Vector3(float.Parse(posdata[linenum][1]), float.Parse(posdata[linenum][2]), float.Parse(posdata[linenum][3]));

					//指定されたところまで滑らかに動けるようにする
	                Debug.Log (posdata[linenum][1]);
				}
            }

//			var x = 5 * Mathf.Sin(Time.time);
//			var z = 5 * Mathf.Cos(Time.time);
//			transform.position = new Vector3(x, 0.5f, z);

			yield return new WaitForSeconds (0.5f);
		}
	}

    IEnumerator CheckTimeOut(WWW www, float timeout)
    {
        float requestTime = Time.time;
        while (!www.isDone)
        {
            if (Time.time - requestTime < timeout)
                yield return null;
            else
            {
                Debug.Log("TimeOut");
                break;
            }
        }
    }*/

	
}
