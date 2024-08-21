using UnityEngine;
using System.Collections;
using TMPro;

public class DamageUI : MonoBehaviour
{

	private TextMeshProUGUI damageText;
	//　フェードアウトするスピード
	private float fadeOutSpeed = 1f;
	//　移動値
	[SerializeField]
	private float moveSpeed = 0.4f;

	/// <summary>
    /// DamageUIのパラメーター初期化
    /// とりあえずInstantiateはしない方針
    /// </summary>
    /// <param name="txt">表示するテキスト</param>
	public void Initialize(string txt)
    {
		damageText = GetComponentInChildren<TextMeshProUGUI>();
		damageText.text = txt;
	}

	
	void Start()
	{
		damageText = GetComponentInChildren<TextMeshProUGUI>();
	}
	

	void LateUpdate()
	{
		transform.rotation = Camera.main.transform.rotation;
		transform.position += Vector3.up * moveSpeed * Time.deltaTime;

		damageText.color = Color.Lerp(damageText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);

		if (damageText.color.a <= 0.1f)
		{
			Destroy(gameObject);
		}
	}
}