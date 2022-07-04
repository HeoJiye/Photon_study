using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	public static int HP;
	public static int Score;

	// 상대방 총알이랑 나랑 부딪히면 체력--;
	// 내 총알이랑 상대방이 부딪히면 스코어++;

	// 수치에 따른 UI 변화는 GameManager가 조정
}
