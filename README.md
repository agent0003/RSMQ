# Redis Simple Message Queue 
Very basic subset of the [RSMQ node module](https://github.com/smrchy/rsmq) for C#. 
[StackExchange Redis client](https://github.com/StackExchange/StackExchange.Redis).

At the moment just containing SendMessage support.

## Example

```c#
using StackExchange.Redis;
using RSMQ;

// reuse this:
var redisConnection = ConnectionMultiplexer.Connect("<connectionstring>");

var rsmq = new RedisSMQ(redisConnection);
rsmq.SendMessage(new RSMQMessage {
	Message = "message",
	QueueName = "queue name"
});
```
