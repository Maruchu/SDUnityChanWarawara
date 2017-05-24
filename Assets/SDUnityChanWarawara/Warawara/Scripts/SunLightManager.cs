//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
//
//	太陽光のライト制御スクリプト
//
//	Copyright(C)2016 Maruchu
//	http://maruchu.nobody.jp/
//
//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
using UnityEngine;
using System.Collections;




///	<summary>
///	太陽光のライト制御クラス
///	</summary>
public		class		SunLightManager				: MonoBehaviour {



	private		float			m_rotXSpeed_Normal		=  3.0f;						//毎秒かかる回転の量(360度で一回転)
	private		float			m_rotXSpeed_Fast		= 10.0f;						//毎秒かかる回転の量(360度で一回転)

	private		Vector3			m_rotationNow			= Vector3.zero;					//現在の回転の値


	private		float			m_skipRotX_Min			= 210f;							//スキップする角度
	private		float			m_skipRotX_Max			= 330f;							//スキップする角度

	private		float			m_fastRotX_Min			= 10f;							//早送りする角度
	private		float			m_fastRotX_Max			= 170f;							//早送りする角度



	///	<summary>
	///	初期化時
	///	</summary>
	private	void Start() {
		//角度を取得
		m_rotationNow			= transform.eulerAngles;
	}

	///	<summary>
	///	毎フレーム呼び出される関数
	///	</summary>
	private	void Update() {

		//夜のシーンは一気に飛ばす
		if( (m_rotationNow.x > m_skipRotX_Min) && (m_rotationNow.x < m_skipRotX_Max)) {
			m_rotationNow.x		= m_skipRotX_Max;
		}

		//加える角度
		float	addRotX			= m_rotXSpeed_Normal;
		//昼間も早送り(夕焼けだけゆっくり見せる)
		if( (m_rotationNow.x > m_fastRotX_Min) && (m_rotationNow.x < m_fastRotX_Max)) {
			addRotX				= m_rotXSpeed_Fast;
		}
		//経過時間分の回転を加える
		m_rotationNow.x			= ((m_rotationNow.x	+(addRotX		*Time.deltaTime))	%360f);

		//Transformを更新して角度を反映
		transform.rotation		= Quaternion.Euler( m_rotationNow);
	}

}
