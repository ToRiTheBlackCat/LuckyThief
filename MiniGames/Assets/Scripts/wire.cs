using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class wire : MonoBehaviour
    {
        Vector3 startPoint;
        //Vector3 startPosition;
        public SpriteRenderer wireEnd;
        public GameObject lightOn;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            startPoint = transform.position;
            //startPosition = transform.position;
            if (wireEnd == null || lightOn == null)
            {
                Debug.LogError("wireEnd hoặc lightOn chưa được gán trong Inspector!");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseDrag()
        {
            Camera minigameCamera = GameObject.FindWithTag("MinigameCamera").GetComponent<Camera>();
            Vector3 newPosition = minigameCamera.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;

            //check for nearby connection point
            Collider2D[] colliders = Physics2D.OverlapCircleAll(newPosition, 2f);
            foreach (Collider2D collider in colliders)
            {
                //make sure not my own collider
                if (collider.gameObject != gameObject)
                {
                    UpdateWire(collider.transform.position);

                    if (transform.parent.name.Equals(collider.transform.parent.name))
                    {
                        Main.Instance.SwitchChange(1);
                        collider.GetComponent<wire>()?.Done();
                        Done();
                    }

                    return;
                }
            }

            UpdateWire(newPosition);
        }

        private void OnMouseUp()
        {
            UpdateWire(startPoint);
        }

        void Done()
        {
            if (lightOn != null)
            {
                lightOn.SetActive(true);
            }
            Destroy(this);
        }

        void UpdateWire(Vector3 newPosition)
        {
            //update poition
            transform.position = newPosition;

            //update direction
            Vector3 direction = newPosition - startPoint;
            transform.right = direction * transform.lossyScale.x;

            //update scale
            if (wireEnd != null)
            {
                float dist = Vector2.Distance(startPoint, newPosition);
                wireEnd.size = new Vector2(dist, wireEnd.size.y);
            }
        }
    }
}
