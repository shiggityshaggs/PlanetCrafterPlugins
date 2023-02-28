using System.Collections;
using UnityEngine;

namespace SSPCP_PurgeWhenFull.Classes
{
    internal class Purger
    {
        internal static IEnumerator Coroutine()
        {
            UnityEngine.Debug.Log(Application.dataPath);
            for (;;)
            {
                Destructors.Purge();
                Recyclers.Purge();
                yield return new WaitForSeconds(1f);
            }
        }
    }
}