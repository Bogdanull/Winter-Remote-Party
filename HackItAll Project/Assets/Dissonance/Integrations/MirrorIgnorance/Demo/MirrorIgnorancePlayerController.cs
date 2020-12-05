using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace Dissonance.Integrations.MirrorIgnorance.Demo
{
    public class MirrorIgnorancePlayerController : NetworkBehaviour
    {
        Vector3 offset;
        float offsetNorm;
        public float speedCoef, rotationCoef;

        public Material m_a, m_b, m_c, m_d, m_e;
        public Sprite s_a, s_b, s_c, s_d;

        bool isPressed;

        [SyncVar]
        public string namePlayer;

        public MeshRenderer meshRenderer;
        public GameObject meshRendererGO;

        public SpriteRenderer spriteRenderer;
        public GameObject spriteRendererGO;

        float lastCount;
        public InputField input;
        public GameObject inputGO;


        public void Start()
        {
            offset = GameObject.Find("Main Camera").GetComponent<Camera>().transform.position - this.GetComponent<Transform>().position;
            offsetNorm = offset.magnitude;
            lastCount = Time.time;
            if (!isLocalPlayer)
            {
                DestroyImmediate(inputGO);
            }
            if (!isLocalPlayer)
            {
                DestroyImmediate(meshRendererGO);
            }
            if (!isLocalPlayer)
            {
                DestroyImmediate(spriteRendererGO);
            }
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            Camera main = GameObject.Find("Main Camera").GetComponent<Camera>();

            //GameObject.Find("Main Camera").GetComponent<Camera>().transform.position = this.transform.position + offset;
            main.transform.LookAt(this.transform);


            if (Input.GetMouseButton(1))
            {
                float x = Input.GetAxis("Mouse X");
                float y = -Input.GetAxis("Mouse Y");
                float zoom = Input.GetAxis("Mouse ScrollWheel");

                main.transform.RotateAround(this.transform.position, main.transform.up, x * 3);
                if ((offset.y > 0.1 * offsetNorm || y > 0) && (offset.y < 0.9 * offsetNorm || y < 0))
                {
                    main.transform.RotateAround(this.transform.position, main.transform.right, y * 3);
                }
                if (offsetNorm >= 6 && zoom < 0 || zoom > 0 && offsetNorm <= 20)
                {
                    offsetNorm += zoom * 6;
                }
                offset = offsetNorm * (GameObject.Find("Main Camera").GetComponent<Camera>().transform.position - this.GetComponent<Transform>().position).normalized;
            }

            main.transform.position = this.transform.position + offset;



            if (Time.time - lastCount > 2000)
            {
                lastCount = Time.time;
                CmdTransform(transform);
                Debug.Log("Sync");
            }

            var rotation = Input.GetAxis("Horizontal") * Time.deltaTime * 175;
            var speed = Input.GetAxis("Vertical") * 6;

            CmdMove(rotation, speed);
        }

        [Client]
        public void SetString()
        {
            string s = input.text;
            if (isLocalPlayer)
            {
                CmdSetString(s);
            }
        }
    
        [Command]
        private void CmdSetString(string s)
        {
            RpcSetString(s);
        }

        [ClientRpc]
        private void RpcSetString(string s)
        {
            namePlayer = s;
        }

        [Client]
        public void SetSmile()
        {
            int s = Random.Range(0, 4);
            if (isLocalPlayer)
            {
                CmdSetSmile(s);
            }
        }

        [Command]
        private void CmdSetSmile(int s)
        {
            RpcSetSmile(s);
        }

        [ClientRpc]
        private void RpcSetSmile(int s)
        {
            if (s < 1)
            {
                spriteRenderer.sprite = s_a;
            }
            else if (s < 2)
            {
                spriteRenderer.sprite = s_b;
            }
            else if (s < 3)
            {
                spriteRenderer.sprite = s_c;
            }
            else
            {
                spriteRenderer.sprite = s_d;
            }

        }


        [Client]
        public void SetColor()
        {
            int s = Random.Range(0, 5);
            if (isLocalPlayer)
            {
                CmdSetColor(s);
            }
        }

        [Command]
        private void CmdSetColor(int s)
        {
            RpcSetColor(s);
        }

        [ClientRpc]
        private void RpcSetColor(int s)
        {
            if (s < 1)
            {
                meshRenderer.material = m_a;
            }
            else if (s < 2)
            {
                meshRenderer.material = m_b;
            }
            else if (s < 3)
            {
                meshRenderer.material = m_c;
            }
            else if (s < 4)
            {
                meshRenderer.material = m_d;
            }
            else
            {
                meshRenderer.material = m_e;
            }

        }

        [Command]
        private void CmdTransform(Transform trans)
        {
            RpcTransform(trans);
        }

        [ClientRpc]
        private void RpcTransform(Transform trans)
        {
            transform.position = trans.position;
            transform.rotation = trans.rotation;
        }

        [Command]
        private void CmdMove(float rotation, float speed)
        {
            RpcMove(rotation, speed);
        }

        [ClientRpc]
        private void RpcMove(float rotation, float speed)
        {
            transform.Rotate(0, rotation, 0);
            var forward = transform.TransformDirection(Vector3.forward);
            GetComponent<CharacterController>().SimpleMove(forward * speed);

            if (transform.position.y < -3)
            {
                transform.position = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }
        }

    }
}