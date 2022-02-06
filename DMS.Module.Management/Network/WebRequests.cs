using Coever.Lib.CoPlatform.Core.Models;
using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using Coever.Lib.Core.Log;
using DMS.Module.Management.Infrastructure;
using DMS.Module.Management.Model;
using DMS.Module.Management.Model.RestAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using DmComponentMst = DMS.Module.Management.Model.RestAPI.DmComponentMst;

namespace DMS.Module.Management.Network
{
    internal static class WebRequests
    {
        #region IP 카메라

        public static List<DmIpcamMst> GetIPCams(this CoPlatformWebClient client, uint? code = null)
        {
            return Coever.Lib.CoPlatform.IPCam.Core.Utils.WebUtils.GetIPCams(client, code);
        }

        public static DmIpcamFull GetIPCamFull(this CoPlatformWebClient client, uint code)
        {
            return Coever.Lib.CoPlatform.IPCam.Core.Utils.WebUtils.GetIPCamFull(client, code);
        }

        #endregion

        #region 시나리오

        public static List<RecordAlarmModel> GetRecordingAlarmAPI(this CoPlatformWebClient client, uint code, DateTime startTime, DateTime endTime)
        {
            try
            {
                Logger.Log(LogLevels.I, $"Service: RecordingAlarm");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get($"RecordingAlarm?ipcamCode={code}&startTime={startTime}&endTime={endTime}");

                string result = response.Content.ReadAsStringAsync().Result;

                List<RecordAlarmModel> resultList = JsonConvert.DeserializeObject<List<RecordAlarmModel>>(result);

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        public static List<ScenarioModel> GetScenarioList(this CoPlatformWebClient client, string type)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.GetScenarioList(client, type);
        }

        public static ScenarioModel GetScenarioInfo(this CoPlatformWebClient client, int code)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.GetScenarioInfo(client, code);
        }

        public static ScenarioModel AddScenario(this CoPlatformWebClient client, ScenarioModel scenario)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.AddScenario(client, scenario);
        }

