using System.Collections.Generic;
using UnityEngine;

public class CutscenePrefabs : MonoBehaviour {

    [System.Serializable]
    public class CutscenePrefab
    {
        public GameObject gameObject;
        public string id;

        public CutscenePrefab(string id, GameObject gameObject)
        {
            this.id = id;
            this.gameObject = gameObject;
        }
    }

    public CutscenePrefab[] prefabs;

    public GameObject GetPrefab(string id) {
        foreach (var prefab in prefabs) {
            if (prefab.id == id)
                return prefab.gameObject;
        }
        return null;
    }
}