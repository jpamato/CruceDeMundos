using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelObjectsGen : MonoBehaviour {

	public bool editing;
	public GameObject edit;

	public List<LevelObstacle> levelObstacles;
	public List<LevelBlock> levelBlocks;
	public List<LevelCharge> levelCharges;
	public List<LevelCharacter> levelCharacters;

	[Serializable]
	public class LevelObject{
		public GameObject objectPrefab;
		public List<Transform> transform;
	}

	[Serializable]
	public class LevelObstacle{
		public GameObject objectPrefab;
		public List<ObstacleDetails> obstacleDetails;
		[Serializable]
		public class ObstacleDetails{
			public int level;
			public Transform transform;
		}
	}

	[Serializable]
	public class LevelBlock{
		public GameObject objectPrefab;
		public List<BlockDetails> blockDetails;
		[Serializable]
		public class BlockDetails{			
			public BlockTypes blockType;
			public enum BlockTypes
			{
				block1,
				block2
			}
			public Transform transform;
		}
	}

	[Serializable]
	public class LevelCharge{
		public GameObject objectPrefab;
		public List<ChargeDetails> chargeDetails;
		[Serializable]
		public class ChargeDetails{			
			public CollectableItem.CollectableType type;
			public int val;
			public Transform transform;
		}
	}

	[Serializable]
	public class LevelCharacter{
		public GameObject objectPrefab;
		public Transform transform;
	}

	void Start(){
		if (editing) {
			edit.SetActive (true);
		} else {
			edit.SetActive (false);
			SetLevelObstacles ();
			SetLevelBlocks ();
			SetLevelCharges ();
			SetLevelCharacters ();
		}
	}

	void SetLevelObstacles(){
		foreach (LevelObstacle levelOb in levelObstacles) {
			foreach (LevelObstacle.ObstacleDetails oDet in levelOb.obstacleDetails) {
				GameObject go = Instantiate (levelOb.objectPrefab, transform) as GameObject;
				go.transform.position = oDet.transform.position;
				ObstacleData oData = go.GetComponent<ObstacleData> ();
				oData.CurrentLevel = oData.levels [oDet.level-1];
			}
		}
	}

	void SetLevelBlocks(){
		foreach (LevelBlock levelBlock in levelBlocks) {
			foreach (LevelBlock.BlockDetails bDet in levelBlock.blockDetails) {
				GameObject go = Instantiate (levelBlock.objectPrefab, transform) as GameObject;
				go.transform.position = bDet.transform.position;
				go.transform.localScale = bDet.transform.lossyScale;
				string[] names = Enum.GetNames(typeof(LevelBlock.BlockDetails.BlockTypes));
				for( int i = 0; i < names.Length; i++ )
				{
					if (names [i] == bDet.blockType.ToString()) {
						go.transform.Find (names [i]).gameObject.SetActive (true);
					} else {
						go.transform.Find (names [i]).gameObject.SetActive (false);
					}
				}
			}
		}
	}

	void SetLevelCharges(){
		foreach (LevelCharge levelCh in levelCharges) {
			foreach (LevelCharge.ChargeDetails chDet in levelCh.chargeDetails) {
				GameObject go = Instantiate (levelCh.objectPrefab, transform) as GameObject;
				go.transform.position = chDet.transform.position;
				CollectableItem ci = go.GetComponent<CollectableItem> ();
				ci.val = chDet.val;
				ci.itemType = chDet.type;
			}
		}
	}

	void SetLevelCharacters(){
		foreach (LevelCharacter levelCh in levelCharacters) {
			GameObject go = Instantiate (levelCh.objectPrefab, transform) as GameObject;
			go.transform.position = levelCh.transform.position;
		}
	}
}
