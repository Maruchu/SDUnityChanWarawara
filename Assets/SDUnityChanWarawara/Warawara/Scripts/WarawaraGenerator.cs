//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
//
//	適当にプレハブをわらわら生成するスクリプト
//
//	Copyright(C)2016 Maruchu
//	http://maruchu.nobody.jp/
//
//	=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
using UnityEngine;
using System.Collections;




/*
	<summary>
	適当にプレハブをわらわら生成するクラス
	</summary>
 */
public		class		WarawaraGenerator			: MonoBehaviour {



	//作成するGameObject
	public		GameObject[]	m_prefabObject				= new GameObject[ 1];					//対象のオブジェクト(複数指定したらランダムに選ばれる)


	//配置するための情報
	public		int				m_prefab_Count				= 64;									//対象を作成する全体の数

	public		Vector3			m_prefab_Pos_Min			= new Vector3(  -20,    0,  -20);		//対象を作成する位置 最小(メートル)
	public		Vector3			m_prefab_Pos_Max			= new Vector3(   20,    0,   20);		//対象を作成する位置 最大(メートル)

	public		Vector3			m_prefab_Rot_Min			= new Vector3(    0, -180,    0);		//対象を作成する角度 最小(-360～360度)
	public		Vector3			m_prefab_Rot_Max			= new Vector3(    0,  180,    0);		//対象を作成する角度 最大(-360～360度)

	public		float			m_prefab_Scale_Min			= 1f;									//対象を作成するスケール 最小
	public		float			m_prefab_Scale_Max			= 2f;									//対象を作成するスケール 最大



	/*
		<summary>
		初期化時
		</summary>
	 */
	private	void Start() {

		//対象作成
		int	i;
		for( i=0; i<m_prefab_Count; i++) {
			//指定された個数の対象を作成
			CreateObject_m_prefab();
		}
	}



	/*
		<summary>
		対象を作成
		</summary>
	 */
	private	void CreateObject_m_prefab() {
		//無ければ無視
		if( (null==m_prefabObject) || (m_prefabObject.Length <= 0)) {
			//無視
			return;
		}

		//作成するオブジェクトの番号をランダムに選ぶ
		int	index	= Random.Range( 0, m_prefabObject.Length);
		//配列の中身はnull？
		if( null==m_prefabObject[ index]) {
			//無視
			return;
		}

		//作成する場所
		Vector3	pos		= new Vector3(
							Random.Range( m_prefab_Pos_Min.x, m_prefab_Pos_Max.x),
							Random.Range( m_prefab_Pos_Min.y, m_prefab_Pos_Max.y),
							Random.Range( m_prefab_Pos_Min.z, m_prefab_Pos_Max.z));
		//作成する時の角度(-360～360度で指定)
		Vector3	rot		= new Vector3(
							Random.Range( m_prefab_Rot_Min.x, m_prefab_Rot_Max.x),
							Random.Range( m_prefab_Rot_Min.y, m_prefab_Rot_Max.y),
							Random.Range( m_prefab_Rot_Min.z, m_prefab_Rot_Max.z));
		//ローカルスケール
		float	scale	= Random.Range( m_prefab_Scale_Min, m_prefab_Scale_Max);

		//対象を作成
		GameObject	temp			= Instantiate( m_prefabObject[ index], pos, Quaternion.Euler( rot))		as GameObject;
		//スケールを入れる
		temp.transform.localScale	= (Vector3.one *scale);
		//自分の子供にする
		temp.transform.parent		= transform;
	}



}
