using Mirror;
using UnityEngine;

namespace Dissonance.Integrations.MirrorIgnorance.Demo
{
    public class MirrorIgnorancePlayerController : NetworkBehaviour
    {
        Vector3 offset;
        public float speedCoef, rotationCoef; 


        public void Start()
        {
            offset = GameObject.Find("Main Camera").GetComponent<Camera>().transform.position - this.GetComponent<Transform>().position;
        }

        private void Update()
        {
            if (!isLocalPlayer)
            {
                return;
            }

            

            var rotation = Input.GetAxis("Horizontal") * Time.deltaTime * 175;
            var speed = Input.GetAxis("Vertical") * 6;

            GameObject.Find("Main Camera").GetComponent<Camera>().transform.position = this.GetComponent<Transform>().position + offset;

            CmdMove(rotation, speed);
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