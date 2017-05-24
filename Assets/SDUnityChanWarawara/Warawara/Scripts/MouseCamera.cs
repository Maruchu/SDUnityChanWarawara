//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
//
//	Oculus Rift 持ってない(Leap Motion しか持ってない)人用のカメラをマウスで動かすスクリプト
//
//	Copyright(C)2016 Maruchu
//	http://maruchu.nobody.jp/
//
//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
using UnityEngine;
using System.Collections;




///	<summary>
///	Oculus Rift 持ってない人用マウスでカメラを動かすクラス
///	
///	Escキーでマウスのロック有効/解除
///	</summary>
public		class		MouseCamera					: MonoBehaviour {




	private		static readonly		float		ROTATION_X_MOUSE		= -180.0f;	//回転の速度 横
	private		static readonly		float		ROTATION_Y_MOUSE		=  360.0f;	//回転の速度 縦

	private							float		m_rotationX				= 0.0f;		//回転角度
	private							float		m_rotationY				= 0.0f;		//回転角度

	private							bool		m_mouseLockFlag			= true;		//マウスをロックする機能



	///	<summary>
	///	初期化時
	///	</summary>
	private		void	Awake() {
		//最初の角度を記憶
		Vector3	vecOriginalRot		= transform.rotation.eulerAngles;
		m_rotationX		= vecOriginalRot.x;
		m_rotationY		= vecOriginalRot.y;
	}
	///	<summary>
	///	毎フレーム呼び出される関数
	///	</summary>
	private		void	Update() {
		CheckMouseLock();
		CheckMove();
	}


	///	<summary>
	///	マウスロック処理のチェック
	///	</summary>
	private		void	CheckMouseLock() {

		//Escキーをおした時の動作
		if( Input.GetKeyDown( KeyCode.Escape)) {
			//フラグをひっくり返す
			m_mouseLockFlag	= !m_mouseLockFlag;
		}

		//マウスロックされてる？
		if( m_mouseLockFlag) {
			//ロックしていたらロック解除
			Cursor.lockState	= CursorLockMode.None;
			Cursor.visible		= false;
		} else {
			//ロック解除されていたらロック
			Cursor.lockState	= CursorLockMode.Locked;
			Cursor.visible		= true;
		}
	}
	///	<summary>
	///	移動処理のチェック
	///	</summary>
	private		void	CheckMove() {

		//回転
		{
			//キー操作による回転
			float	addRotationX	= 0.0f;
			float	addRotationY	= 0.0f;

			//マウスの移動量による回転
			if( m_mouseLockFlag) {
				//移動量を取得して角度に渡す
				addRotationX		+= (Input.GetAxis( "Mouse Y")	*ROTATION_X_MOUSE);		//2DのXYの移動量を3DのY軸とX軸に入れる
				addRotationY		+= (Input.GetAxis( "Mouse X")	*ROTATION_Y_MOUSE);
			}

			//現在の角度に加算
			m_rotationX			+= (addRotationX	*Time.deltaTime);
			m_rotationY			+= (addRotationY	*Time.deltaTime);

			//オイラー角で入れる
			transform.rotation	= Quaternion.Euler( m_rotationX, m_rotationY, 0);
		}
	}




}
