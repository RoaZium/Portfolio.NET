using Coever.Lib.CoPlatform.Core.Network;
using Coever.Lib.CoPlatform.Scenario.Core;
using Coever.Lib.Core.Log;
using DMS.Module.Map.Infrastructure;
using DMS.Module.Map.Model;
using DMS.Module.Map.Model.RestAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrismWPF.Common;
using PrismWPF.Common.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Module.Map.Network
{
    internal static class WebRequests
    {
        #region 공정 구성

        /// <summary>
        /// 테이블: routing_configuration
        /// API: RoutingConfiguration/NonHierarchy
        /// 공정 구성 항목 조회
        /// 기타: 조회만 가능, 조건은 useYN = Y 사용중인것만 조회
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static List<DmRoutingConfigurationModel> GetRoutingConfiguration(this CoPlatformWebClient client)
        {
            try
            {
                Logger.Log(LogLevels.I, $"Service: DmRoutingConfigurationModel()");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_RoutingConfiguration);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmRoutingConfigurationModel> resultList = JsonConvert.DeserializeObject<List<DmRoutingConfigurationModel>>(result);

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        #endregion

        #region IP 카메라

        public static List<Coever.Lib.CoPlatform.Core.Models.DmIpcamMst> GetIPCams(this CoPlatformWebClient client, uint? code = null)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return Coever.Lib.CoPlatform.IPCam.Core.Utils.WebUtils.GetIPCams(client, code);
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return null;
            }
        }

        public static Coever.Lib.CoPlatform.Core.Models.DmIpcamFull GetIPCamFull(this CoPlatformWebClient client, uint code)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return Coever.Lib.CoPlatform.IPCam.Core.Utils.WebUtils.GetIPCamFull(client, code);
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return null;
            }
        }

        #endregion

        #region 모니터링 시나리오

        public static List<ScenarioModel> GetScenarioList(this CoPlatformWebClient client, string type)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.GetScenarioList(client, type);
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return null;
            }
        }

        public static ScenarioModel GetScenarioInfo(this CoPlatformWebClient client, int code)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.GetScenarioInfo(client, code);
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return null;
            }
        }

        #endregion

        #region 알람 시나리오

        public static List<AlarmMonitoringTargetModel> GetAlarmMonitoringTargets(this CoPlatformWebClient client)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            List<AlarmMonitoringTargetModel> alarmMonitoringTargets = new List<AlarmMonitoringTargetModel>();

            try
            {
                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_AlarmMonitoringTargets);

                string result = response.Content.ReadAsStringAsync().Result;

                List<AlarmMonitoringTargetModel> resultList = JsonConvert.DeserializeObject<List<AlarmMonitoringTargetModel>>(result);

                foreach (AlarmMonitoringTargetModel item in resultList)
                {
                    item.Actions = new List<IActionModel>();

                    AlarmScenarioModel scenario = client.GetAlarmScenarioInfo(item.ScenarioCode.Value);

                    if (scenario != null && scenario.ScenarioActions != null)
                    {
                        item.Actions = scenario.ScenarioActions.ConvertAll<IActionModel>(action =>
                        {
                            switch (action.ActionType.ToString())
                            {
                                case nameof(ActionTypes.layout):
                                    {
                                        return action.CopyObject<LayoutActionModel>();
                                    }
                                case nameof(ActionTypes.ipcam_compo):
                                    {
                                        return action.CopyObject<IPCamCompoActionModel>();
                                    }
                                case nameof(ActionTypes.alarm_layout):
                                    {
                                        return action.CopyObject<AlarmLayoutActionModel>();
                                    }
                                default:
                                    {
                                        return action.CopyObject<InvalidActionModel>();
                                    }
                            }
                        });
                    }

                    alarmMonitoringTargets.Add(item);
                }
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
            }

            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
            return alarmMonitoringTargets;
        }

        public static AlarmScenarioModel GetAlarmScenarioInfo(this CoPlatformWebClient client, int code)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                return Coever.Lib.CoPlatform.Scenario.Core.Utils.WebUtils.GetAlarmScenarioInfo(client, code);
            }
            catch (Exception e)
            {
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return null;
            }
        }

        #endregion

        #region 테이블: dm_component_mst(공정 구성 항목)

        public static List<DmComponentMst> GetComponentMst(this CoPlatformWebClient client, string dmComponentList)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: DmComponentMst()");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_DmComponentMst + dmComponentList);

                string result = response.Content.ReadAsStringAsync().Result;

                // 속성 명칭 변경 시 구버전 지원을 위한 클래스
                PropertyMappingConverter pmc = new PropertyMappingConverter();
                string jsonData = pmc.ConverterPropertyName(result);

                List<DmComponentMst> resultList = JsonConvert.DeserializeObject<List<DmComponentMst>>(jsonData);

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

        public static int PostComponentMst(this CoPlatformWebClient client, List<DmComponentMst> componentMst)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: DmComponentMst({componentMst})");

                foreach (DmComponentMst item in componentMst)
                {
                    item.ComponentProperty = JsonConvert.SerializeObject(item.PropertyM, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }

                string data = JsonConvert.SerializeObject(componentMst, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                StringContent httpContent = new StringContent(data, Encoding.UTF8, ConstantManager.StringContent_MediaType);

                HttpResponseMessage response = CoPlatformWebClient.Instance.Post(ConstantManager.APIKey_DmComponentMst, httpContent);

                int statusCode = (int)response.StatusCode;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return statusCode;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return 1;
            }
        }

        public static int PutComponentMst(this CoPlatformWebClient client, List<DmComponentMst> componentMst)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: DmComponentMst({componentMst})");

                foreach (DmComponentMst item in componentMst)
                {
                    item.ComponentProperty = JsonConvert.SerializeObject(item.PropertyM, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                }

                string data = JsonConvert.SerializeObject(componentMst, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                StringContent httpContent = new StringContent(data, Encoding.UTF8, ConstantManager.StringContent_MediaType);

                HttpResponseMessage response = CoPlatformWebClient.Instance.Put(ConstantManager.APIKey_DmComponentMst, httpContent);

                int statusCode = (int)response.StatusCode;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return statusCode;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return 1;
            }
        }

        public static int DeleteComponentMst(this CoPlatformWebClient client, List<DmComponentMst> componentMst)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: DmComponentMst({ componentMst})");

                string param = string.Empty;

                foreach (DmComponentMst data in componentMst)
                {
                    param += "routingCode=" + data.RoutingCode + "&";
                }

                HttpResponseMessage response = CoPlatformWebClient.Instance.Delete(ConstantManager.APIKey_DmComponentMst + param);

                int result = (int)response.StatusCode;

                return result;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return 0;
            }
        }

        #endregion

        #region 알람 상태 정보

        /// <summary>
        /// API 호출
        /// ResourceSensor만 조회됨
        /// </summary>
        /// <param name="client"></param>
        /// <param name="sensorCodeList"></param>
        /// <returns></returns>
        public static List<DmAlarmStatus> GetAlarmStatus(this CoPlatformWebClient client, string sensorCodeList)
        {
            try
            {
                Logger.Log(LogLevels.I, $"Service: DmAlarmStatusList({sensorCodeList})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_DmAlarmStatusList + sensorCodeList);

                string result = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<List<DmAlarmStatus>>(result);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// API: RoutingConfigutaionAlarmStatus
        /// 설명: 상위레벨 및 리소스 전체 알람 상태 조회
        /// </summary>
        /// <param name="client"></param>
        /// <param name="resourceSensorList"></param>
        /// <returns></returns>
        public static List<DmRoutingConfigurationAlarmStatusModel> GetRoutingConfigutaionAlarmStatus(this CoPlatformWebClient client, string resourceSensorList)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: RoutingConfigutaionAlarmStatus({ resourceSensorList})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_RoutingConfigutaionAlarmStatus + resourceSensorList);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmRoutingConfigurationAlarmStatusModel> resultList = JsonConvert.DeserializeObject<List<DmRoutingConfigurationAlarmStatusModel>>(result);

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        #endregion

        #region 테이블: resource_sensor(resourcesensor 정보)

        public static List<DmResourceSensorModel> GetResourceSensors(this CoPlatformWebClient client, string resourceSensors)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: ResourceSensor({resourceSensors})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_ResourceSensor + resourceSensors);

                string result = response.Content.ReadAsStringAsync().Result;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return JsonConvert.DeserializeObject<List<DmResourceSensorModel>>(result);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                return null;
            }
        }

        public static List<DmResourceSensorModel> PutResourceSensors(this CoPlatformWebClient client, List<DmResourceSensorModel> resourceSensors)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: ResourceSensorList({resourceSensors})");

                string data = JsonConvert.SerializeObject(resourceSensors, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                StringContent httpContent = new StringContent(data, Encoding.UTF8, ConstantManager.StringContent_MediaType);

                HttpResponseMessage response = CoPlatformWebClient.Instance.Put(ConstantManager.APIKey_ResourceSensor, httpContent);

                string result = response.Content.ReadAsStringAsync().Result;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return JsonConvert.DeserializeObject<List<DmResourceSensorModel>>(result);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                return null;
            }
        }

        #endregion

        #region 파일 업로드, 다운로드

        public static List<CustomFileManager> GetFileInfo(this CoPlatformWebClient client, string componentMst)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: File({ componentMst})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_File + componentMst);

                string result = response.Content.ReadAsStringAsync().Result;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return JsonConvert.DeserializeObject<List<CustomFileManager>>(result);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                return null;
            }
        }

        public static int PostFileInfo(this CoPlatformWebClient client, List<DmComponentMst> componentMst)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: File({componentMst})");

                MultipartFormDataContent multipartForm = new MultipartFormDataContent();

                foreach (DmComponentMst item in componentMst)
                {
                    if (item.RoutingType == ComponentType.DM701001.ToString())
                    {
                        multipartForm.Add(new ByteArrayContent(FileUtils.GetBytesFromFile(item.PropertyM.ImagePath)), "files", item.PropertyM.ImageName);
                        multipartForm.Add(new StringContent(item.PropertyM.ItemCode), "fileNames");
                    }
                }

                HttpResponseMessage response = CoPlatformWebClient.Instance.Post(ConstantManager.APIKey_File, multipartForm);

                int statusCode = (int)response.StatusCode;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return statusCode;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                return 1;
            }
        }

        public static int PutFileInfo(this CoPlatformWebClient client, List<DmComponentMst> componentMst)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: File({componentMst})");

                MultipartFormDataContent multipartForm = new MultipartFormDataContent();

                foreach (DmComponentMst item in componentMst)
                {
                    if (item.RoutingType == ComponentType.DM701001.ToString())
                    {
                        multipartForm.Add(new ByteArrayContent(FileUtils.GetBytesFromFile(item.PropertyM.ImagePath)), "files", item.PropertyM.ImageName);
                        multipartForm.Add(new StringContent(item.PropertyM.ItemCode), "fileNames");
                    }
                }

                HttpResponseMessage response = CoPlatformWebClient.Instance.Put(ConstantManager.APIKey_File, multipartForm);

                int statusCode = (int)response.StatusCode;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return statusCode;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return 1;
            }
        }

        public static int DeleteFileInfo(this CoPlatformWebClient client, List<DmComponentMst> componentMst)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: DmComponentMst({ componentMst})");

                string param = string.Empty;

                foreach (DmComponentMst item in componentMst)
                {
                    if (item.RoutingType == ComponentType.DM701001.ToString())
                    {
                        param += "fileNames=" + item.PropertyM.ItemCode + "&";
                    }
                }

                HttpResponseMessage response = CoPlatformWebClient.Instance.Delete(ConstantManager.APIKey_File + param);

                int statusCode = (int)response.StatusCode;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return statusCode;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return 1;
            }
        }

        #endregion

        #region 타겟, 항목

        public static List<DmBaseInfoTypeModel> GetBaseInfoType(this CoPlatformWebClient client, LanguageType languageType)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: BaseInfoType({ languageType})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_BaseInfoType + languageType);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmBaseInfoTypeModel> resultList = JsonConvert.DeserializeObject<List<DmBaseInfoTypeModel>>(result);

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return null;
            }
        }

        public static List<DmBaseInfoModel> GetBaseInfo(this CoPlatformWebClient client, string type)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: BaseInfo({type})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_BaseInfo + type);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmBaseInfoModel> resultList = JsonConvert.DeserializeObject<List<DmBaseInfoModel>>(result);

                if(resultList == null)
                {
                    return null;
                }

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, resultList.Count.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message);
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        #endregion

        #region 시스템 사용 여부 체크(업데이트 없음 -> 업데이트 하지 않고 바로 삭제)

        public static List<SystemUseCheckModel> GetSystemUseCheck(this CoPlatformWebClient client, SystemUseCheckModel systemUseCheckModel)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: GetSystemUseCheck()");

                string param = "account=" + systemUseCheckModel.Account + "&" + "windowName=" + systemUseCheckModel.WindowName + "&" + "clientName=" + systemUseCheckModel.ClientName;

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_SystemUseCheck + param);

                string result = response.Content.ReadAsStringAsync().Result;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return JsonConvert.DeserializeObject<List<SystemUseCheckModel>>(result);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return null;
            }
        }

        public static SystemUseCheckModel PostSystemUseCheck(this CoPlatformWebClient client, SystemUseCheckModel systemUseCheckModel)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: PostSystemUseCheck({systemUseCheckModel})");

                string data = JsonConvert.SerializeObject(systemUseCheckModel, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                StringContent httpContent = new StringContent(data, Encoding.UTF8, ConstantManager.StringContent_MediaType);

                HttpResponseMessage response = CoPlatformWebClient.Instance.Post(ConstantManager.APIKey_SystemUseCheck, httpContent);

                string result = response.Content.ReadAsStringAsync().Result;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());

                return JsonConvert.DeserializeObject<SystemUseCheckModel>(result);
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
                return null;
            }
        }

        public static void DeleteSystemUseCheck(this CoPlatformWebClient client, SystemUseCheckModel systemUseCheckModel)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                Logger.Log(LogLevels.I, $"Service: DeleteSystemUseCheck({systemUseCheckModel})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Delete(ConstantManager.APIKey_SystemUseCheck + "windowName=" + systemUseCheckModel.WindowName + "&" + "account=" + systemUseCheckModel.Account);

                string result = response.Content.ReadAsStringAsync().Result;

                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, e.ToString());
                LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.OUT.ToString());
            }
        }

        #endregion

        #region ResourceSensor 데이터 조회

        public static List<DmDataCollectionModel> GetResourceSensorData(this CoPlatformWebClient client, string resourceSensorList)
        {
            try
            {
                Logger.Log(LogLevels.I, $"Service: DataCollection({ resourceSensorList})");

                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_DataCollection + resourceSensorList);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmDataCollectionModel> resultList = JsonConvert.DeserializeObject<List<DmDataCollectionModel>>(result);

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        #endregion

        #region 공정, 설비, 리소스 Master Table

        public static List<DmProcessModel> GetProcessMst(this CoPlatformWebClient client, string code)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_Process + code);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmProcessModel> resultList = JsonConvert.DeserializeObject<List<DmProcessModel>>(result);

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        public static List<DmEquipmentModel> GetEquipmentMst(this CoPlatformWebClient client, string code)
        {
            LogManager.Instance.WriteLine(LOG.API, LOG_LEVEL.InfoLevel, MethodBase.GetCurrentMethod().Name, INOUT.IN.ToString());

            try
            {
                HttpResponseMessage response = CoPlatformWebClient.Instance.Get(ConstantManager.APIKey_Equipment + code);

                string result = response.Content.ReadAsStringAsync().Result;

                List<DmEquipmentModel> resultList = JsonConvert.DeserializeObject<List<DmEquipmentModel>>(result);

                return resultList;
            }
            catch (Exception e)
            {
                Logger.Log(LogLevels.E, e.Message + " : " + e.StackTrace);
                return null;
            }
        }

        #endregion
    }
}
