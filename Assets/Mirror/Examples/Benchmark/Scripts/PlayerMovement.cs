using UnityEngine;

namespace Mirror.Examples.Benchmark
{
    public class PlayerMovement : NetworkBehaviour
    {
	public CharacterController controller;
        public float speed = 12;

        void Update()
        {
            if (!isLocalPlayer) return;

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move*speed*Time.deltaTime);
    	}
	}
}