        public static bool RemoveScenario(this CoPlatformWebClient client, int code)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.RemoveScenario(client, code);
        }

        public static bool SaveScenarioProperties(this CoPlatformWebClient client, ScenarioModel scenario)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.SaveScenarioProperties(client, scenario);
        }

        public static bool SwapScenarioPosition(this CoPlatformWebClient client, int firstOneId, int secondOneId)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.SwapScenarioPosition(client, firstOneId, secondOneId);
        }

        public static IList<CompSelectionModel> GetIPCamCompoSelectionList(this CoPlatformWebClient client, string scenarioType)
        {
            List<CompSelectionModel> result = new List<CompSelectionModel>();

            try
            {
                Model.RestAPI.DmComponentMst dmComponentMst = new Model.RestAPI.DmComponentMst();

                if (scenarioType == "PMS")
                {
                    dmComponentMst.MapType = 1;
                }
                else
                {
                    dmComponentMst.MapType = 2;
                }

                var resultAPI = CoPlatformWebClient.Instance.GetComponentMst(dmComponentMst);

                var resultAPI1 = resultAPI;
                var resultAPI2 = resultAPI;

                var response = (from A in resultAPI1
                                join B in resultAPI2 on A.PRoutingCode equals B.RoutingCode
                                where A.RoutingType is "DM701003"
                                select new CompSelectionModel()
                                {
                                    Key = A.RoutingCode,
                                    DisplayName = B.RoutingName,
                                    Value = A.ComponentProperty
                                }).ToList<CompSelectionModel>();


                var ipcams = client.GetIPCams();

                result.AddRange(response.ConvertAll<CompSelectionModel>(item =>
                {
                    string compName = "";
                    uint? targetCode = null;
                    try
                    {
                        var property = JObject.Parse(item.Value.ToString());
                        targetCode = uint.Parse(property["TargetCode"].ToString());
                        compName = ipcams.Find(cam => cam.Code == targetCode).Name;
                    }
                    catch { }

                    if (targetCode != null)
                    {
                        return new CompSelectionModel()
                        {
                            Key = item.Key,
                            Value = targetCode,
                            CompName = string.IsNullOrEmpty(compName) ? "Nonamed" : compName,
                            LayoutName = item.DisplayName,
                        };
                    }
                    else
                    {
                        return null;
                    }
                }));
                result.RemoveAll(item => item == null);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message);
            }
            return result;
        }

        #endregion

        #region 알람 시나리오
        public static List<AlarmScenarioModel> GetAlarmScenarioList(this CoPlatformWebClient client)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.GetAlarmScenarioList(client);
        }

        public static AlarmScenarioModel GetAlarmScenarioInfo(this CoPlatformWebClient client, int code)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.GetAlarmScenarioInfo(client, code);
        }

        public static AlarmScenarioModel AddAlarmScenario(this CoPlatformWebClient client, AlarmScenarioModel scenario)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.AddAlarmScenario(client, scenario);
        }

        public static bool RemoveAlarmScenario(this CoPlatformWebClient client, int code)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.RemoveAlarmScenario(client, code);
        }

        public static bool SaveAlarmScenarioProperties(this CoPlatformWebClient client, AlarmScenarioModel scenario)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.SaveAlarmScenarioProperties(client, scenario);
        }

        public static bool SwapAlarmScenarioPosition(this CoPlatformWebClient client, int firstOneId, int secondOneId)
        {
            return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.SwapAlarmScenarioPosition(client, firstOneId, secondOneId);
        }

        public static IList<TargetModel> GetAlarmTargetList(this CoPlatformWebClient client)
        {
            List<TargetModel> result = new List<TargetModel>();

            try
            {
                var resultAPI = CoPlatformWebClient.Instance.GetComponentMst(null);

                if(resultAPI == null)
                {
                    return result;
                }

                var resultLinq = resultAPI.Where(p => p.RoutingType == "DM601005");

                foreach (Model.RestAPI.DmComponentMst item in resultLinq)
                {
                    result.Add(new TargetModel()
                    {
                        TargetCode = item.RoutingCode,
                        TargetName = item.RoutingName,
                        TargetType = item.RoutingType
                    });
                }
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message);
            }
            return result;
        }

        /// <summary>
        /// Get MES Layout Selections
        /// </summary>
        /// <returns></returns>
        public static IList<SelectionModel> GetMESLayoutSelectionList(this CoPlatformWebClient client)
        {
            List<SelectionModel> result = new List<SelectionModel>();
            Model.RestAPI.DmComponentMst dmComponentMst = new Model.RestAPI.DmComponentMst()
            {
                MapType = 1
            };

            try
            {
                var resultAPI = CoPlatformWebClient.Instance.GetComponentMst(dmComponentMst);

                result.AddRange(resultAPI.Where(p =>
                {
                    return
                    !(p.RoutingType == "DM701001"
                    || p.RoutingType == "DM701002"
                    || p.RoutingType == "DM701003"
                    || p.RoutingType == "DM701004"
                    || p.RoutingType == "DM701005"
                    || p.RoutingType == "DM701010"
                    || p.RoutingType == "DM801001"
                    || p.RoutingType == "DM801002");
                }).ToList().ConvertAll<SelectionModel>(row =>
                {
                    return new SelectionModel()
                    {
                        Key = row.RoutingCode.ToString(),
                        Value = row.RoutingName.ToString(),
                        DisplayName = row.RoutingName.ToString()
                    };
                }));
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message);
            }
            return result;
        }

        #endregion

        #region 테이블: dm_component_mst(공정 구성 항목)

        public static List<DmComponentMst> GetComponentMst(this CoPlatformWebClient client, DmComponentMst componentMst = null)
        {
            try
            {
                Logger.Log(LogLevels.I, $"Service: DmComponentMst({ componentMst})");

                string param = null;

                if (componentMst != null)
                {
                    param = "mapType=" + componentMst.MapType;
                }

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get($"DmComponentMst?{param}");

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmComponentMst> resultList = JsonConvert.DeserializeObject<List<DmComponentMst>>(result);

                foreach (DmComponentMst item in resultList)
                {
                    if (item.ComponentProperty != null)
                    {
                        item.PropertyM = JsonConvert.DeserializeObject<DmComponentProperty>(item.ComponentProperty);
                    }
                }

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        #endregion

        public static List<DmRoutingConfigurationStatusModel> GetRoutingConfigurationStatus(this CoPlatformWebClient client, Model.RestAPI.DmComponentMst componentMst = null)
        {
            try
            {
                Logger.Log(LogLevels.I, $"Service: RoutingConfigurationStatus()");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_RoutingConfigurationStatus);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmRoutingConfigurationStatusModel> resultList = JsonConvert.DeserializeObject<List<DmRoutingConfigurationStatusModel>>(result);

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        public static List<DmBaseInfoModel> GetBaseInfo(this CoPlatformWebClient client, string type)
        {
            try
            {
                Logger.Log(LogLevels.I, $"Service: BaseInfo({type})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_BaseInfo + type);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmBaseInfoModel> resultList = JsonConvert.DeserializeObject<List<DmBaseInfoModel>>(result);

                if (resultList == null)
                {
                    return null;
                }

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message);
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }
    }
}
