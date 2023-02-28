using System.Collections;
using UnityEngine;

namespace PurgeWhenFull
{
    internal class Purger
    {
        internal static IEnumerator Coroutine()
        {
            for (;;)
            {
                Destructors.Purge();
                Recyclers.Purge();
                yield return new WaitForSeconds(2f);
            }
        }
    }
}