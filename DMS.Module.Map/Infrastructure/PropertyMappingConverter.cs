using System.Text;

namespace DMS.Module.Map.Infrastructure
{
    /// <summary>
    /// 속성 명칭 변경 시 구버전 지원을 위한 클래스
    /// </summary>
    public class PropertyMappingConverter
    {
        public PropertyMappingConverter()
        {

        }

        public string ConverterPropertyName(string dmcomponentList)
        {
            if (dmcomponentList == null)
            {
                return null;
            }

            var result = ConverterNestedDonutSeries2DProperty(dmcomponentList);

            return result;
        }

        private string ConverterNestedDonutSeries2DProperty(string jsonData)
        {
            StringBuilder builder = new StringBuilder(jsonData);

            // 내부 구멍 크기
            jsonData.Replace("NestedDonutSeries2DHoleRadiusPercent", "HoleRadiusPercent");

            // 회전
            jsonData.Replace("NestedDonutSeries2DRotation", "Rotation");

            jsonData = builder.ToString();

            return jsonData;
        }
    }
}
