# Serilog.Sinks.WeCom

一个用于向企业微信机器人发送消息的Serilog日志插槽

## 快速上手

### 添加NuGet包

```shell
dotnet add package Serilog.Sinks.WeCom
```

### 使用方式

```csharp
// 配置Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.WeCom("https://qyapi.weixin.qq.com/cgi-bin/webhook/send?key=example")
    .CreateLogger();
// 发送日志
Log.Error("Test message");
```

接下来，您就可以在企业微信中查看机器人发送的日志了

### 配置参数

详细可配置参数如下：

| 参数                         | 类型                         | 描述                     | 是否必须 | 默认值                                        |
|:--------------------------:|:--------------------------:|:----------------------:|:----:|:------------------------------------------:|
| Webhook                    | string                     | 企业微信机器人地址              | 是    |                                            |
| MinimumLevel               | LogEventLevel              | 最低发送日志等级               |      | Information                                |
| Title                      | string                     | 标题，随日志一起发送             |      | This message send by Serilog\.Sinks\.WeCom |
| Properties                 | Dictionary<string, string> | 自定义属性，随日志一起发送          |      |                                            |
| DateTimeFormat             | string                     | 日期时间格式化                |      | yyyy\-MM\-dd HH:mm:ss\.fff                 |
| ChannelCapacity            | int?                       | 发送日志队列容量，为null表示不限制    |      | null                                       |
| MaximumLogCharLength       | int?                       | 日志发送最大字符长度，若为null，则不限制 |      | null                                       |
| MaximumExceptionCharLength | int?                       | 异常发送最大字符长度，若为null，则不限制 |      | null                                       |
| OnlyException              | bool                       | 是否仅发送有异常的日志            |      | false                                      |
| IgnoreExceptions           | HashSet<string>            | 忽略的异常类型全名              |      |                                            |
