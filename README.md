# Redis Simple Message Queue 
Basic subset of the [RSMQ node module](https://github.com/smrchy/rsmq) for C#

At the moment just containing SendMessage support.

## Example

```c#
var rsmq = new RSMQ.RSMQ("<connectionstring>"));
rsmq.SendMessage(new RSMQ.MessageOptions {
	Message = @"{ ""kioskId"": ""GmZWMn3fifcjEqSyAAAC"", ""type"": ""reload"" }",
	QueueName = "kiosk-queue"
});
```
