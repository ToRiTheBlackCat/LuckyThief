using System.Collections;
using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class MegaBotLazer : MonoBehaviour
    {
        private Vector3 movementDirection;
        private LineRenderer lineRenderer;
        private float laserDuration = 1.5f; // Thời gian tia laze tồn tại
        private float laserRange = 15f;    // Tầm bắn của tia laze
        private Transform laserOrigin;

        public void Initialize(Transform origin, float range, float duration)
        {
            laserOrigin = origin;
            laserRange = range;
            laserDuration = duration;
            lineRenderer = GetComponent<LineRenderer>();

            StartCoroutine(FireLaser());
        }

        private IEnumerator FireLaser()
        {
            float startTime = Time.time;

            if (movementDirection == Vector3.zero)
            {
                yield break;
            }
            else
            {
                // Tính toán góc quay để laser hướng đúng hướng
                float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);

                // Xác định điểm kết thúc của laser theo hướng đã truyền từ MegaBot
                Vector3 endPosition = laserOrigin.position + movementDirection * laserRange;


                // Cập nhật LineRenderer để vẽ laser
                lineRenderer.SetPosition(0, laserOrigin.position);
                lineRenderer.SetPosition(1, endPosition);
            }

            Destroy(gameObject, 2f); // Hủy laser sau khi kết thúc
        }
        public void SetMovementDirection(Vector3 direction)
        {
            movementDirection = direction;
        }
    }
}
