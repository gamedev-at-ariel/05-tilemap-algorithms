using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
[RequireComponent(typeof(SnapToGrid))]
public class KeyboardMoverByTile: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;

    SnapToGrid snapToGrid;
    private void Awake() {
        snapToGrid = GetComponent<SnapToGrid>();
    }

    void Update() {
        if (moveAction.WasPerformedThisFrame()) {
            Vector3 movement = moveAction.ReadValue<Vector2>(); // Implicitly convert Vector2 to Vector3, setting z=0.
            Vector3 newLogicalPosition = snapToGrid.LogicalPosition + movement;
            Vector3Int cellPosition = tilemap.WorldToCell(newLogicalPosition);
            TileBase tileOnNewPosition = tilemap.GetTile(cellPosition);
            if (allowedTiles.Contains(tileOnNewPosition)) {
                snapToGrid.LogicalPosition = newLogicalPosition;
            } else {
                Debug.LogWarning("You cannot walk on " + tileOnNewPosition + "!");
            }

            if (TryGetComponent(out TargetMover targetMover)) {
                targetMover.SetNoTarget();  // Stop automatic path-finding, as the player moves manually
            }
        }
    }
}
