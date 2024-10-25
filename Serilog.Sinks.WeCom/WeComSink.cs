using System.Net.Http.Json;
using System.Text;
using System.Threading.Channels;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Formatting.Display;

namespace Serilog.Sinks.WeCom;

/// <summary>
/// 企业微信插槽
/// </summary>
public sealed class WeComSink : ILogEventSink, IDisposable, IAsyncDisposable
{
    private readonly HttpClient _client;
    private readonly WeComSinkOptions _options;
    private readonly Channel<LogEvent> _channel;
    private readonly MessageTemplateTextFormatter _formatter;
    private Task? _task;
    private int _disposeFlag;

    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="client">Http客户端</param>
    /// <param name="options">企业微信插槽选项</param>
    public WeComSink(HttpClient client, WeComSinkOptions options)
    {
        _client = client;
        _options = options;
        if (options.ChannelCapacity == null)
        {
            _channel = Channel.CreateUnbounded<LogEvent>();
        }
        else
        {
            _channel = Channel.CreateBounded<LogEvent>(new BoundedChannelOptions(options.ChannelCapacity.Value)
            {
                FullMode = BoundedChannelFullMode.DropWrite,
                SingleReader = true
            });
        }

        _task = Task.Factory.StartNew(ProcessLogEventTask);
        _formatter = new MessageTemplateTextFormatter("{Message}");
    }

    /// <inheritdoc />
    public void Emit(LogEvent logEvent)
    {
        // 小于最低日志等级
        if (logEvent.Level < _options.MinimumLevel) return;

        if (_options.OnlyException && logEvent.Exception == null) return;

        if (_options.IgnoreExceptions is { Count: > 0 } && logEvent.Exception != null)
        {
            var fullName = logEvent.Exception.GetType().FullName;
            if (fullName != null && _options.IgnoreExceptions.Contains(fullName))
            {
                return;
            }
        }

        _channel.Writer.TryWrite(logEvent);
    }

    /// <inheritdoc />
    public void Dispose() => DisposeAsync().AsTask().GetAwaiter().GetResult();

    /// <inheritdoc />
    public async ValueTask DisposeAsync()
    {
        var disposeFlag = Interlocked.CompareExchange(ref _disposeFlag, 1, 0);
        if (disposeFlag != 0) return;

        if (_task != null)
        {
            _channel.Writer.Complete();
            await _task;
            _task = null;
        }
    }

    /// <summary>处理日志事件任务</summary>
    private async Task ProcessLogEventTask()
    {
        while (await _channel.Reader.WaitToReadAsync())
        {
            while (_channel.Reader.TryRead(out var logEvent))
            {
                try
                {
                    await SendLogEventAsync(logEvent);
                }
                catch (Exception e)
                {
                    SelfLog.WriteLine("Failed to send log event to WeCom: {0}", e);
                }
            }
        }
    }

    /// <summary>发送日志事件</summary>
    private async Task SendLogEventAsync(LogEvent logEvent)
    {
        // ReSharper disable once UseAwaitUsing
        using var stringWriter = new StringWriter();
        _formatter.Format(logEvent, stringWriter);
        // 获取日志信息
        var message = stringWriter.ToString();
        // 截断日志信息
        if (_options.MaximumLogCharLength != null && message.Length > _options.MaximumLogCharLength.Value)
        {
            message = message.Substring(0, _options.MaximumLogCharLength.Value);
        }

        // 获取异常信息
        var exception = logEvent.Exception?.ToString();
        // 截断异常信息
        if (_options.MaximumExceptionCharLength != null &&
            exception?.Length > _options.MaximumExceptionCharLength.Value)
        {
            exception = exception.Substring(0, _options.MaximumExceptionCharLength.Value);
        }

        var sb = new StringBuilder();
        // 标题
        if (!string.IsNullOrEmpty(_options.Title))
        {
            sb.AppendLine(_options.Title);
        }

        // 自定义属性
        if (_options.Properties is { Count: > 0 })
        {
            foreach (var pair in _options.Properties)
            {
                sb.AppendLine($">**{pair.Key}:** <font color=\"comment\">{pair.Value}</font>");
            }
        }

        // 时间
        sb.AppendLine(
            $">**DateTime:** <font color=\"comment\">{logEvent.Timestamp.ToString(_options.DateTimeFormat)}</font>");
        // 日志等级
        sb.AppendLine($">**Level:** <font color=\"comment\">{logEvent.Level}</font>");
        // 日志消息
        sb.AppendLine($">**Message:** <font color=\"info\">{message}</font>");
        // 异常
        if (!string.IsNullOrEmpty(exception))
        {
            sb.AppendLine($">**Exception:** <font color=\"warning\">{exception}</font>");
        }

        // 发送至企业微信机器人
        await _client.PostAsJsonAsync(_options.Webhook, new WeComMessage
        {
            Markdown = new WeComMarkdown
            {
                Content = sb.ToString()
            }
        }, WeComMessageContent.Default.WeComMessage);
    }
}