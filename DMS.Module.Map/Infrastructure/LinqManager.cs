using DMS.Module.Map.Model.RestAPI;
using DMS.Module.Map.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DMS.Module.Map.Infrastructure
{
    public static class LinqManager
    {
        #region FirstOrDefault

        /// <summary>
        /// 항목: ResourceSensor
        /// 설명: ResourceSensor 명칭 찾기 위한 용도
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DmResourceSensorModel FilterFirstResource(string code)
        {
            DmResourceSensorModel resultLinq = BaseSingletonManager.Instance.ResourceSensorList.FirstOrDefault(p =>
            {
                return p.ResourceCode == code;
            });

            return resultLinq;
        }

        /// <summary>
        /// 항목: Code
        /// 설명: Code 정보로 공정 구성 항목 조회
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DmComponentMst FilterFirstRoutingCode(string code)
        {
            DmComponentMst resultLinq = BaseSingletonManager.Instance.AssetList.FirstOrDefault(p =>
            {
                return p.RoutingCode == code;
            });

            return resultLinq;
        }

        #endregion

        #region Where

        /// <summary>
        /// 항목: 공정구성, 선형, 막대형, 데이터 상자
        /// 설명: 알람상태 정보 갱신을 위한 용도
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static IEnumerable<DmComponentMst> FilterWhereAllAlarmStatus()
        {
            IEnumerable<DmComponentMst> resultLinq = BaseSingletonManager.Instance.AssetList.Where(p =>
            {
                return
                p.RoutingType == ComponentType.DM601001.ToString() ||
                p.RoutingType == ComponentType.DM601002.ToString() ||
                p.RoutingType == ComponentType.DM601003.ToString() ||
                p.RoutingType == ComponentType.DM601004.ToString() ||
                p.RoutingType == ComponentType.DM601005.ToString() ||
                p.RoutingType == ComponentType.DM801001.ToString() ||
                p.RoutingType == ComponentType.DM801002.ToString() ||
                p.RoutingType == ComponentType.DM801003.ToString() ||
                p.RoutingType == ComponentType.Gif.ToString();
            });

            return resultLinq;
        }

        /// <summary>
        /// 항목: 공정 구성
        /// 설명: 공정 구성 항목 데이터
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DmComponentMst> FilterWhereRoutingConfiguration()
        {
            IEnumerable<DmComponentMst> resultLinq = BaseSingletonManager.Instance.AssetList.Where(p =>
            {
                return
                p.RoutingType == ComponentType.DM601001.ToString() ||
                p.RoutingType == ComponentType.DM601002.ToString() ||
                p.RoutingType == ComponentType.DM601003.ToString() ||
                p.RoutingType == ComponentType.DM601004.ToString() ||
                p.RoutingType == ComponentType.DM601005.ToString() ||
                p.RoutingType == ComponentType.Screen.ToString();
            });

            return resultLinq;
        }

        public static IEnumerable<DmComponentMst> FilterWhereAlarmCode()
        {
            IEnumerable<DmComponentMst> resultLinq = BaseSingletonManager.Instance.AssetList.Where(p =>
            {
                return
                (p.RoutingType == ComponentType.DM601001.ToString() ||
                p.RoutingType == ComponentType.DM601002.ToString() ||
                p.RoutingType == ComponentType.DM601003.ToString() ||
                p.RoutingType == ComponentType.DM601004.ToString() ||
                p.RoutingType == ComponentType.DM601005.ToString() ||
                p.RoutingType == ComponentType.Screen.ToString()) && p.PropertyM.AlarmCode != null;
            });

            return resultLinq ?? Enumerable.Empty<DmComponentMst>();
        }

        /// <summary>
        /// 항목: 공정 구성
        /// 설명: 공정 구성 항목 데이터
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DmComponentMst> FilterWhereRoutingConfiguration(int maptype)
        {
            IEnumerable<DmComponentMst> resultLinq = BaseSingletonManager.Instance.AssetList.Where(p =>
            {
                return p.MapType == maptype //USER 모드 항목은 제외
                && (p.RoutingType == ComponentType.DM601001.ToString()
                || p.RoutingType == ComponentType.DM601002.ToString()
                || p.RoutingType == ComponentType.DM601003.ToString()
                || p.RoutingType == ComponentType.DM601004.ToString()
                || p.RoutingType == ComponentType.DM601005.ToString()
                || p.RoutingType == ComponentType.Screen.ToString());
            }).OrderBy(p => p.DspSort);

            return resultLinq;
        }

        /// <summary>
        /// 항목: 공정, 작업장, 설비, 단말기(상위공정구성(리소스 제외))
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DmComponentMst> FilterWhereUpperAlarmStatus()
        {
            IEnumerable<DmComponentMst> resultLinq = BaseSingletonManager.Instance.AssetList.Where(p =>
            {
                return
                p.RoutingType == ComponentType.DM601001.ToString() ||
                p.RoutingType == ComponentType.DM601002.ToString() ||
                p.RoutingType == ComponentType.DM601003.ToString() ||
                p.RoutingType == ComponentType.DM601004.ToString();
            });

            return resultLinq;
        }
        /// <summary>
        /// 항목: 레이아웃에 맵핑된 컴포넌트
        /// 설명: 레이아웃 활성화시 해당 레이아웃에 맵핑된 컴포넌트 조건 조회
        /// 주의: 자기 자신은 레이아웃에 올리면 안되므로 자기 자신을 조건 조회에서 제외 처리
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static IEnumerable<DmComponentMst> FilterWhereLayout(string code)
        {
            IEnumerable<DmComponentMst> resultLinq = BaseSingletonManager.Instance.AssetList.Where(p =>
            {
                return (p.PRoutingCode == code) && (p.PRoutingCode != p.RoutingCode);
            }).OrderBy(p => p.Zindex);

            return resultLinq;
        }

        /// <summary>
        /// 항목: 알람 히스토리 컴포넌트
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DmComponentMst> FilterWhereAlarmHistory()
        {
            IEnumerable<DmComponentMst> resultLinq = BaseSingletonManager.Instance.AssetList.Where(p =>
            {
                return p.RoutingType == ComponentType.AlarmHistory.ToString();
            });

            return resultLinq ?? Enumerable.Empty<DmComponentMst>();
        }

        public static IEnumerable<DmComponentMst> FilterWhereImageComponent()
        {
            IEnumerable<DmComponentMst> resultLinq = BaseSingletonManager.Instance.AssetList.Where(p =>
            {
                return p.RoutingType == ComponentType.DM701001.ToString();
            });

            return resultLinq ?? Enumerable.Empty<DmComponentMst>();
        }

        #endregion

        #region 타입: BaseInfoTypeList

        /// <summary>
        /// 타입
        /// 설명: BaseInfoType 조건 조회
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DmBaseInfoTypeModel FilterFirstBaseInfoType(string code)
        {
            DmBaseInfoTypeModel resultLinq = BaseSingletonManager.Instance.BaseInfoTypeList.FirstOrDefault(p =>
            {
                return p.Type == code;
            });

            return resultLinq;
        }

        #endregion

        #region ResourceSensor

        /// <summary>
        /// ResourceSensor
        /// 설명: ResourceSensor 조건 조회
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DmResourceSensorModel FilterFirstResourceSensor(string code)
        {
            DmResourceSensorModel resultLinq = BaseSingletonManager.Instance.ResourceSensorList.FirstOrDefault(p =>
            {
                return p.ResourceCode == code;
            });

            return resultLinq;
        }

        #endregion
    }
}
