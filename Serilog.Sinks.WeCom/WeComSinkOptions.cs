using Serilog.Events;

namespace Serilog.Sinks.WeCom;

/// <summary>
/// 企业微信插槽选项
/// </summary>
public class WeComSinkOptions
{
    /// <summary>
    /// 机器人Webhook
    /// </summary>
    public string Webhook { get; set; } = string.Empty;

    /// <summary>
    /// 最低日志等级
    /// </summary>
    public LogEventLevel MinimumLevel { get; set; } = Constants.DefaultMinimumLevel;

    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; } = Constants.DefaultTitle;

    /// <summary>
    /// 自定义属性
    /// </summary>
    public Dictionary<string, string>? Properties { get; set; }

    /// <summary>
    /// 日期时间格式化
    /// </summary>
    public string DateTimeFormat { get; set; } = Constants.DefaultDateTimeFormat;

    /// <summary>
    /// 通道容量，若为null，则不限制
    /// </summary>
    public int? ChannelCapacity { get; set; }

    /// <summary>
    /// 日志发送最大字符长度，若为null，则不限制
    /// </summary>
    public int? MaximumLogCharLength { get; set; }

    /// <summary>
    /// 异常发送最大字符长度，若为null，则不限制
    /// </summary>
    public int? MaximumExceptionCharLength { get; set; }

    /// <summary>
    /// 是否仅发送有异常的日志
    /// </summary>
    public bool OnlyException { get; set; }

    /// <summary>
    /// 忽略的异常类型
    /// </summary>
    public HashSet<string>? IgnoreExceptions { get; set; }
}