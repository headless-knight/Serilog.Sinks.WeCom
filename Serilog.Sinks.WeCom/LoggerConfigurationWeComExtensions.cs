using Serilog.Configuration;
using Serilog.Events;

namespace Serilog.Sinks.WeCom;

/// <summary>
/// 日志配置企业微信拓展
/// </summary>
public static class LoggerConfigurationWeComExtensions
{
    /// <summary>
    /// 添加企业微信插槽
    /// </summary>
    /// <param name="configuration">日志插槽配置</param>
    /// <param name="options">企业微信插槽选项</param>
    /// <param name="client">Http客户端，为null时内部创建，默认为null</param>
    /// <returns>日志插槽配置</returns>
    public static LoggerConfiguration WeCom(this LoggerSinkConfiguration configuration, WeComSinkOptions options,
        HttpClient? client = null)
    {
        if (string.IsNullOrWhiteSpace(options.Webhook))
        {
            throw new ArgumentException("WeCom robot webhook is required");
        }

        if (options.ChannelCapacity is < 0)
        {
            throw new ArgumentException("ChannelCapacity must be greater than or equal to 0");
        }

        if (options.MaximumLogCharLength is < 0)
        {
            throw new ArgumentException("MaximumLogCharLength must be greater than or equal to 0");
        }

        if (options.MaximumExceptionCharLength is < 0)
        {
            throw new ArgumentException("MaximumExceptionCharLength must be greater than or equal to 0");
        }

        return configuration.Sink(new WeComSink(client ?? new HttpClient(), options));
    }

    /// <summary>
    /// 添加企业微信插槽
    /// </summary>
    /// <param name="configuration">日志插槽配置</param>
    /// <param name="webhook">企业微信机器人Webhook</param>
    /// <param name="minimumLevel">最小日志等级</param>
    /// <param name="title">标题</param>
    /// <param name="properties">自定义属性</param>
    /// <param name="dateTimeFormat">日期时间格式</param>
    /// <param name="channelCapacity">通道容量</param>
    /// <param name="maximumLogCharLength">最大日志字符长度</param>
    /// <param name="maximumExceptionCharLength">最大异常字符长度</param>
    /// <param name="onlyException">是否仅发送有异常的日志</param>
    /// <param name="ignoreExceptions">忽略的异常类型</param>
    /// <returns>日志插槽配置</returns>
    public static LoggerConfiguration WeCom(this LoggerSinkConfiguration configuration, string webhook,
        LogEventLevel minimumLevel = Constants.DefaultMinimumLevel,
        string title = Constants.DefaultTitle,
        Dictionary<string, string>? properties = null,
        string dateTimeFormat = Constants.DefaultDateTimeFormat,
        int? channelCapacity = null,
        int? maximumLogCharLength = null,
        int? maximumExceptionCharLength = null,
        bool onlyException = false,
        HashSet<string>? ignoreExceptions = null)
    {
        return configuration.WeCom(new WeComSinkOptions
        {
            Webhook = webhook,
            MinimumLevel = minimumLevel,
            Title = title,
            Properties = properties,
            DateTimeFormat = dateTimeFormat,
            ChannelCapacity = channelCapacity,
            MaximumLogCharLength = maximumLogCharLength,
            MaximumExceptionCharLength = maximumExceptionCharLength,
            OnlyException = onlyException,
            IgnoreExceptions = ignoreExceptions
        });
    }
}