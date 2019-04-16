namespace BreatheLight.Core.Interfaces
{
    public interface ILightRegulator
    {
        /// <summary>
        /// 设置亮度0~100.100最亮
        /// </summary>
        /// <param name="brightness"></param>
        void SetLightBrightness(float brightness);
        void SetLightStatus(bool isOpen);
    }
}