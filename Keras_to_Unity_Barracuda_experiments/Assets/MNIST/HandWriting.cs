using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandWriting : MonoBehaviour
{
    [SerializeField] BarracudaMnist _barracudaMnist;
	[SerializeField] GameObject plane;
	Texture2D tex;
	[SerializeField] TextMeshProUGUI kerasText;
	[SerializeField] TextMeshProUGUI mnist8Text;

	void Start()
	{
		tex = new Texture2D(28, 28);
		plane.GetComponent<Renderer>().sharedMaterial.mainTexture = tex;
		Clear();
	}

	void Update()
	{
		HandWrite();
		PredictByKerasModel();
		PredictByMNIST8Model();
	}


	public void PredictByKerasModel()
	{
		var n = _barracudaMnist.PredictByKerasModel(tex);
		kerasText.text = "Keras : " + n.ToString();
	}

	public void PredictByMNIST8Model()
	{
		var n = _barracudaMnist.PredictByMNIST8Model(tex);
		mnist8Text.text = "MNIST8 : " + n.ToString();
	}


	// https://gist.github.com/huytd/9569548
	private void HandWrite()
    {
		if (tex != null)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Input.GetButton("Fire1"))
			{
				if (Physics.Raycast(ray, out hit, Mathf.Infinity))
				{
					// Find the u,v coordinate of the Texture
					Vector2 uv;
					uv.x = (hit.point.x - hit.collider.bounds.min.x) / hit.collider.bounds.size.x;
					uv.y = (hit.point.y - hit.collider.bounds.min.y) / hit.collider.bounds.size.y;
					// Paint it red
					tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height), Color.white);
					tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) + 1, Color.white);
					tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height), Color.white);
					tex.SetPixel((int)(-uv.x * tex.width), (int)(uv.y * tex.height) - 1, Color.white);
					tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height), Color.white);
					tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) + 1, Color.white);
					tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) - 1, Color.white);
					tex.SetPixel((int)(-uv.x * tex.width) - 1, (int)(uv.y * tex.height) + 1, Color.white);
					tex.SetPixel((int)(-uv.x * tex.width) + 1, (int)(uv.y * tex.height) - 1, Color.white);
					tex.Apply();
				}
			}
			if (Input.GetButton("Fire2"))
			{
				Clear();
			}
		}
	}


	public void Clear()
	{
		for (int i = 0; i < 128; i++)
		{
			for (int j = 0; j < 128; j++)
			{
				tex.SetPixel(i, j, Color.black);
			}
		}
		tex.Apply();
	}
}
