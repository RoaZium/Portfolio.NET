using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.Core.Log;
using DynamicMonitoring.Model;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace DynamicMonitoring.Network
{
    internal static class WebRequests
    {
        public static void DeleteSystemUseCheck(this CoPlatformWebClient client, SystemUseCheckModel systemUseCheckModel)
        {
            try
            {
                Logger.Log(LogLevels.I, $"Service: DeleteSystemUseCheck({systemUseCheckModel})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Delete($"DeleteSystemUseCheckByLogin?{"account=" + systemUseCheckModel.Account + "&" + "clientName=" + systemUseCheckModel.ClientName}");

                string result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
            }
        }
    }
}
