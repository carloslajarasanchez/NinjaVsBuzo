using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Frog-2", new Vector3(-2.5f, -.5f, 0f), Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Frog-3", new Vector3(2.5f, -.5f, 0f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
