using NUnit.Framework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Labyrinth.Level {
    public class LevelSetup : MonoBehaviour {

        [Header("Debug")]
        [SerializeField]
        Tilemap[] tilemaps;

        [SerializeField]
        Transform playerStart;

        public Vector2 playerStartPos => playerStart.position;

        protected void OnValidate() {
            if (!playerStart) {
                foreach (Transform child in transform) {
                    if (child.CompareTag("PlayerStart")) {
                        playerStart = child;
                        break;
                    }
                }
            }
            Assert.True(playerStart, "Player Start is missing!");
        }

        protected void OnEnable() {
            tilemaps = GetComponentsInChildren<Tilemap>();
        }

        public void StartLevel() {
            foreach (var tilemap in tilemaps) {
                tilemap.enabled = true;
                tilemap.transform.GetComponent<TilemapRenderer>().enabled = true;
                tilemap.transform.GetComponent<TilemapCollider2D>().enabled = true;
            }
        }

        public void HideLevel() {
            foreach (var tilemap in tilemaps) {
                tilemap.enabled = false;
                tilemap.transform.GetComponent<TilemapRenderer>().enabled = false;
                tilemap.transform.GetComponent<TilemapCollider2D>().enabled = false;
            }
        }
    }
}