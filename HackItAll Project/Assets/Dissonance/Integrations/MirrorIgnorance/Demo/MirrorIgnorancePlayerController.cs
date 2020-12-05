using Mirror;
using UnityEngine;

namespace Dissonance.Integrations.MirrorIgnorance.Demo
{
    public class MirrorIgnorancePlayerController : NetworkBehaviour
    {
        Vector3 offset;
        public float speedCoef, rotationCoef;
        string name;
        float lastCount;

        public void Start()
        {
            offset = GameObject.Find("Main Camera").GetComponent<Camera>().transform.position - this.GetComponent<Transform>().position;
            lastCount = Time.time;

        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            if (Time.time - lastCount > 5000)
            {
                lastCount = Time.time;
                CmdTransform(transform);
                Debug.Log("Sync");
            }

            var rotation = Input.GetAxis("Horizontal") * Time.deltaTime * 175;
            var speed = Input.GetAxis("Vertical") * 6;

            GameObject.Find("Main Camera").GetComponent<Camera>().transform.position = this.GetComponent<Transform>().position + offset;

            CmdMove(rotation, speed);
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