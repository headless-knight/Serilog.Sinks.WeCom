using Serilog.Events;

namespace Serilog.Sinks.WeCom;

/// <summary>
/// 常量类
/// </summary>
internal static class Constants
{
    /// <summary>
    /// 默认最小日志级别
    /// </summary>
    public const LogEventLevel DefaultMinimumLevel = LogEventLevel.Information;

    /// <summary>
    /// 默认标题
    /// </summary>
    public const string DefaultTitle = "This message send by Serilog.Sinks.WeCom";

    /// <summary>
    /// 默认日期时间格式
    /// </summary>
    public const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
}