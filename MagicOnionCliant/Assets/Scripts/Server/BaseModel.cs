using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseModel : MonoBehaviour
{
#if DEBUG
    public const string ServerURL = "http://localhost:7000";
#else
    public const string ServerURL = "http://realtime-game.japaneast.cloudapp.azure.com:7000";
#endif
}
