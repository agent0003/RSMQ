# Redis Simple Message Queue 
Very basic subset of the [RSMQ node module](https://github.com/smrchy/rsmq) for C#

At the moment just containing SendMessage support.

## Example

```c#
using StackExchange.Redis;
using RSMQ;

var rsmq = new RedisSMQ(ConnectionMultiplexer.Connect("<connectionstring>"));
rsmq.SendMessage(new MessageOptions {
	Message = "message",
	QueueName = "kiosk-queue"
});
```
