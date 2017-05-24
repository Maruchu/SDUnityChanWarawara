//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
//
//	適当に動くキャラクタースクリプト
//
//	Copyright(C)2016 Maruchu
//	http://maruchu.nobody.jp/
//
//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
using UnityEngine;
using System.Collections;




///	<summary>
///	適当に動くキャラクタークラス
///
///	キャラクターの移動、メカニム(モーション)の制御など
///	</summary>
public		class		WarawaraMover				: MonoBehaviour {




	public							GameObject	m_charaObject			= null;		//動かす対象のモデル
	public							GameObject	m_walkEffect			= null;		//歩くときのエフェクト

	private							Animator	m_charaAnimator			= null;		//メカニムを持っているアニメーター




	private							float		m_localScale_Now		= 1.0f;		//キャラのスケール
	private							float		m_rotationY				= 0.0f;		//プレーヤーの回転角度


	//AI用の情報
	public							float		m_waitSec_Min			= 2.0f;		//AIの思考間隔の最小(秒)
	public							float		m_waitSec_Max			= 5.0f;		//AIの思考間隔の最大(秒)

	private							float		m_waitSec_Now			= 0.0f;		//AIの思考間隔 現在の待ち時間(秒)


	public							float		m_stopPercent			= 0.3f;		//AIが止まっているだけの行動を選ぶ確率(0.0f～1.0f、1.0fで100％)


	public							float		m_addPosZ_Min			= 0.8f;		//AIの移動速度の最小(メートル)
	public							float		m_addPosZ_Max			= 2.5f;		//AIの移動速度の最大(メートル)
	public							float		m_addRotY_Max			= 60.0f;	//AIの回転の最大(0～360度)

	private							float		m_addPosZ_Now			= 0.0f;		//AIが動く速さ 直進移動 現在
	private							float		m_addPosZ_Next			= 0.0f;		//AIが動く速さ 直進移動 ゆっくりこの速さに変わる
	private							float		m_addRotY_Now			= 0.0f;		//AIが動く速さ 回転 現在


	public		static readonly		float		FLYING_HEIGHT			= 0.5f;		//空中判定の境界線




	///	<summary>
	///	初期化時
	///	</summary>
	private		void	Awake() {
		//メカニム用アニメーター取得
		m_charaAnimator			= m_charaObject.GetComponent<Animator>();

		//角度を取得
		m_rotationY				= transform.rotation.eulerAngles.y;
		//スケールを取得、一応xyzを揃えておく
		m_localScale_Now		= transform.localScale.y;
		transform.localScale	= (Vector3.one *m_localScale_Now);
	}
	///	<summary>
	///	毎フレーム呼び出される関数
	///	</summary>
	private		void	Update() {
		//移動処理
		CheckMove();
	}


	///	<summary>
	///	移動処理のチェック
	///	</summary>
	private		void	CheckMove() {

		//AIの思考更新タイマー
		m_waitSec_Now	-= Time.deltaTime;
		//タイマーが0以下なら再抽選
		if( m_waitSec_Now < 0f) {

			//AIの思考を抽選
			m_waitSec_Now	= Random.Range( m_waitSec_Min, m_waitSec_Max);

			//止まるかどうか確認
			if( Random.value < m_stopPercent) {
				//一定時間止まる
				m_addPosZ_Next	= 0;
				m_addRotY_Now	= 0;
			} else {
				//一定時間動く(回転しながら直進)
				m_addPosZ_Next	=
				m_addPosZ_Now	= Random.Range(  m_addPosZ_Min, m_addPosZ_Max);
				m_addRotY_Now	= Random.Range( -m_addRotY_Max, m_addRotY_Max);
			}
		}


		//地上か空中かの判定
		bool	flyingFlag;
		{
			//位置が高い？
			if( transform.position.y > FLYING_HEIGHT) {
				//空中
				flyingFlag	= true;
			} else {
				//地上
				flyingFlag	= false;
			}

			//歩くときのエフェクトは地上のみ
			if( null!=m_walkEffect) {
				m_walkEffect.SetActive( !flyingFlag);
			}
		}


		//回転
		{
			//現在の角度に加算
			m_rotationY			+= (m_addRotY_Now	*Time.deltaTime);
			//オイラー角で入れる
			transform.rotation	= Quaternion.Euler( 0, m_rotationY, 0);
		}

		//移動
		{
			//速度はゆっくり変化させる
			m_addPosZ_Now	= ((m_addPosZ_Next *0.1f) +(m_addPosZ_Now *0.9f));
			//ジャンプ中は移動しない
			if( false==flyingFlag) {
				//移動量を Transform に渡して移動させる
				transform.position	+= ((transform.rotation	 	*new Vector3( 0f, 0, m_addPosZ_Now	*m_localScale_Now))		*Time.deltaTime);		//移動量にはプレハブのスケールも加味
			}
		}


		//メカニム(モーション)
		if( null!=m_charaAnimator) {
			//Animator に値を渡す
			m_charaAnimator.SetFloat(	"SpeedZ",	m_addPosZ_Now);		//前進の移動量
			m_charaAnimator.SetBool(	"Flying",	flyingFlag);		//空中フラグ
		}
	}




}
